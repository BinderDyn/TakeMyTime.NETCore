using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;
using static TakeMyTime.Tests.Common.CreateUpdateParams;

namespace TakeMyTime.Models.Tests
{
    [TestClass]
    public class ProjectTypeTests
    {
        ProjectTypeCreateParam createParam = new ProjectTypeCreateParam
        {
            Description = "Test description",
            Name = "Test name"
        };

        [TestMethod]
        public void Create_Test()
        {
            // ARRANGE
            // ...

            // ACT
            var projectType = ProjectType.Create(createParam);

            // ASSERT
            Assert.IsNotNull(projectType);
        }

        [TestMethod]
        public void Update_Test()
        {
            // ARRANGE
            var projectType = ProjectType.Create(createParam);
            string changedName = "Test name changed";
            string changedDescription = "Test description changed";

            // ACT
            projectType.Update(changedName, changedDescription);

            // ASSERT
            Assert.AreEqual(changedName, projectType.Name);
            Assert.AreEqual(changedDescription, projectType.Description);
        }
    }
}
