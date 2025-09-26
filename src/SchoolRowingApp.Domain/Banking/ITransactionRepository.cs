// Domain/Interfaces/Repositories/ITransactionRepository.cs
using SchoolRowingApp.Domain.Banking;

namespace SchoolRowingApp.Domain.Banking;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(DateTime operationDate, decimal amount, string currency, CancellationToken cancellationToken);
}

