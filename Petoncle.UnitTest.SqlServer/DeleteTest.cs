using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Delete")]
public class DeleteTest : General
{
    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
    }

    [Test]
    public void DeleteAll()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Petoncle.Db.Delete<User>();
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));

    }

    [Test]
    public void DeleteWhere()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Petoncle.Db.Insert(new User {LastName = "Alo", Age = 23});
        Petoncle.Db.Insert(new User {LastName = "Alu", Age = 24});
        Petoncle.Db.Delete<User>(u => u.Age == 23);
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
    }
}