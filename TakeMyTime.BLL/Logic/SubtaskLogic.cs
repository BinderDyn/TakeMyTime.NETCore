using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DAL.uow;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class SubtaskLogic
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public SubtaskLogic()
        {
        }

        public Subtask Get(int id)
        {
            return unitOfWork.Subtasks.Get(id);
        }

        public void Update(int id, Subtask.IUpdateParam param)
        {
            var edit = unitOfWork.Subtasks.Get(id);
            edit.Update(param);
            unitOfWork.Complete();
        }

        public void Delete(Subtask subtask)
        {
            unitOfWork.Subtasks.Remove(subtask);
            unitOfWork.Complete();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
