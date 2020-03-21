using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;
using static TakeMyTime.Tests.Common.CreateUpdateParams;

namespace TakeMyTime.Models.Tests
{
    [TestClass]
    public class EntryTests
    {
        EntryCreateParam createParam = new EntryCreateParam
        {
            Comment = "Test comment",
            Date = DateTime.Now.AddMinutes(-20),
            Ended = DateTime.Now.AddMinutes(-9),
            Started = DateTime.Now.AddMinutes(-20),
            Name = "Test entry",
            Project = new Project()
        };

        [TestMethod]
        public void Create_Test()
        {
            // ARRANGE
            // ...

            // ACT
            var entry = Entry.Create(createParam);

            // ASSERT
            Assert.IsNotNull(entry);
        }

        [TestMethod]
        public void IsDurationCorrect()
        {
            // ARRANGE & ACT
            var entry = Entry.Create(createParam);
            entry.CalculateDuration();

            // ASSERT
            Assert.AreEqual(new TimeSpan(0, 10, 0).Minutes, new TimeSpan(entry.DurationAsTicks.Value).Minutes);
        }

        [TestMethod]
        public void Update_Test()
        {
            // ARRANGE
            var entry = Entry.Create(createParam);
            string changedName = "Test changed";
            string changedComment = "Comment changed";
            var updateParam = new EntryUpdateParam
            {
                Comment = changedComment,
                Name = changedName
            };

            // ACT
            entry.Update(updateParam);

            // ASSERT
            Assert.AreEqual(changedName, entry.Name);
            Assert.AreEqual(changedComment, entry.Comment);
        }
    }
}
