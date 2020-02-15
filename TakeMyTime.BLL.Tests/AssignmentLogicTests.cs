using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.BLL.Logic;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;
using static TakeMyTime.Tests.Common.CreateUpdateParams;

namespace TakeMyTime.BLL.Tests
{
    [TestClass]
    public class AssignmentLogicTests
    {
        UnitOfWork uow;

        public void GetTestUnitOfWork()
        {
            this.uow = new UnitOfWork();
        }

        private void CreateTestEnvironment()
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

        [TestMethod]
        public void AddAssignment_Test()
        {
            // ARRANGE
            CreateTestEnvironment();
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

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.Dispose();
        }
    }
}
