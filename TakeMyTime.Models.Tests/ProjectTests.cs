using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;
using static TakeMyTime.Tests.Common.CreateUpdateParams;

namespace TakeMyTime.Models.Tests
{
    [TestClass]
    public class ProjectTests
    {
        ProjectCreateParam createParam = new ProjectCreateParam
        {
            Description = "Test description",
            Name = "Test name",
            ProjectType = new Models.ProjectType()
        };

        [TestMethod]
        public void Create_Test()
        {
            // ARRANGE
            // ...

            // ACT
            var project = Project.Create(createParam);

            // ASSERT
            Assert.IsNotNull(project);
        }

        [TestMethod]
        public void CheckCanDelete_TestSuccess()
        {
            // ARRANGE
            var project = Project.Create(createParam);
            project.Assignments.Add(new Assignment() { AssignmentStatus = EnumDefinition.AssignmentStatus.Done });
            project.Assignments.Add(new Assignment() { AssignmentStatus = EnumDefinition.AssignmentStatus.Aborted });
            project.ProjectStatus = EnumDefinition.ProjectStatus.Archived;

            // ACT
            bool canDelete = project.CanDelete;

            // ASSERT
            Assert.IsTrue(canDelete);
        }

        [TestMethod]
        public void CheckCanDelete_TestFail()
        {
            // ARRANGE
            var project = Project.Create(createParam);
            project.Assignments.Add(new Assignment() { AssignmentStatus = EnumDefinition.AssignmentStatus.InProgress });
            project.Assignments.Add(new Assignment() { AssignmentStatus = EnumDefinition.AssignmentStatus.Aborted });
            project.ProjectStatus = EnumDefinition.ProjectStatus.Active;

            // ACT
            bool canDelete = project.CanDelete;

            // ASSERT
            Assert.IsFalse(canDelete);
        }
    }
}
