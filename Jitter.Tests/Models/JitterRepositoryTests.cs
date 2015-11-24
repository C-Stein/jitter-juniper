using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jitter.Models;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace Jitter.Tests.Models
{
    [TestClass]
    public class JitterRepositoryTests
    {
        private Mock<JitterContext> mock_context;
        private Mock<DbSet<JitterUser>> mock_set;
        private JitterRepository repository;

        private void ConnectMocksToDataStore(IEnumerable<JitterUser> data_store)
        {
            var data_source = data_store.AsQueryable<JitterUser>();
            // HINT HINT: var data_source = (data_store as IEnumerable<JitterUser>).AsQueryable();
            // Convince LINQ that our Mock DbSet is a (relational) Data store.
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());
            
            // This is Stubbing the JitterUsers property getter
            mock_context.Setup(a => a.JitterUsers).Returns(mock_set.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            mock_context = new Mock<JitterContext>();
            mock_set = new Mock<DbSet<JitterUser>>();
            repository = new JitterRepository(mock_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mock_context = null;
            mock_set = null;
            repository = null;
        }

        [TestMethod]
        public void JitterContextEnsureICanCreateInstance()
        {
            JitterContext context = mock_context.Object;
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanCreatInstance()
        {
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanGetAllUsers()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);

            // Act
            var actual = repository.GetAllUsers();
            // Assert

            Assert.AreEqual("adam1", actual.First().Handle);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void JitterRepositoryEnsureIHaveAContext()
        {
            // Arrange
            // Act
            var actual = repository.Context;
            // Assert
            Assert.IsInstanceOfType(actual, typeof(JitterContext));
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanGetUserByHandle()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);
          
            ConnectMocksToDataStore(expected);
            //act
            string handle = "rumbadancer2";
            JitterUser actual_user = repository.GetUserByHandle(handle);
            //assert
            Assert.AreEqual("rumbadancer2", actual_user.Handle);
        }

        [TestMethod]
        public void JitterRepositoryGetUserByHandleUserDoesNotExist()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            //act
            string handle = "bogus";
            JitterUser actual_user = repository.GetUserByHandle(handle);
            //assert
            Assert.IsNull(actual_user);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void JitterRepositoryGetUserByHandleFailsMultipleUsers()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "adam1"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            //act
            string handle = "adam1";
            JitterUser actual_user = repository.GetUserByHandle(handle);
        }

        [TestMethod]
        public void JitterRepositoryEnsureHandleIsAvailable()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            //act
            string handle = "bogus";
            bool actual_user = repository.IsHandleAvailable(handle);
            //assert
            Assert.IsTrue(actual_user);
        }

        [TestMethod]
        public void JitterRepositoryEnsureHandleIsNotAvailable()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            //act
            string handle = "adam1";
            bool actual_user = repository.IsHandleAvailable(handle);
            //assert
            Assert.IsFalse(actual_user);
        }

        [TestMethod]
        public void JitterRepositoryEnsureHandleIsNotAvailableMultipleUsers()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "adam1"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            //act
            string handle = "adam1";
            bool actual_user = repository.IsHandleAvailable(handle);
            //assert
            Assert.IsFalse(actual_user);
        }
            [TestMethod]
        public void JitterRepositoryEnsureICanSearchByHandle()
        {
            //arrange
            var expected = new List<JitterUser>
            {
                new JitterUser { Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"},
                new JitterUser { Handle = "treeHugger"},
                new JitterUser { Handle = "treeDancer"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            //act
            string handle = "tree";
            List<JitterUser> expected_users = new List<JitterUser>
            {
                new JitterUser { Handle = "treeDancer"},
                new JitterUser { Handle = "treeHugger"}
            };
            List<JitterUser> actual_users = repository.SearchByHandle(handle);
            //assert
            //Collection assert doesn;t use compareTo under the hood
            //CollectionAssert.AreEqual(expected_users, actual_users);
            Assert.AreEqual("treeDancer", actual_users[0].Handle);
            Assert.AreEqual("treeHugger", actual_users[1].Handle);

        }

    }
}
