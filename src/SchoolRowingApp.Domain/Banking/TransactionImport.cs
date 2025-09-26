// Domain/Banking/TransactionImport.cs
using SchoolRowingApp.Domain.Common;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Banking;

/// <summary>
/// Запись о факте импорта файла с операциями
/// </summary>
public class TransactionImport : GuidEntity
{
    /// <summary>
    /// Имя загруженного файла
    /// </summary>
    public string FileName { get; private set; }

    /// <summary>
    /// Дата и время импорта
    /// </summary>
    public DateTime ImportDate { get; private set; }

    /// <summary>
    /// Количество успешно импортированных операций
    /// </summary>
    public int SuccessCount { get; private set; }

    /// <summary>
    /// Количество пропущенных операций (уже существующих)
    /// </summary>
    public int SkippedCount { get; private set; }

    /// <summary>
    /// Количество ошибок при импорте
    /// </summary>
    public int ErrorCount { get; private set; }

    /// <summary>
    /// Общее количество строк в файле
    /// </summary>
    public int TotalRows { get; private set; }

    /// <summary>
    /// Хэш содержимого файла для проверки на повторный импорт
    /// </summary>
    public string FileHash { get; private set; }

    // Навигационные свойства
    public virtual ICollection<TransactionImportDetail> ImportDetails { get; private set; } = new List<TransactionImportDetail>();
    public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    /// <summary>
    /// Создает запись об импорте
    /// </summary>
    /// <param name="fileName">Имя файла</param>
    /// <param name="fileHash">Хэш файла</param>
    /// <param name="totalRows">Общее количество строк</param>
    public TransactionImport(string fileName, string fileHash, int totalRows)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new DomainException("Имя файла обязательно");

        if (string.IsNullOrWhiteSpace(fileHash))
            throw new DomainException("Хэш файла обязателен");

        FileName = fileName;
        ImportDate = DateTime.UtcNow;
        TotalRows = totalRows;
        FileHash = fileHash;
        SuccessCount = 0;
        SkippedCount = 0;
        ErrorCount = 0;
    }

    /// <summary>
    /// Обновляет статистику импорта
    /// </summary>
    /// <param name="successCount">Количество успешных операций</param>
    /// <param name="skippedCount">Количество пропущенных операций</param>
    /// <param name="errorCount">Количество ошибок</param>
    public void UpdateStatistics(int successCount, int skippedCount, int errorCount)
    {
        SuccessCount = successCount;
        SkippedCount = skippedCount;
        ErrorCount = errorCount;
        UpdateLastModified();
    }
}