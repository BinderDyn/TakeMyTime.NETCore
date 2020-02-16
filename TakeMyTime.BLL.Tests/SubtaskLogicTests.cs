using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.BLL.Logic;
using TakeMyTime.DAL.uow;
using TakeMyTime.Models.Models;
using static TakeMyTime.Tests.Common.CreateUpdateParams;

namespace TakeMyTime.BLL.Tests
{
    [TestClass]
    public class SubtaskLogicTests
    {
        UnitOfWork uow;

        [TestInitialize]
        public void InitializeTestingEnvironment()
        {
            this.uow = new UnitOfWork();
            this.uow.CreateDatabase();
        }

        #region SetUpTestingEnvironment

        private void CreatePrerequisitesForSubtaskCreation()
        {
            var projectTypeCreateParam = new ProjectTypeCreateParam
            {
                Name = "Test",
                Description = "Test project type"
            };

            uow.ProjectTypes.Add(ProjectType.Create(projectTypeCreateParam));
            this.uow.Complete();

            var projectType = this.uow.ProjectTypes.GetAll().First();
            var projectCreateParam = new ProjectCreateParam
            {
                Description = "Project description",
                Name = "New project",
                ProjectType = projectType
            };
            uow.Projects.Add(Project.Create(projectCreateParam));
            this.uow.Complete();

            var project = this.uow.Projects.GetAll().First();
            var assignmentCreateParam = new AssignmentCreateParam
            {
                Name = "TestAssignment",
                DateDue = DateTime.Now.AddDays(1),
                DatePlanned = DateTime.Now,
                Description = "New description",
                DurationPlannedAsTicks = new TimeSpan(1, 0, 0).Ticks,
                Project = project
            };

            uow.Assignments.Add(Assignment.Create(assignmentCreateParam));
            this.uow.Complete();
        }

        private void CreateSubtask()
        {
            CreatePrerequisitesForSubtaskCreation();
            var assignment = this.uow.Assignments.GetAll().First();
            var subtaskCreateParam = new SubtaskCreateParam
            {
                Name = "Subtask 1",
                Description = "Subtask description",
                Priority = EnumDefinition.SubtaskPriority.Medium
            };
            var subtask = Subtask.Create(subtaskCreateParam);
            subtask.Assignment_Id = assignment.Id;
            subtask.Assignment = assignment;

            this.uow.Subtasks.Add(subtask);
            this.uow.Complete();
        }

        #endregion

        [TestMethod]
        public void GetById_Test()
        {
            // ARRANGE
            CreateSubtask();
            var subtaskLogic = new SubtaskLogic();

            // ACT
            var subtask = subtaskLogic.GetById(1);
            subtaskLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(subtask);
        }

        [TestMethod]
        public void GetByAssignmentId_Test()
        {
            // ARRANGE
            CreateSubtask();
            var subtaskLogic = new SubtaskLogic();

            // ACT
            var subtask = subtaskLogic.GetByAssignmentId(1);
            subtaskLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(subtask);
        }

        [TestMethod]
        public void Update_Test()
        {
            // ARRANGE
            CreateSubtask();
            var subtaskUpdateParam = new SubtaskUpdateParam
            {
                Description = "Subtask description updated",
                Name = "Subtask name updated",
                Priority = EnumDefinition.SubtaskPriority.Highest
            };
            var subtaskLogic = new SubtaskLogic();

            // ACT
            subtaskLogic.Update(1, subtaskUpdateParam);
            subtaskLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updated = this.uow.Subtasks.Get(1);
            Assert.IsNotNull(updated);
            Assert.AreEqual(subtaskUpdateParam.Name, updated.Name);
            Assert.AreEqual(subtaskUpdateParam.Description, updated.Description);
            Assert.AreEqual(subtaskUpdateParam.Priority, updated.Priority);
        }

        [TestMethod]
        public void AddEntry_Test()
        {
            // ARRANGE
            CreateSubtask();
            var subtask = this.uow.Subtasks.Get(1);
            var project = this.uow.Projects.Get(1);
            var entryCreateParam = new EntryCreateParam
            {
                Name = "Test entry",
                Comment = "Test comment",
                Date = DateTime.Now.Date,
                Started = DateTime.Now.AddHours(-2),
                Ended = DateTime.Now.AddHours(-1),
                Subtask = subtask,
                Project = project
            };
            var subtaskLogic = new SubtaskLogic();

            // ACT
            subtaskLogic.AddEntry(subtask.Id, entryCreateParam);
            subtaskLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var inserted = this.uow.Entries.Get(1);
            Assert.IsNotNull(inserted);
            Assert.AreEqual(subtask.Id, inserted.Subtask_Id.Value);
        }

        [TestMethod]
        public void Delete_Test()
        {
            // ARRANGE
            CreateSubtask();
            var subtaskLogic = new SubtaskLogic();

            // ACT
            subtaskLogic.Delete(1);
            subtaskLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var deleted = this.uow.Subtasks.Get(1);
            Assert.IsNull(deleted);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
