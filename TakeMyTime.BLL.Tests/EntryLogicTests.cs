using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DAL.uow;

namespace TakeMyTime.BLL.Tests
{
    [TestClass]
    public class EntryLogicTests
    {
        UnitOfWork uow;

        [TestInitialize]
        public void InitializeTestingEnvironment()
        {
            this.uow = new UnitOfWork();
            this.uow.CreateDatabase();
        }

        #region SetUpTestingEnvironment



        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            this.uow.DeleteDatabase();
            this.uow.Dispose();
        }
    }
}
