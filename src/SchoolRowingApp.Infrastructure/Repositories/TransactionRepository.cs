// Infrastructure/Repositories/TransactionRepository.cs
using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Banking;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(DateTime operationDate, decimal amount, string currency, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AnyAsync(t => t.OperationDate == operationDate &&
                          t.Amount == amount &&
                          t.Currency == currency,
                          cancellationToken);
    }
}