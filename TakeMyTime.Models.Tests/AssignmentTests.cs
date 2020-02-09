using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TakeMyTime.Common.Exceptions;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;
using static Common.Enums.EnumDefinition;
using static TakeMyTime.Models.Tests.CreateUpdateParams;

namespace TakeMyTime.Models.Tests
{
    [TestClass]
    public class AssignmentTests
    {
        AssignmentCreateParam createParam =
            new AssignmentCreateParam
            {
                DateDue = DateTime.Now.AddDays(4),
                DatePlanned = DateTime.Now.AddDays(3),
                Description = "Testdescription",
                DurationPlannedAsTicks = new TimeSpan(1, 0, 0).Ticks,
                Name = "Testassignment",
                Project = new Project()
            };

        [TestMethod]
        public void Create_Test()
        {
            // ARRANGE
            // ...

            // ACT
            var assignment = Assignment.Create(createParam);

            // ASSERT
            Assert.AreEqual(AssignmentStatus.InProgress, assignment.AssignmentStatus);
        }

        [TestMethod]
        [ExpectedException(typeof(CannotChangeStatusException))]
        public void UpdateStatus_TestAssertFail()
        {
            // ARRANGE
            var assignment = Assignment.Create(createParam);
            assignment.AssignmentStatus = AssignmentStatus.Done;

            // ACT & ASSERT
            assignment.UpdateStatus(AssignmentStatus.Future);
        }

        [TestMethod]
        public void UpdateStatus_TestAssertSuccess()
        {
            // ARRANGE
            var assignment = Assignment.Create(createParam);

            // ACT
            assignment.UpdateStatus(AssignmentStatus.Done);

            // ASSERT
            Assert.AreEqual(AssignmentStatus.Done, assignment.AssignmentStatus);
        }

        [TestMethod]
        public void CanDelete_TestAssertTrue()
        {
            // ARRANGE
            var assignment = Assignment.Create(createParam);

            // ACT
            var canDelete = assignment.CanDelete();

            // ASSERT
            Assert.IsTrue(canDelete);
        }

        [TestMethod]
        public void CanDelete_TestAssertFalse()
        {
            // ARRANGE
            var assignment = Assignment.Create(createParam);
            assignment.Subtasks.Add(Subtask.Create(new SubtaskCreateParam { Name = "Test subtask", Description = "123", Priority = SubtaskPriority.High }));

            // ACT
            var canDelete = assignment.CanDelete();

            // ASSERT
            Assert.IsFalse(canDelete);
        }
    }
}
