using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.BLL.Logic
{
    public class AssignmentLogic
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public AssignmentLogic()
        {

        }

        #region CRUD

        public Assignment GetAssignmentById(int id)
        {
            return unitOfWork?.Assignments?.Get(id);
        }

        public IEnumerable<Assignment> GetAllAssignments()
        {
            return unitOfWork.Assignments.GetAll();
        }

        public IEnumerable<Assignment> GetAssignmentsByProjectId(int projectId)
        {
            return unitOfWork.Assignments.Find(x => x.Project_Id == projectId).ToList();
        }

        public void AddAssignment(Assignment.ICreateParam param)
        {
                var assignment = Assignment.Create(param);
                unitOfWork.Assignments.Add(assignment);
                unitOfWork.Complete();
        }

        public void UpdateAssignment(int id, Assignment.IUpdateParam param)
        {
            var edit = unitOfWork.Assignments.Get(id);
            edit.Update(param);

            unitOfWork.Complete();
        }

        public void UpdateAssignments(IEnumerable<Assignment> assignments)
        {
            var edits = unitOfWork.Assignments.GetAll();

            foreach(var edit in edits)
            {
                foreach(var assignment in assignments)
                {
                    if(edit.Id == assignment.Id)
                    {
                        edit.Name = assignment.Name;
                        edit.Description = assignment.Description;
                        edit.DatePlanned = assignment.DatePlanned;
                        //edit.DurationPlanned = assignment.DurationPlanned;
                        edit.DurationPlannedAsTicks = assignment.DurationPlannedAsTicks;
                        edit.AssignmentStatus = assignment.AssignmentStatus;
                        edit.Edited = DateTime.Now;
                    }
                }
            }

            unitOfWork.Complete();
            Dispose();
        }

        public void DeleteAssignment(int id)
        {
            var toBeDeleted = GetAssignmentById(id);
            unitOfWork.Assignments.Remove(toBeDeleted);
            unitOfWork.Complete();
            Dispose();
        }

        public void DeleteAssignments(IEnumerable<Assignment> assignments)
        {
            IList<Assignment> entities = new List<Assignment>();
            foreach (var a in assignments)
            {
                var entity = unitOfWork.Assignments.Get(a.Id);
                if (entity != null) entities.Add(entity);
            }

            unitOfWork.Assignments.RemoveRange(entities);

            unitOfWork.Complete();
        }

        public void Dispose()
        {

            unitOfWork.Dispose();
        }


        #endregion

        public bool? CheckForTimePlan(int id)
        {
            return unitOfWork.Assignments.CheckForTimePlan(id);
        }

        public TimeSpan? GetActualDuration(int id)
        {
            return unitOfWork.Assignments.GetActualDuration(id);
        }

        private bool CanBePushed(int assignmentId)
        {
            var assignment = unitOfWork.Assignments.Get(assignmentId);
            return assignment.TimesPushed < 3 ? true : false;
        }

        //public bool PushOneWeekForward(int assignmentId)
        //{
        //    const bool wasPushed = true;
        //    if(CanBePushed(assignmentId))
        //    {
        //        var assignment = unitOfWork.Assignments.Get(assignmentId);
        //        assignment.DateDue = assignment.DateDue + TimeSpan.FromDays(7);
        //        assignment.TimesPushed++;
        //        UpdateAssignment(assignment);
        //        return wasPushed;
        //    }
        //    return !wasPushed;
        //}

        public string GetDueAssignmentsForToday()
        {
            string assignmentNotice = string.Empty;

            IEnumerable<Assignment> assignments = GetAllAssignments()
                .Where(a => a.DateDue.Value.Date == DateTime.Now.Date && 
                a.AssignmentStatus != EnumDefinition.AssignmentStatus.Done &&
                a.AssignmentStatus != EnumDefinition.AssignmentStatus.Aborted)
                .OrderBy(a => a.Project.Name)
                .ToList();

            assignmentNotice = "Assignments due for today:";
            var assignmentsWithDateDue = assignments.Where(a => a.DateDue.Value.Date == DateTime.Now.Date);
            foreach (var assignment in assignmentsWithDateDue)
            {
                assignmentNotice += System.Environment.NewLine + "Project: " + assignment.Project.Name + " - " + assignment.Name + System.Environment.NewLine;
            }

            if (assignmentNotice == "Assignments due for today:") assignmentNotice += " None.";

            return assignmentNotice;
        }
    }
}
