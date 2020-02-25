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
    public class SubtaskTests
    {
        SubtaskCreateParam createParam = new SubtaskCreateParam
        {
            Description = "Test description",
            Name = "Test name",
            Priority =  EnumDefinition.SubtaskPriority.High
        };

        [TestMethod]
        public void Create_Test()
        {
            // ARRANGE
            // ...

            // ACT
            var subtask = Subtask.Create(createParam);

            // ASSERT
            Assert.IsNotNull(subtask);
        }

        [TestMethod]
        public void Update_Test()
        {
            // ARRANGE
            var subtask = Subtask.Create(createParam);
            string changedName = "Test name changed";
            string changedDescription = "Test description changed";
            EnumDefinition.SubtaskPriority changedPriority = EnumDefinition.SubtaskPriority.Lowest;

            // ACT
            subtask.Update(new SubtaskUpdateParam { Name = changedName, Description = changedDescription, Priority = changedPriority });

            // ASSERT
            Assert.AreEqual(changedName, subtask.Name);
            Assert.AreEqual(changedDescription, subtask.Description);
            Assert.AreEqual(changedPriority, subtask.Priority);
        }

        [TestMethod]
        public void SetStatus_TestSuccess()
        {
            // ARRANGE
            var subtask = Subtask.Create(createParam);

            // ACT
            subtask.SetStatus(EnumDefinition.SubtaskStatus.Done);

            // ASSERT
            Assert.AreEqual(EnumDefinition.SubtaskStatus.Done, subtask.Status);
        }

        [TestMethod]
        public void SetStatus_TestFail()
        {
            // ARRANGE
            var subtask = Subtask.Create(createParam);
            subtask.Status = EnumDefinition.SubtaskStatus.Aborted;

            // ACT
            subtask.SetStatus(EnumDefinition.SubtaskStatus.Done);

            // ASSERT
            subtask.Status = EnumDefinition.SubtaskStatus.Aborted;
        }
    }
}
