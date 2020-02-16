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
    public class EntryLogicTests
    {
        UnitOfWork uow;

        [TestInitialize]
        public void InitializeTestingEnvironment()
        {
            this.uow = new UnitOfWork();
            this.uow.CreateDatabase();
        }

        #region SetUpTestingEnvironment

        private void CreateEntries()
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
            var assignment = this.uow.Assignments.GetAll().First();
            var subtask = Subtask.Create(new SubtaskCreateParam
            {
                Name = "Test subtask",
                Description = "Test description",
                Priority = EnumDefinition.SubtaskPriority.Medium
            });

            subtask.Assignment_Id = assignment.Id;
            subtask.Assignment = assignment;
            this.uow.Subtasks.Add(subtask);
            this.uow.Complete();

            var entryCreateParam = new EntryCreateParam
            {
                Comment = "Test comment",
                Date = DateTime.Now.Date,
                Started = DateTime.Now.AddHours(-2),
                Ended = DateTime.Now.AddHours(-1),
                Name = "Test entry",
                Project = project
            };

            for (int i = 0; i < 3; i++)
            {
                this.uow.Entries.Add(Entry.Create(entryCreateParam));
            }

            this.uow.Complete();
        }

        #endregion

        [TestMethod]
        public void GetAllEntries_Test()
        {
            // ARRANGE
            CreateEntries();
            var entryLogic = new EntryLogic();

            // ACT
            var entries = entryLogic.GetAllEntries().ToList();
            entryLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(entries);
            Assert.IsTrue(entries.Count() == 3);
        }

        [TestMethod]
        public void GetEntryById_Test()
        {
            // ARRANGE
            CreateEntries();
            var entryLogic = new EntryLogic();

            // ACT
            var entry = entryLogic.GetEntryById(1);
            entryLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(entry);
        }

        [TestMethod]
        public void UpdateEntry_Test()
        {
            // ARRANGE
            CreateEntries();
            var entryLogic = new EntryLogic();
            var updateParam = new EntryUpdateParam
            {
                Comment = "Test comment updated",
                Name = "Test name updated"
            };

            // ACT
            entryLogic.UpdateEntry(1, updateParam);
            entryLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updatedEntry = this.uow.Entries.Get(1);
            Assert.IsNotNull(updatedEntry);
            Assert.AreEqual("Test comment updated", updatedEntry.Comment);
            Assert.AreEqual("Test name updated", updatedEntry.Name);
        }

        [TestMethod]
        public void DeleteEntry_Test()
        {
            // ARRANGE
            CreateEntries();
            var entryLogic = new EntryLogic();

            // ACT
            entryLogic.DeleteEntry(1);
            entryLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var deletedEntry = this.uow.Entries.Get(1);
            Assert.IsNull(deletedEntry);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
