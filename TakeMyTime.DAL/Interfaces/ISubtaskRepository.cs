using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface ISubtaskRepository : IRepository<Subtask>
    {
        void SetStatus(int id, EnumDefinition.SubtaskStatus status);
    }
}
