using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class AssignmentLogic
    {
        private readonly UnitOfWork unitOfWork;

        public AssignmentLogic(UnitOfWork uow = null)
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

        #region CRUD

        public Assignment GetAssignmentById(int id)
        {
            return unitOfWork?.Assignments?.GetAssignmentById(id);
        }

        public IEnumerable<Assignment> GetAllAssignments()
        {
            return unitOfWork.Assignments.GetAllAssignmentsLoadFull();
        }

        public IEnumerable<Assignment> GetAssignmentsByProjectId(int project_id)
        {
            return unitOfWork.Assignments
                .GetAssignmentsByProjectId(project_id).ToList();
        }

        public Assignment AddAssignment(Assignment.ICreateParam param)
        {
            var assignment = Assignment.Create(param);
            unitOfWork.Assignments.Add(assignment);

            unitOfWork.Complete();
            return assignment;
        }

        public void UpdateAssignment(int id, Assignment.IUpdateParam param)
        {
            var edit = unitOfWork.Assignments.Get(id);
            edit.Update(param);

            unitOfWork.Complete();
        }

        public void DeleteAssignment(int id)
        {
            var toBeDeleted = GetAssignmentById(id);
            if (!toBeDeleted.CanDelete())
            {
                var subtasks = toBeDeleted.ClearSubtasks();
                foreach (var subtask in subtasks)
                {
                    var entries = subtask.ClearEntries();
                    unitOfWork.Entries.RemoveRange(entries);
                    unitOfWork.Complete();
                }

                unitOfWork.Subtasks.RemoveRange(subtasks);
                unitOfWork.Complete();
            }
            unitOfWork.Assignments.Remove(toBeDeleted);
            unitOfWork.Complete();
        }

        public void DeleteAssignments(IEnumerable<Assignment> assignments)
        {
            IList<Assignment> toBeDeletedAssignments = new List<Assignment>();
            foreach (var a in assignments)
            {
                var entity = unitOfWork.Assignments.Get(a.Id);
                if (entity != null && entity.CanDelete()) toBeDeletedAssignments.Add(entity);
                else
                {
                    var subtasksToDelete = entity.ClearSubtasks();
                    unitOfWork.Subtasks.RemoveRange(subtasksToDelete);
                }

            }
            unitOfWork.Assignments.RemoveRange(toBeDeletedAssignments);
            unitOfWork.Complete();
        }

        public void DeleteSubtask(int assignment_id, int subtask_id)
        {
            unitOfWork.Assignments.DeleteSubtask(assignment_id, subtask_id);
            unitOfWork.Complete();
        }

        public void AddSubtask(int id, Subtask.ICreateParam param)
        {
            var subtask = Subtask.Create(param);
            var assignment = unitOfWork.Assignments.GetAssignmentById(id);
            assignment.Subtasks.Add(subtask);
            unitOfWork.Complete();
        }

        public void SetDone(int id)
        {
            var assignment = unitOfWork.Assignments.GetAssignmentById(id);
            assignment.UpdateStatus(EnumDefinition.AssignmentStatus.Done);
            unitOfWork.Complete();
        }

        public void SetAborted(int id)
        {
            var assignment = unitOfWork.Assignments.GetAssignmentById(id);
            assignment.UpdateStatus(EnumDefinition.AssignmentStatus.Aborted);
            unitOfWork.Complete();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        #endregion
    }
}
