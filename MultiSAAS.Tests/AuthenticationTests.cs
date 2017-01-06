namespace MultiSAAS.Tests
{
  using NUnit.Framework;
  using System.Collections.Generic;
  using Data.Entities;
  using Moq;
  using Data;
  using Extensions;
  using System.Data.Entity;
  using System.Linq;

  [TestFixture]
  public class AuthenticationTests
  {
    private UserData MockUserCredentials(string username, string password)
    {
      var passwordEncrypted = password.Encrypt();
      var mockUser = new Mock<User>();
      mockUser.SetupGet(u => u.Username).Returns(username);
      mockUser.SetupGet(u => u.Password).Returns(passwordEncrypted);
      var usersInMemory = new List<User> { mockUser.Object };
      var mockUserDbSet = MockDbSet<User>(usersInMemory);
      var context = new Data.DbContext { Users = mockUserDbSet.Object };
      var userData = new UserData(context);
      return userData;
    }

    [Test]
    public void UserLogsIn_WithInvalidPassword_AuthenticationFails()
    {
      //Arrange
      var userData = MockUserCredentials("user", "password");

      //Act
      var User = userData.Authenticate("user", "abc");

      //Assert
      Assert.IsNull(User);
    }

    [Test]
    public void UserLogsIn_WithValidPassword_AuthenticationSucceeds()
    {
      //Arrange
      var userData = MockUserCredentials("user", "password");

      //Act
      var User = userData.Authenticate("user", "password");

      //Assert
      Assert.IsNotNull(User);
    }

    private static Mock<DbSet<T>> MockDbSet<T>() where T: class
    {
      return MockDbSet<T>(null);
    }

    private static Mock<DbSet<T>> MockDbSet<T>(List<T> dataInMemory) where T: class
    {
      if (dataInMemory == null)
      {
        dataInMemory = new List<T>();
      }
      var mockDbSet = new Mock<DbSet<T>>();
      var queryableData = dataInMemory.AsQueryable();

      mockDbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(dataInMemory.Add);
      mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
      mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
      mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
      mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
      mockDbSet.Setup(m => m.AsNoTracking()).Returns(mockDbSet.Object);

      return mockDbSet;
    }
  }
}
