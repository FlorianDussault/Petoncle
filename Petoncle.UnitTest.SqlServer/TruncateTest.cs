using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Truncate")]
public class TruncateTest : General
{
    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
    }

    [Test]
    public void TruncatePetoncleObject()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(1));
        Petoncle.Db.Truncate<User>();
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));
    }
    
    [Test]
    public void TruncateWithTableName()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(1));
        Petoncle.Db.Truncate("users");
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));
    }
    
    [Test]
    public void TruncateWithSchemaAndTableName()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(1));
        Petoncle.Db.Truncate("dbo", "users");
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));
    }
}