// Domain/Banking/TransactionImportDetail.cs
using SchoolRowingApp.Domain.Common;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Banking;

/// <summary>
/// Детали импорта конкретной операции
/// </summary>
public class TransactionImportDetail : GuidEntity
{
    /// <summary>
    /// Ссылка на импорт
    /// </summary>
    public Guid TransactionImportId { get; private set; }

    /// <summary>
    /// Номер строки в файле
    /// </summary>
    public int RowNumber { get; private set; }

    /// <summary>
    /// Результат импорта строки
    /// </summary>
    public ImportResult Result { get; private set; }

    /// <summary>
    /// Сообщение об ошибке (если есть)
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Исходная строка из файла
    /// </summary>
    public string RawData { get; private set; }

    // Навигационные свойства
    public virtual TransactionImport TransactionImport { get; private set; }

    /// <summary>
    /// Создает деталь импорта
    /// </summary>
    /// <param name="transactionImportId">ID импорта</param>
    /// <param name="rowNumber">Номер строки</param>
    /// <param name="result">Результат импорта</param>
    /// <param name="rawData">Исходные данные</param>
    /// <param name="errorMessage">Сообщение об ошибке (опционально)</param>
    public TransactionImportDetail(
        Guid transactionImportId,
        int rowNumber,
        ImportResult result,
        string rawData,
        string? errorMessage = null)
    {
        TransactionImportId = transactionImportId;
        RowNumber = rowNumber;
        Result = result;
        RawData = rawData;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Обновляет результат импорта
    /// </summary>
    /// <param name="result">Новый результат</param>
    /// <param name="errorMessage">Сообщение об ошибке</param>
    public void UpdateResult(ImportResult result, string? errorMessage = null)
    {
        Result = result;
        ErrorMessage = errorMessage;
        UpdateLastModified();
    }
}

/// <summary>
/// Результат импорта строки
/// </summary>
public enum ImportResult
{
    Success,
    Skipped,
    Error
}