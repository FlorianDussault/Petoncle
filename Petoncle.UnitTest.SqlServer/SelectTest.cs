using PetoncleDb.SqlServer;
using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Select")]
public class SelectTest : General
{
    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {FirstName = "Bob", LastName = "Sponge", Age = 11, Enabled = false});
        Petoncle.Db.Insert(new User {FirstName = "John", LastName = "Doe", Age = 10, Enabled = true});
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22, Enabled = false});
    }

    [Test]
    public void SelectPetoncleObject()
    {
        List<User> list = Petoncle.Db.Select<User>().ToList();
        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(list[0].Id, Is.EqualTo(1));
        Assert.That(list[0].FirstName, Is.EqualTo("Bob"));
        Assert.That(list[2].Id, Is.EqualTo(3));
        Assert.That(list[2].FirstName, Is.Null);
        Assert.That(list[2].LastName, Is.EqualTo("Ali"));
    }
    
    [Test]
    public void SelectAnonymousObject()
    {
        List<UserAnonymous> list = Petoncle.Db.Select<UserAnonymous>("users").ToList();
        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(list[0].Id, Is.EqualTo(1));
        Assert.That(list[0].FirstName, Is.EqualTo("Bob"));
        Assert.That(list[2].Id, Is.EqualTo(3));
        Assert.That(list[2].FirstName, Is.Null);
        Assert.That(list[2].LastName, Is.EqualTo("Ali"));
    }
    
    [Test]
    public void SelectDynamicObject()
    {
        // TODO: Reactivate
        List<dynamic> list = Petoncle.Db.Select("users").ToList();
        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(list[0].id, Is.EqualTo(1));
        Assert.That(list[0].firstname, Is.EqualTo("Bob"));
        Assert.That(list[2].id, Is.EqualTo(3));
        Assert.That(list[2].firstname, Is.Null);
        Assert.That(list[2].lastname, Is.EqualTo("Ali"));
    }

    [Test]
    public void SelectCount()
    {
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(3));
        Assert.That(Petoncle.Db.Select("users").Count, Is.EqualTo(3));
    }

    [Test]
    public void SelectWhereExpression()
    {
        List<User> list = Petoncle.Db.Select<User>(u=>u.Age == 22).OrderBy(u=>u.Age).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].LastName, Is.EqualTo("Ali"));
    }
    
    [Test]
    public void SelectWhereSql()
    {
        List<dynamic> list = Petoncle.Db.Select<dynamic>("users", new Sql("Age = @Age", new {Age = 22})).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].lastname, Is.EqualTo("Ali"));
    }

    [Test]
    public void SelectOrderBy()
    {
        List<User> list = Petoncle.Db.Select<User>(u => u.Age < 100).OrderByAsc(u => u.Age).ToList();
        Assert.That(list[0].Age, Is.EqualTo(10));
        Assert.That(list[1].Age, Is.EqualTo(11));
        Assert.That(list[2].Age, Is.EqualTo(22));
    }
    
    [Test]
    public void SelectOrderByDesc()
    {
        List<User> list = Petoncle.Db.Select<User>(u => u.Age < 100).OrderByDesc(u => u.Age).ToList();
        Assert.That(list[0].Age, Is.EqualTo(22));
        Assert.That(list[1].Age, Is.EqualTo(11));
        Assert.That(list[2].Age, Is.EqualTo(10));
    }
    
    [Test]
    public void SelectOrderBySql()
    {
        List<User> list = Petoncle.Db.Select<User>(u => u.Age < 100).OrderByAsc("age", "firstname").ToList();
        Assert.That(list[0].Age, Is.EqualTo(10));
        Assert.That(list[1].Age, Is.EqualTo(11));
        Assert.That(list[2].Age, Is.EqualTo(22));
    }
    
    [Test]
    public void SelectOrderByDescSql()
    {
        List<User> list = Petoncle.Db.Select<User>(u => u.Age < 100).OrderByDesc("age").ToList();
        Assert.That(list[0].Age, Is.EqualTo(22));
        Assert.That(list[1].Age, Is.EqualTo(11));
        Assert.That(list[2].Age, Is.EqualTo(10));
    }

    [Test]
    public void SelectTop()
    {
        List<User> list = Petoncle.Db.Select<User>(u => u.Age < 100).OrderByDesc("age").Top(2).ToList();
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0].Age, Is.EqualTo(22));
        Assert.That(list[1].Age, Is.EqualTo(11));
    }

    [Test]
    public void SelectColumns()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => u.Age, u => u.Id).ToList();
        Assert.That(list.Count, Is.EqualTo(3));
        foreach (User user in list)
        {
            Assert.That(user.Id, Is.Not.EqualTo(0));
            Assert.That(user.Age, Is.Not.EqualTo(0));
            Assert.That(user.LastName, Is.Null);
            Assert.That(user.FirstName, Is.Null);
            Assert.That(user.Enabled, Is.False);
            Assert.That(user.CreateDate, Is.Null);
            
        }
    }
    
    [Test]
    public void SelectAs()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => (u.Age + 1).As("Id"), u => u.Age).ToList();
        Assert.That(list.Count, Is.EqualTo(3));
        foreach (User user in list)
            Assert.That(user.Id, Is.EqualTo(user.Age + 1));
    }

    [Test]
    public void SelectSum()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => u.Age.Sum().As("Age")).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age, Is.EqualTo(43));
    }
    
    [Test]
    public void SelectMin()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => u.Age.Min().As("Age")).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age, Is.EqualTo(10));
    }
    
    [Test]
    public void SelectMax()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => u.Age.Max().As("Age")).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age, Is.EqualTo(22));
    }
    
    [Test]
    public void SelectAvg()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => u.Age.Avg().As("Age")).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age, Is.EqualTo(14));
    }

    [Test]
    public void SelectGroupBy()
    {
        List<User> list = Petoncle.Db.Select<User>().Columns(u => u.Enabled, u => 1.Count().As("Age")).GroupBy(u=>u.Enabled).OrderByAsc(u=>u.Enabled).ToList();
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0].Enabled, Is.False);
        Assert.That(list[0].Age, Is.EqualTo(2));
        Assert.That(list[1].Enabled, Is.True);
        Assert.That(list[1].Age, Is.EqualTo(1));
    }
    
    
    
}