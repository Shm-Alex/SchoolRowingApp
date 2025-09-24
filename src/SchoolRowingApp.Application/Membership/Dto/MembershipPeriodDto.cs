using SchoolRowingApp.Domain.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRowingApp.Application.Membership.Dto
{
    /// <summary>
    /// DTO для представления периода членства в системе.
    /// Используется для передачи данных о периоде членства через API.
    /// </summary>
    public record  MembershipPeriodDto(int Month, int Year, decimal BaseFee)
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса MembershipPeriodDto на основе доменной сущности MembershipPeriod.
        /// Преобразует доменную модель в DTO.
        /// </summary>
        /// <param name="membershipPeriod">Доменная сущность периода членства</param>
        public MembershipPeriodDto(MembershipPeriod membershipPeriod)
            : this(membershipPeriod.Month, membershipPeriod.Year, membershipPeriod.BaseFee) { }
    }
    /// <summary>
    /// <see cref="SchoolRowingApp.Application.Membership.Dto.MembershipPeriodDto"/>
    /// <see cref="SchoolRowingApp.Domain.Membership.AthleteMembership"/>
    /// </summary>
    public record AthleteMembershipDto(int MembershipPeriodMonth, int MembershipPeriodYear, decimal ParticipationCoefficient)
    {
        public AthleteMembershipDto(SchoolRowingApp.Domain.Membership.AthleteMembership athleteMembership)
            : this(athleteMembership.MembershipPeriodMonth, athleteMembership.MembershipPeriodYear, athleteMembership.ParticipationCoefficient) { }
    }
}
