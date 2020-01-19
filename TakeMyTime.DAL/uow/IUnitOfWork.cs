using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.Interfaces;

namespace TakeMyTime.DAL.uow
{
    public interface IUnitOfWork : IDisposable
    {
        //Add Repositories here
        IProjectRepository Projects { get; }
        IEntryRepository Entries { get; }
        IAssignmentRepository Assignments { get; }
        IProjectTypeRepository ProjectTypes { get; }
        ISubtaskRepository Subtasks { get; }

        int Complete();
    }
}
