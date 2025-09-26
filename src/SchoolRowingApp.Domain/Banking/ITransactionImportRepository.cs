using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRowingApp.Domain.Banking
{
    public interface ITransactionImportRepository
    {
        Task AddAsync(TransactionImport import, CancellationToken cancellationToken);
        Task UpdateAsync(TransactionImport import, CancellationToken cancellationToken);
        Task<TransactionImport?> GetByFileHashAsync(string fileHash, CancellationToken cancellationToken);
        Task AddImportDetailAsync(TransactionImportDetail detail, CancellationToken cancellationToken);
    }
}
