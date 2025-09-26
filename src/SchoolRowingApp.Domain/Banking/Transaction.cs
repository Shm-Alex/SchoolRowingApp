// Domain/Banking/Transaction.cs
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Banking;

/// <summary>
/// Сущность банковской операции (платежа)
/// Уникальность определяется комбинацией даты операции, суммы и валюты
/// </summary>
public class Transaction : CompositeKeyEntity
{
    /// <summary>
    /// Дата и время операции
    /// Часть составного первичного ключа
    /// </summary>
    public DateTime OperationDate { get; private set; }

    /// <summary>
    /// Дата платежа
    /// </summary>
    public DateOnly PaymentDate { get; private set; }

    /// <summary>
    /// Последние 4 цифры номера карты (может быть пустым)
    /// </summary>
    public string? CardLastDigits { get; private set; }

    /// <summary>
    /// Статус операции (OK, FAILED)
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Сумма операции в рублях
    /// Часть составного первичного ключа
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Валюта операции
    /// Часть составного первичного ключа
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// Сумма платежа
    /// </summary>
    public decimal PaymentAmount { get; private set; }

    /// <summary>
    /// Валюта платежа
    /// </summary>
    public string PaymentCurrency { get; private set; }

    /// <summary>
    /// Кэшбэк (если есть)
    /// </summary>
    public decimal? Cashback { get; private set; }

    /// <summary>
    /// Категория операции
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// MCC код (Merchant Category Code)
    /// </summary>
    public string? MccCode { get; private set; }

    /// <summary>
    /// Описание операции
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Бонусы (включая кэшбэк)
    /// </summary>
    public decimal BonusAmount { get; private set; }

    /// <summary>
    /// Округление на инвесткопилку
    /// </summary>
    public decimal RoundUpAmount { get; private set; }

    /// <summary>
    /// Сумма операции с округлением
    /// </summary>
    public decimal OperationAmountWithRoundUp { get; private set; }

    /// <summary>
    /// Флаг: была ли операция уже обработана
    /// </summary>
    public bool IsProcessed { get; private set; }

    // Навигационные свойства
    public virtual ICollection<TransactionImport> TransactionImports { get; private set; } = new List<TransactionImport>();

    /// <summary>
    /// Создает новую банковскую операцию
    /// Первичный ключ формируется из OperationDate, Amount, Currency
    /// </summary>
    /// <param name="operationDate">Дата и время операции</param>
    /// <param name="paymentDate">Дата платежа</param>
    /// <param name="cardLastDigits">Последние цифры карты (опционально)</param>
    /// <param name="status">Статус операции</param>
    /// <param name="amount">Сумма операции</param>
    /// <param name="currency">Валюта операции</param>
    /// <param name="paymentAmount">Сумма платежа</param>
    /// <param name="paymentCurrency">Валюта платежа</param>
    /// <param name="cashback">Кэшбэк</param>
    /// <param name="category">Категория операции</param>
    /// <param name="mccCode">MCC код</param>
    /// <param name="description">Описание операции</param>
    /// <param name="bonusAmount">Бонусы</param>
    /// <param name="roundUpAmount">Округление на инвесткопилку</param>
    /// <param name="operationAmountWithRoundUp">Сумма операции с округлением</param>
    public Transaction(
        DateTime operationDate,
        DateOnly paymentDate,
        string? cardLastDigits,
        string status,
        decimal amount,
        string currency,
        decimal paymentAmount,
        string paymentCurrency,
        decimal? cashback,
        string category,
        string? mccCode,
        string description,
        decimal bonusAmount,
        decimal roundUpAmount,
        decimal operationAmountWithRoundUp)
    {
        ValidateOperation(operationDate, status, amount, currency, category, description);

        OperationDate = operationDate;
        PaymentDate = paymentDate;
        CardLastDigits = cardLastDigits;
        Status = status;
        Amount = amount;
        Currency = currency;
        PaymentAmount = paymentAmount;
        PaymentCurrency = paymentCurrency;
        Cashback = cashback;
        Category = category;
        MccCode = mccCode;
        Description = description;
        BonusAmount = bonusAmount;
        RoundUpAmount = roundUpAmount;
        OperationAmountWithRoundUp = operationAmountWithRoundUp;
        IsProcessed = false;
    }

    /// <summary>
    /// Отмечает операцию как обработанную
    /// </summary>
    public void MarkAsProcessed()
    {
        IsProcessed = true;
        UpdateLastModified();
    }

    /// <summary>
    /// Валидация данных операции
    /// </summary>
    private void ValidateOperation(
        DateTime operationDate,
        string status,
        decimal amount,
        string currency,
        string category,
        string description)
    {
        if (operationDate > DateTime.UtcNow.AddDays(1))
            throw new DomainException("Дата операции не может быть в будущем");

        if (string.IsNullOrWhiteSpace(status))
            throw new DomainException("Статус операции обязателен");

        if (amount == 0)
            throw new DomainException("Сумма операции не может быть нулевой");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Валюта операции обязательна");

        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("Категория операции обязательна");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Описание операции обязательно");
    }
}