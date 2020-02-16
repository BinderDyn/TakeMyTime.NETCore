using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DAL.uow;

using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class SubtaskLogic
    {
        private readonly UnitOfWork unitOfWork;

        public SubtaskLogic(UnitOfWork uow = null)
        {
            if (uow != null)
            {
                this.unitOfWork = uow;
            }
            else
            {
                this.unitOfWork = new UnitOfWork();
            }
        }

        public Subtask GetById(int id)
        {
            return unitOfWork.Subtasks.Get(id);
        }

        public IEnumerable<Subtask> GetByAssignmentId(int assignment_id)
        {
            return unitOfWork.Subtasks.Load(s => s.Assignment_Id == assignment_id).ToArray();
        }

        public void Update(int id, Subtask.IUpdateParam param)
        {
            var edit = unitOfWork.Subtasks.Get(id);
            edit.Update(param);
            unitOfWork.Complete();
        }

        public void AddEntry(int subtask_id, Entry.ICreateParam param)
        {
            var entry = Entry.Create(param);
            var subtask = unitOfWork.Subtasks.GetSubtaskFullyLoaded(subtask_id);
            subtask.Entries.Add(entry);
            unitOfWork.Complete();
        }

        public void Delete(int subtask_id)
        {
            var subtask = unitOfWork.Subtasks.Get(subtask_id);
            unitOfWork.Subtasks.Remove(subtask);
            unitOfWork.Complete();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
