// Application/Membership/Validators/UpdateExistingPeriodsCommandValidator.cs
using FluentValidation;
using SchoolRowingApp.Application.Membership.Commands;

namespace SchoolRowingApp.Application.Membership.Validators;

public class UpdateExistingPeriodsCommandValidator : AbstractValidator<CreateOrUpdatePeriodsCommand>
{
    public UpdateExistingPeriodsCommandValidator()
    {
        // Проверка начального месяца
        RuleFor(x => x.startMonth)
            .InclusiveBetween(1, 12)
            .WithMessage("Начальный месяц должен быть в диапазоне от 1 до 12");

        // Проверка конечного месяца
        RuleFor(x => x.endMonth)
            .InclusiveBetween(1, 12)
            .WithMessage("Конечный месяц должен быть в диапазоне от 1 до 12");

        // Проверка диапазона дат
        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("Начальная дата не может быть позже конечной даты");

        // Проверка базового взноса
        RuleFor(x => x.BaseFee)
            .GreaterThan(0)
            .WithMessage("Базовый взнос должен быть положительным числом");
    }

    private bool HaveValidDateRange(CreateOrUpdatePeriodsCommand command)
    {
        var startDate = new DateTime(command.startYear, command.startMonth, 1);
        var endDate = new DateTime(command.endYear, command.endMonth, 1);
        return startDate <= endDate;
    }
}