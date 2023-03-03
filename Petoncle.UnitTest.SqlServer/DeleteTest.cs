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

    private void AddRows()
    {
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22}, 
            new User {LastName = "Alo", Age = 23},
            new User {LastName = "Alu", Age = 24});
    }

    [Test]
    public void DeleteAll()
    {
        Assert.Throws<PetoncleException>(() => { Petoncle.Db.Delete(null); });
        AddRows();
        Petoncle.Db.Delete<User>();
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DeleteTableName()
    {
        AddRows();
        Petoncle.Db.Delete("users");
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DeleteSchemaNameAndTableName()
    {
        AddRows();
        Petoncle.Db.Delete("dbo", "users");
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DeleteObject()
    {
        AddRows();
        User user = Petoncle.Db.Select<User>(u => u.Age == 23).First();
        Petoncle.Db.Delete(user);
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u => u.Age == 23).Count, Is.EqualTo(0));
    }

    [Test]
    public void DeleteWhereExpression()
    {
        AddRows();
        Petoncle.Db.Delete<User>(u => u.Age == 23);
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DeleteWhereExpressionAnonymous()
    {
        AddRows();
        Petoncle.Db.Delete<UserAnonymous>("users", u => u.Age == 23);
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
        AddRows();
        Petoncle.Db.Delete<UserAnonymous>("dbo","users", u => u.Age == 23);
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DeleteWhereSql()
    {
        AddRows();
        Petoncle.Db.Delete<User>(new Sql("Age = @Age", new {Age = 23}));
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
    }
    
    [Test]
    public void DeleteWhereSqlDynamic()
    {
        AddRows();
        Petoncle.Db.Delete("users",new Sql("Age = @Age", new {Age = 23}));
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
        
        AddRows();
        Petoncle.Db.Delete("dbo","users",new Sql("Age = @Age", new {Age = 23}));
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(2));
        Assert.That(Petoncle.Db.Select<User>(u=>u.Age == 23).Count, Is.EqualTo(0));
    }
}