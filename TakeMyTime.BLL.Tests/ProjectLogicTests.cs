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
    public class ProjectLogicTests
    {
        UnitOfWork uow;

        [TestInitialize]
        public void InitializeTestingEnvironment()
        {
            this.uow = new UnitOfWork();
            this.uow.CreateDatabase();
        }

        #region SetUpTestingEnvironment

        private void CreateProjectType()
        {
            var createParam = new ProjectTypeCreateParam
            {
                Description = "Test description",
                Name = "Test name"
            };

            this.uow.ProjectTypes.Add(ProjectType.Create(createParam));
            this.uow.Complete();
        }

        private void CreateProject()
        {
            CreateProjectType();
            var projectType = this.uow.ProjectTypes.GetAll().First();
            var createParam = new ProjectCreateParam
            {
                Name = "Test project",
                Description = "Test description",
                ProjectType = projectType
            };

            this.uow.Projects.Add(Project.Create(createParam));
            this.uow.Complete();
        }

        private void CreateProjects()
        {
            CreateProjectType();
            var projectType = this.uow.ProjectTypes.GetAll().First();
            var createParam = new ProjectCreateParam
            {
                Name = "Test project",
                Description = "Test description",
                ProjectType = projectType
            };

            for (int i = 0; i < 3; i++)
            {
                this.uow.Projects.Add(Project.Create(createParam));
            }
            this.uow.Complete();
        }


        #endregion

        [TestMethod]
        public void GetProjectById_Test()
        {
            // ARRANGE
            CreateProject();
            var project = this.uow.Projects.GetAll().First();
            var projectLogic = new ProjectLogic();

            // ACT
            var foundProject = projectLogic.GetProjectById(project.Id);
            projectLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(foundProject);
            Assert.AreEqual("Test project", foundProject.Name);
        }

        [TestMethod]
        public void GetAllProjects_Test()
        {
            // ARRANGE
            CreateProjects();
            var projectLogic = new ProjectLogic();

            // ACT
            var foundProjects = projectLogic.GetAllProjects();
            projectLogic.Dispose();

            // ASSERT
            Assert.IsNotNull(foundProjects);
            Assert.AreEqual(3, foundProjects.Count());
        }

        [TestMethod]
        public void InsertProject_Test()
        {
            // ARRANGE
            CreateProjectType();
            var projectType = this.uow.ProjectTypes.GetAll().First();
            var projectCreateParam = new ProjectCreateParam
            {
                Description = "Test description",
                Name = "Test project",
                ProjectType = projectType
            };
            var projectLogic = new ProjectLogic();

            // ACT
            projectLogic.InsertProject(projectCreateParam);
            projectLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var createdProject = this.uow.Projects.GetAll().First();
            Assert.IsNotNull(createdProject);
            Assert.AreEqual(EnumDefinition.ProjectStatus.Active, createdProject.ProjectStatus);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
