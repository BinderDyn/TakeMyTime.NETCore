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

        private void CreateProjectWithEntries()
        {
            CreateProject();
            var project = this.uow.Projects.GetAll().First();
            var entries = new List<Entry>()
            {
                Entry.Create(new EntryCreateParam
                {
                    Name = "Test",
                    Started = DateTime.Now.AddHours(-1),
                    Ended = DateTime.Now.AddHours(0),
                    Project = project,
                    Comment = "Test comment",
                    Date = DateTime.Now.Date
                }),
                Entry.Create(new EntryCreateParam
                {
                    Name = "Test 2",
                    Started = DateTime.Now.AddHours(-1),
                    Ended = DateTime.Now.AddHours(0),
                    Project = project,
                    Comment = "Test comment",
                    Date = DateTime.Now.Date
                }),
                Entry.Create(new EntryCreateParam
                {
                    Name = "Test 3",
                    Started = DateTime.Now.AddHours(-1),
                    Ended = DateTime.Now.AddHours(0),
                    Project = project,
                    Comment = "Test comment",
                    Date = DateTime.Now.Date
                }),
            };

            this.uow.Entries.AddRange(entries);
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

        [TestMethod]
        public void UpdateProject_Test()
        {
            // ARRANGE
            CreateProject();
            var projectToUpdate = this.uow.Projects.GetAll().First();
            var updateParam = new ProjectUpdateParam
            {
                Description = "Test description updated",
                Name = "Test project updated"
            };
            var projectLogic = new ProjectLogic();

            // ACT
            projectLogic.UpdateProject(updateParam, projectToUpdate.Id);
            projectLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updatedProject = this.uow.Projects.Get(projectToUpdate.Id);
            Assert.IsNotNull(updatedProject);
            Assert.AreNotEqual(projectToUpdate.Name, updatedProject.Name);
            Assert.AreEqual(updateParam.Name, updatedProject.Name);
            Assert.AreEqual(updateParam.Description, updatedProject.Description);
        }

        [TestMethod]
        public void SetStatus_Test()
        {
            // ARRANGE
            CreateProject();
            var projectToUpdate = this.uow.Projects.GetAll().First();
            var projectLogic = new ProjectLogic();

            // ACT
            projectLogic.SetStatus(projectToUpdate.Id, EnumDefinition.ProjectStatus.Archived);
            projectLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var updatedProject = this.uow.Projects.Get(projectToUpdate.Id);
            Assert.IsNotNull(updatedProject);
            Assert.AreEqual(EnumDefinition.ProjectStatus.Archived, updatedProject.ProjectStatus);
        }

        [TestMethod]
        public void DeleteProject_TestSuccess()
        {
            // ARRANGE
            CreateProject();
            var toBeDeleted = this.uow.Projects.GetAll().First();
            var projectLogic = new ProjectLogic();

            // ACT
            projectLogic.SetStatus(toBeDeleted.Id, EnumDefinition.ProjectStatus.Archived);
            projectLogic.DeleteProject(toBeDeleted.Id);
            projectLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var deleted = this.uow.Projects.Get(toBeDeleted.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteProject_TestFail()
        {
            // ARRANGE
            CreateProject();
            var toBeDeleted = this.uow.Projects.GetAll().First();
            var projectLogic = new ProjectLogic();

            // ACT
            projectLogic.DeleteProject(toBeDeleted.Id);
            projectLogic.Dispose();
            this.uow.Dispose();
            this.uow = new UnitOfWork();

            // ASSERT
            var deleted = this.uow.Projects.Get(toBeDeleted.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void RetrieveWorkingTime_Test()
        {
            // ARRANGE
            CreateProjectWithEntries();
            var projectWithEntries = this.uow.Projects.GetAll().First();
            var projectLogic = new ProjectLogic();

            // ACT
            var workingTime = projectLogic.RetrieveWorkingTime(projectWithEntries.Id);
            projectLogic.Dispose();

            // ASSERT
            var expected = new TimeSpan(3, 0, 0);
            Assert.IsTrue(expected <= workingTime);
        }

        [TestMethod]
        public void GetAllActiveProjects_Test()
        {
            // ARRANGE
            CreateProjects();
            var projectWithArchivedStatus = this.uow.Projects.GetAll().First();
            projectWithArchivedStatus.SetStatus(EnumDefinition.ProjectStatus.Archived);
            this.uow.Complete();
            var projectLogic = new ProjectLogic();

            // ACT
            var activeProjects = projectLogic.GetAllActiveProjects();

            // ASSERT
            Assert.IsTrue(activeProjects.Count() == 2);
            Assert.IsTrue(activeProjects.All(p => p.ProjectStatus == EnumDefinition.ProjectStatus.Active));
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
