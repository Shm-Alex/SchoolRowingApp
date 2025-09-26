using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Banking;
using SchoolRowingApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRowingApp.Infrastructure.Repositories
{
    public class TransactionImportRepository : ITransactionImportRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionImportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TransactionImport import, CancellationToken cancellationToken)
        {
            await _context.TransactionImports.AddAsync(import, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TransactionImport import, CancellationToken cancellationToken)
        {
            _context.TransactionImports.Update(import);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<TransactionImport?> GetByFileHashAsync(string fileHash, CancellationToken cancellationToken)
        {
            return await _context.TransactionImports.FirstOrDefaultAsync(ti => ti.FileHash == fileHash, cancellationToken);
        }

        public async Task AddImportDetailAsync(TransactionImportDetail detail, CancellationToken cancellationToken)
        {
            await _context.TransactionImportDetails.AddAsync(detail, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
