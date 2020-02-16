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
    public class AssignmentLogicTests
    {
        UnitOfWork uow;

        [TestInitialize]
        public void GetTestUnitOfWork()
        {
            this.uow = new UnitOfWork();
            this.uow.CreateDatabase();
        }

        #region SettingUpTestingEnvironment

        private void CreateTestProjectAndProjectType()
        {
            GetTestUnitOfWork();
            var projectTypeCreateParam = new ProjectTypeCreateParam
            {
                Name = "Test",
                Description = "Test project type"
            };

            var projectCreateParam = new ProjectCreateParam
            {
                Description = "Project description",
                Name = "New project",
                ProjectType = ProjectType.Create(projectTypeCreateParam)
            };

            uow.ProjectTypes.Add(ProjectType.Create(projectTypeCreateParam));
            uow.Projects.Add(Project.Create(projectCreateParam));
            uow.Complete();
        }

        private Assignment CreateTestAssignment()
        {
            var projectTypeCreateParam = new ProjectTypeCreateParam
            {
                Name = "Test",
                Description = "Test project type"
            };

            var projectCreateParam = new ProjectCreateParam
            {
                Description = "Project description",
                Name = "New project",
                ProjectType = ProjectType.Create(projectTypeCreateParam)
            };

            uow.ProjectTypes.Add(ProjectType.Create(projectTypeCreateParam));
            uow.Projects.Add(Project.Create(projectCreateParam));
            uow.Complete();

            var project = uow.Projects.GetAll().First(p => p.Name == "New project");
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
            uow.Complete();

            var assignment = uow.Assignments.Load(a => a.Name == "TestAssignment").Single();
            return assignment;
        }

        #endregion


        [TestMethod]
        public void AddAssignment_Test()
        {
            // ARRANGE
            CreateTestProjectAndProjectType();
            var project = uow.Projects.GetAll().First();

            var createParam = new AssignmentCreateParam
            {
                Name = "TestAssignment",
                DateDue = DateTime.Now.AddDays(1),
                DatePlanned = DateTime.Now,
                Description = "New description",
                DurationPlannedAsTicks = new TimeSpan(1, 0, 0).Ticks,
                Project = project
            };

            var assignmentLogic = new AssignmentLogic();

            // ACT
            assignmentLogic.AddAssignment(createParam);
            assignmentLogic.Dispose();

            // ASSERT
            var assignments = uow.Assignments.GetAll().ToList();
            Assert.IsTrue(assignments.Count > 0);
            Assert.AreEqual("TestAssignment", assignments.First(a => a.Name == "TestAssignment").Name);
        }

        [TestMethod]
        public void GetAssignmentById_Test()
        {
            // ARRANGE
            var createdAssignment = CreateTestAssignment();
            int assignmentId = createdAssignment.Id;
            var assignmentLogic = new AssignmentLogic();

            // ACT
            var foundAssignment = assignmentLogic.GetAssignmentById(assignmentId);
            assignmentLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(foundAssignment);
            Assert.AreEqual(createdAssignment.Name, foundAssignment.Name);
        }

        [TestMethod]
        public void GetAssignmentsByProjectId_Test()
        {
            // ARRANGE
            var assignment = CreateTestAssignment();
            int projectId = assignment.Project_Id.Value;
            var assignmentLogic = new AssignmentLogic();

            // ACT
            var foundAssignments = assignmentLogic.GetAssignmentsByProjectId(projectId);
            assignmentLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(foundAssignments);
            Assert.IsTrue(foundAssignments.All(a => a.Project_Id.Value == projectId));
        }

        [TestMethod]
        public void UpdateAssignment_Test()
        {
            // ARRANGE
            var assignment = CreateTestAssignment();
            var updateParam = new AssignmentUpdateParam
            {
                Name = "TestAssignment Updated",
                DateDue = DateTime.Now.AddDays(1),
                DatePlanned = DateTime.Now,
                Description = "New description Updated",
                DurationPlannedAsTicks = new TimeSpan(1, 0, 0).Ticks,
            };
            var assignmentLogic = new AssignmentLogic();

            // ACT
            assignmentLogic.UpdateAssignment(assignment.Id, updateParam);
            assignmentLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updatedAssignment = uow.Assignments.Get(assignment.Id);
            Assert.AreNotEqual(assignment.Name, updatedAssignment.Name);
            Assert.AreEqual("TestAssignment Updated", updatedAssignment.Name);
        }

        [TestMethod]
        public void DeleteAssignment_Test()
        {
            // ARRANGE
            var assignment = CreateTestAssignment();
            var assignmentLogic = new AssignmentLogic();

            // ACT
            assignmentLogic.DeleteAssignment(assignment.Id);
            assignmentLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT 
            var foundAssignment = uow.Assignments.Get(assignment.Id);
            Assert.IsNull(foundAssignment);
        }

        [TestMethod]
        public void AddSubtask_Test()
        {
            // ARRANGE
            var assignment = CreateTestAssignment();
            var assignmentLogic = new AssignmentLogic();
            var subtaskCreateParam = new SubtaskCreateParam
            {
                Description = "Test description",
                Name = "Test subtask",
                Priority = EnumDefinition.SubtaskPriority.High
            };

            // ACT
            assignmentLogic.AddSubtask(assignment.Id, subtaskCreateParam);
            assignmentLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var subtask = this.uow.Subtasks.Load(s => s.Assignment_Id == assignment.Id).First();
            Assert.IsNotNull(subtask);
        }

        [TestMethod]
        public void SetDone_Test()
        {
            // ARRANGE
            var assignment = CreateTestAssignment();
            var assignmentLogic = new AssignmentLogic();

            // ACT
            assignmentLogic.SetDone(assignment.Id);
            assignmentLogic.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updatedAssignment = uow.Assignments.Get(assignment.Id);
            Assert.AreEqual(EnumDefinition.AssignmentStatus.Done, updatedAssignment.AssignmentStatus);
        }

        [TestMethod]
        public void SetAborted_Test()
        {
            // ARRANGE
            var assignment = CreateTestAssignment();
            var assignmentLogic = new AssignmentLogic();

            // ACT
            assignmentLogic.SetAborted(assignment.Id);
            assignmentLogic.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updatedAssignment = uow.Assignments.Get(assignment.Id);
            Assert.AreEqual(EnumDefinition.AssignmentStatus.Aborted, updatedAssignment.AssignmentStatus);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
