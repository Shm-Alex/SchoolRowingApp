// Domain/Membership/IMembershipPeriodRepository.cs
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Membership;

/// <summary>
/// Репозиторий для работы с периодами членства.
/// Предоставляет методы для управления периодами (месяцами) членства в школе.
/// </summary>
public interface IMembershipPeriodRepository
{
    /// <summary>
    /// Получает период членства по ID.
    /// </summary>
    /// <param name="id">ID периода</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Период членства или null, если не найден</returns>
    Task<MembershipPeriod> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Получает период членства по году и месяцу.
    /// Используется для проверки существования периода перед созданием новых записей.
    /// </summary>
    /// <param name="year">Год</param>
    /// <param name="month">Месяц (1-12)</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Период членства или null, если не найден</returns>
    Task<MembershipPeriod> GetByYearAndMonthAsync(int year, int month, CancellationToken ct);

    /// <summary>
    /// Получает все периоды членства, отсортированные по дате.
    /// Используется для отображения календаря членства в UI.
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список всех периодов членства</returns>
    Task<List<MembershipPeriod>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Добавляет новый период членства.
    /// Используется при инициализации системы или добавлении будущих периодов.
    /// </summary>
    /// <param name="period">Период членства</param>
    /// <param name="ct">Токен отмены</param>
    Task AddAsync(MembershipPeriod period, CancellationToken ct);

    /// <summary>
    /// Обновляет период членства.
    /// Используется при изменении базового взноса для будущих периодов.
    /// </summary>
    /// <param name="period">Период членства</param>
    /// <param name="ct">Токен отмены</param>
    Task UpdateAsync(MembershipPeriod period, CancellationToken ct);

    /// <summary>
    /// Удаляет период членства.
    /// Используется редко, только в случае ошибки при инициализации.
    /// </summary>
    /// <param name="period">Период членства</param>
    /// <param name="ct">Токен отмены</param>
    Task DeleteAsync(MembershipPeriod period, CancellationToken ct);
}