using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Update")]
public class UpdateTest : General
{
    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
    }
    
    private void AddRows()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Petoncle.Db.Insert(new User {LastName = "Alo", Age = 23});
        Petoncle.Db.Insert(new User {LastName = "Alu", Age = 24});
    }

    [Test]
    public void UpdateAll()
    {
        AddRows();
        int changes = Petoncle.Db.Update(new User {FirstName = "UPDATE", LastName = "UP", Age = 99});
        Assert.That(changes, Is.EqualTo(3));
        int count = Petoncle.Db.Select<User>(u => u.FirstName == "UPDATE" && u.LastName == "UP" && u.Age == 99).Count;
        Assert.That(count, Is.EqualTo(3));
    }
    
    [Test]
    public void UpdateWhere()
    {
        AddRows();
        int changes = Petoncle.Db.Update<User>(new User {FirstName = "UPDATE", LastName = "UP", Age = 99}, user => user.Age == 23);
        Assert.That(changes, Is.EqualTo(1));
        int count = Petoncle.Db.Select<User>(u => u.FirstName == "UPDATE" && u.LastName == "UP" && u.Age == 99).Count;
        Assert.That(count, Is.EqualTo(1));
        count = Petoncle.Db.Select<User>(u => u.Age != 99).Count;
        Assert.That(count, Is.EqualTo(2));
    }
}