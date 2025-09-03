using SchoolRowingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolRowingApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    //DbSet<TodoList> TodoLists { get; }

    //DbSet<TodoItem> TodoItems { get; }
	DbSet<Athlete> Athletes { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
