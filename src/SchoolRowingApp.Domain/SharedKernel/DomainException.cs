using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Как обрабатывать DomainException в API?
// Добавь в Program.cs
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

// Создай обработчик
public class DomainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DomainExceptionHandler> _logger;

    public DomainExceptionHandler(ILogger<DomainExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        if (exception is DomainException domainException)
        {
            _logger.LogWarning("Domain exception: {Message}", domainException.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ошибка бизнес-логики",
                Detail = domainException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            
            return true;
        }
        
        return false;
    }
}
# Дополнительные улучшения#
 ## Создай специализированные исключения ##

// Domain/SharedKernel/PaymentDomainException.cs
public class PaymentDomainException : DomainException
{
    public PaymentDomainException(string message) : base(message) { }
}

// Domain/SharedKernel/PayerDomainException.cs
public class PayerDomainException : DomainException
{
    public PayerDomainException(string message) : base(message) { }
}
## Используй валидацию с FluentValidation ##
// Application/Payments/CreatePaymentCommandValidator.cs
public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Сумма платежа должна быть положительной");
    }
}
 */
namespace SchoolRowingApp.Domain.SharedKernel;

public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
