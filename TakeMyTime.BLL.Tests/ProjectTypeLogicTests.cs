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
    public class ProjectTypeLogicTests
    {
        UnitOfWork uow;

        [TestInitialize]
        public void GetTestUnitOfWork()
        {
            this.uow = new UnitOfWork();
            this.uow.CreateDatabase();
        }

        #region SetUpTestEnvironment

        private void CreateProjectTypes()
        {
            var createParams = new ProjectTypeCreateParam
            {
                Description = "Test description",
                Name = "Test project type"
            };

            for (int i = 0; i < 3; i++)
            {
                this.uow.ProjectTypes.Add(ProjectType.Create(createParams));
            }
            this.uow.Complete();
        }

        #endregion

        [TestMethod]
        public void GetProjectTypes_Test()
        {
            // ARRANGE
            CreateProjectTypes();
            var projectTypeLogic = new ProjectTypeLogic();

            // ACT
            var projectTypes = projectTypeLogic.GetProjectTypes();

            // ASSERT
            Assert.IsTrue(projectTypes.Count() == 3);
            projectTypeLogic.Dispose();
        }

        [TestMethod]
        public void GetProjectType_Test()
        {
            // ARRANGE
            CreateProjectTypes();
            var projectTypeLogic = new ProjectTypeLogic();

            // ACT
            var projectType = projectTypeLogic.GetProjectType(1);

            // ASSERT
            Assert.IsNotNull(projectType);
        }

        [TestMethod]
        public void AddProjectType()
        {
            // ARRANGE
            var projectTypeLogic = new ProjectTypeLogic();
            var createParam = new ProjectTypeCreateParam
            {
                Description = "Test description",
                Name = "Test name"
            };

            // ACT
            projectTypeLogic.AddProjectType(createParam);
            projectTypeLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var createdProjectType = this.uow.ProjectTypes.Load(pt => pt.Name == "Test name").First();
            Assert.IsNotNull(createdProjectType);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
