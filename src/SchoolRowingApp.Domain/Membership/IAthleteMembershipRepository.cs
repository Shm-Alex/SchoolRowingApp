// Domain/Membership/IAthleteMembershipRepository.cs
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Membership;

/// <summary>
/// Репозиторий для работы с членством атлетов.
/// Предоставляет методы для управления записями о членстве атлетов в разные периоды.
/// </summary>
public interface IAthleteMembershipRepository
{
    /// <summary>
    /// Получает запись о членстве по составному ключу.
    /// Так как AthleteMembership имеет составной первичный ключ (AthleteId, MembershipPeriodId),
    /// этот метод не используется напрямую.
    /// </summary>
    /// <param name="id">ID записи (не используется)</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Запись о членстве или null</returns>
    Task<AthleteMembership> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Получает все записи о членстве для указанного атлета.
    /// Используется для отображения истории членства атлета в UI.
    /// </summary>
    /// <param name="athleteId">ID атлета</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список записей о членстве атлета</returns>
    Task<List<AthleteMembership>> GetByAthleteIdAsync(Guid athleteId, CancellationToken ct);

    /// <summary>
    /// Получает все записи о членстве для указанного периода.
    /// Используется для отображения таблицы членства по месяцам в UI.
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список записей о членстве в указанный период</returns>
    Task<List<AthleteMembership>> GetByPeriodAsync(int membershipPeriodMonth, int membershipPeriodYear, CancellationToken ct);

    /// <summary>
    /// Добавляет запись о членстве.
    /// Используется при установке членства атлета через UI.
    /// </summary>
    /// <param name="membership">Запись о членстве</param>
    /// <param name="ct">Токен отмены</param>
    Task AddAsync(AthleteMembership membership, CancellationToken ct);

    /// <summary>
    /// Обновляет запись о членстве.
    /// Используется при изменении коэффициента участия через UI.
    /// </summary>
    /// <param name="membership">Запись о членстве</param>
    /// <param name="ct">Токен отмены</param>
    Task UpdateAsync(AthleteMembership membership, CancellationToken ct);

    /// <summary>
    /// Удаляет запись о членстве.
    /// Используется при удалении записи через UI.
    /// </summary>
    /// <param name="membership">Запись о членстве</param>
    /// <param name="ct">Токен отмены</param>
    Task DeleteAsync(AthleteMembership membership, CancellationToken ct);
}