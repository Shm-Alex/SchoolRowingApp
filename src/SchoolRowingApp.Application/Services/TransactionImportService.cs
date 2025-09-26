// Application/Services/TransactionImportService.cs
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using SchoolRowingApp.Domain.Banking;
using System.Globalization;
using System.IO;

namespace SchoolRowingApp.Application.Services;

public class TransactionImportService : ITransactionImportService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionImportRepository _importRepository;
    private readonly ILogger<TransactionImportService> _logger;

    public TransactionImportService(
        ITransactionRepository transactionRepository,
        ITransactionImportRepository importRepository,
        ILogger<TransactionImportService> logger)
    {
        _transactionRepository = transactionRepository;
        _importRepository = importRepository;
        _logger = logger;
    }

    /// <summary>
    /// Импортирует операции из CSV-файла
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <param name="fileName">Имя файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат импорта</returns>
    public async Task<ImportResultDto> ImportFromCsvAsync(string filePath, string fileName, CancellationToken cancellationToken)
    {
        try
        {
            // Проверяем, существует ли уже такой файл
            var fileHash = await CalculateFileHashAsync(filePath);
            var existingImport = await _importRepository.GetByFileHashAsync(fileHash, cancellationToken);

            if (existingImport != null)
            {
                return new ImportResultDto
                {
                    Success = true,
                    Message = "Файл уже был импортирован ранее",
                    ImportId = existingImport.Id,
                    TotalRows = existingImport.TotalRows,
                    SuccessCount = existingImport.SuccessCount,
                    SkippedCount = existingImport.SkippedCount,
                    ErrorCount = existingImport.ErrorCount
                };
            }

            // Читаем файл
            var transactions = await ReadCsvFileAsync(filePath, fileName, cancellationToken);

            if (!transactions.Any())
            {
                return new ImportResultDto
                {
                    Success = false,
                    Message = "Файл не содержит данных"
                };
            }

            // Создаем запись об импорте
            var import = new TransactionImport(fileName, fileHash, transactions.Count);
            await _importRepository.AddAsync(import, cancellationToken);

            int successCount = 0;
            int skippedCount = 0;
            int errorCount = 0;

            // Обрабатываем каждую операцию
            foreach (var (transaction, rowNumber, rawData) in transactions)
            {
                try
                {
                    // Проверяем, существует ли уже такая операция по составному ключу
                    var exists = await _transactionRepository.ExistsAsync(
                        transaction.OperationDate,
                        transaction.Amount,
                        transaction.Currency,
                        cancellationToken);

                    if (exists)
                    {
                        // Создаем деталь импорта для пропущенной операции
                        var detail = new TransactionImportDetail(
                            import.Id,
                            rowNumber,
                            ImportResult.Skipped,
                            rawData,
                            "Операция уже существует (уникальность по дате, сумме и валюте)");

                        await _importRepository.AddImportDetailAsync(detail, cancellationToken);
                        skippedCount++;
                    }
                    else
                    {
                        // Добавляем новую операцию
                        await _transactionRepository.AddAsync(transaction, cancellationToken);

                        // Создаем деталь импорта для успешной операции
                        var detail = new TransactionImportDetail(
                            import.Id,
                            rowNumber,
                            ImportResult.Success,
                            rawData);

                        await _importRepository.AddImportDetailAsync(detail, cancellationToken);
                        successCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при импорте строки {RowNumber}", rowNumber);

                    var detail = new TransactionImportDetail(
                        import.Id,
                        rowNumber,
                        ImportResult.Error,
                        rawData,
                        ex.Message);

                    await _importRepository.AddImportDetailAsync(detail, cancellationToken);
                    errorCount++;
                }
            }

            // Обновляем статистику импорта
            import.UpdateStatistics(successCount, skippedCount, errorCount);
            await _importRepository.UpdateAsync(import, cancellationToken);

            return new ImportResultDto
            {
                Success = true,
                Message = $"Импорт завершен: {successCount} новых, {skippedCount} пропущено, {errorCount} ошибок",
                ImportId = import.Id,
                TotalRows = transactions.Count,
                SuccessCount = successCount,
                SkippedCount = skippedCount,
                ErrorCount = errorCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при импорте файла {FileName}", fileName);
            return new ImportResultDto
            {
                Success = false,
                Message = $"Ошибка при импорте: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Читает CSV-файл и преобразует в список операций
    /// </summary>
    private async Task<List<(Transaction transaction, int rowNumber, string rawData)>> ReadCsvFileAsync(
        string filePath,
        string fileName,
        CancellationToken cancellationToken)
    {
        var result = new List<(Transaction, int, string)>();

        using var reader = new StreamReader(filePath);
        
        using var csvReader = new CsvReader(reader,
                new CsvHelper.Configuration.CsvConfiguration(new CultureInfo("ru-RU"))
                {
                Delimiter= ";",
                HasHeaderRecord=false,
                MissingFieldFound=null,
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                BadDataFound=null,
               
                }
            );
        // Установка маппинга вручную по индексам
        csvReader.Context.RegisterClassMap<CsvTransactionRecordMap>();
        // Пропускаем заголовок
        await csvReader.ReadAsync();

        int rowNumber = 1; // Начинаем с 2, так как первая строка - заголовок

        while (await csvReader.ReadAsync())
        {
            try
            {
                var record = csvReader.GetRecord<CsvTransactionRecord>();
                var transaction = MapToDomainEntity(record, rowNumber);

                var rawData = string.Join(";", csvReader.Parser.Record);

                result.Add((transaction, rowNumber, rawData));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Пропущена строка {RowNumber} в файле {FileName}", rowNumber, fileName);
            }

            rowNumber++;
        }

        return result;
    }

    /// <summary>
    /// Преобразует запись из CSV в доменную сущность
    /// </summary>
    /// <summary>
    /// Преобразует запись из CSV в доменную сущность
    /// </summary>
    private Transaction MapToDomainEntity(CsvTransactionRecord record, int rowNumber)
    {
        // Парсим дату и время операции
        var operationDateTime = DateTime.ParseExact(
            record.OperationDate,
            "dd.MM.yyyy HH:mm:ss",
            CultureInfo.InvariantCulture);

        // Преобразуем в UTC (предполагаем, что время в MSK)
        var utcOperationDateTime = TimeZoneInfo.ConvertTimeToUtc(operationDateTime, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

        // Парсим дату платежа
        var paymentDateParts = record.PaymentDate.Split('.');
        var paymentDate = DateOnly.FromDateTime(new DateTime(
            int.Parse(paymentDateParts[2]),
            int.Parse(paymentDateParts[1]),
            int.Parse(paymentDateParts[0])));

        // Парсим сумму операции
        var amount = decimal.Parse(record.OperationAmount.Replace(",", "."), CultureInfo.InvariantCulture);

        // Парсим сумму платежа
        var paymentAmount = decimal.Parse(record.PaymentAmount.Replace(",", "."), CultureInfo.InvariantCulture);

        // Парсим кэшбэк (может быть пустым)
        decimal? cashback = null;
        if (!string.IsNullOrWhiteSpace(record.Cashback))
        {
            cashback = decimal.Parse(record.Cashback.Replace(",", "."), CultureInfo.InvariantCulture);
        }

        // Парсим бонусы
        var bonusAmount = decimal.Parse(record.BonusAmount.Replace(",", "."), CultureInfo.InvariantCulture);

        // Парсим округление
        var roundUpAmount = decimal.Parse(record.RoundUpAmount.Replace(",", "."), CultureInfo.InvariantCulture);

        // Парсим сумму с округлением
        var operationAmountWithRoundUp = decimal.Parse(record.OperationAmountWithRoundUp.Replace(",", "."), CultureInfo.InvariantCulture);

        return new Transaction(
            utcOperationDateTime,
            paymentDate,
            GetCardLastDigits(record.CardNumber),
            record.Status,
            amount,
            record.OperationCurrency,
            paymentAmount,
            record.PaymentCurrency,
            cashback,
            record.Category,
            record.Mcc,
            record.Description,
            bonusAmount,
            roundUpAmount,
            operationAmountWithRoundUp);
    }

    /// <summary>
    /// Извлекает последние 4 цифры номера карты
    /// </summary>
    private string? GetCardLastDigits(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber)) return null;

        var digits = new string(cardNumber.Where(char.IsDigit).ToArray());
        return digits.Length >= 4 ? digits.Substring(digits.Length - 4) : digits;
    }

    /// <summary>
    /// Вычисляет хэш файла для проверки на дубликаты
    /// </summary>
    private async Task<string> CalculateFileHashAsync(string filePath)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hash = await sha256.ComputeHashAsync(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}


/// <summary>
/// Вспомогательный класс для чтения CSV
/// </summary>
public class CsvTransactionRecord
{
    
    public string OperationDate { get; set; } = string.Empty;
    public string PaymentDate { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string OperationAmount { get; set; } = string.Empty;
    public string OperationCurrency { get; set; } = string.Empty;
    public string PaymentAmount { get; set; } = string.Empty;
    public string PaymentCurrency { get; set; } = string.Empty;
    public string Cashback { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Mcc { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string BonusAmount { get; set; } = string.Empty;
    public string RoundUpAmount { get; set; } = string.Empty;
    public string OperationAmountWithRoundUp { get; set; } = string.Empty;
}
public class CsvTransactionRecordMap : ClassMap<CsvTransactionRecord> {
    public CsvTransactionRecordMap()
    {
        Map(m => m.OperationDate).Index(0);
        Map(m => m.PaymentDate).Index(1);
        Map(m => m.CardNumber).Index(2);
        Map(m => m.Status).Index(3);
        Map(m => m.OperationAmount).Index(4);
        Map(m => m.OperationCurrency).Index(5);
        Map(m => m.PaymentAmount).Index(6);
        Map(m => m.PaymentCurrency).Index(7);
        Map(m => m.Cashback).Index(8);
        Map(m => m.Category).Index(9);
        Map(m => m.Mcc).Index(10);
        Map(m => m.Description).Index(11);
        Map(m => m.BonusAmount).Index(12);
        Map(m => m.RoundUpAmount).Index(13);
        Map(m => m.OperationAmountWithRoundUp).Index(14);
    }
}
/// <summary>
/// Результат импорта
/// </summary>
public class ImportResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? ImportId { get; set; }
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public int SkippedCount { get; set; }
    public int ErrorCount { get; set; }
}

/// <summary>
/// Интерфейс сервиса импорта
/// </summary>
public interface ITransactionImportService
{
    Task<ImportResultDto> ImportFromCsvAsync(string filePath, string fileName, CancellationToken cancellationToken);
}