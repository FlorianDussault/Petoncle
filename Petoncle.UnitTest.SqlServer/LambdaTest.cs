using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Lambda Expressions")]
public class LambdaTest : General
{
    private int _nbRows;

    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
        Petoncle.Db.Truncate<User>();
        Petoncle.Db.Insert(new User {FirstName = "John", LastName = "Doe", Age = 18, CreateDate = DateTime.Now});
        Petoncle.Db.Insert(new User {FirstName = "Bob", LastName = "Sponge", Age = 1});
        Petoncle.Db.Insert(new User {LastName = "Ali", Age = 22});
        Petoncle.Db.Insert(new User {FirstName = "Bob", LastName = "Inou", Age = 12});
        Petoncle.Db.Insert(new User {FirstName = "Bobo", LastName = "Inou", Age = 12});
        _nbRows = Petoncle.Db.Select<User>().Count;
    }

    [Test]
    public void IsNull()
    {
        Assert.That(Petoncle.Db.Select<User>(u => u.CreateDate == null).Count, Is.EqualTo(4));
        Assert.That(Petoncle.Db.Select<User>(u => u.CreateDate != null).Count, Is.EqualTo(1));
    }
    
    [Test]
    public void Function_Like()
    {
        Assert.Throws<PetoncleException>(() => { "HELLO".Like("ok"); });
        List<User> list = Petoncle.Db.Select<User>(u => u.FirstName.Like("%ob")).ToList();
        Assert.That(list.Count, Is.EqualTo(2));
        foreach (User user in list)
        {
            Assert.That(user.FirstName.EndsWith("ob"), Is.True);
        }
    }
    
    [Test]
    public void Function_NotLike()
    {
        Assert.Throws<PetoncleException>(() => { "HELLO".NotLike("ok"); });
        List<User> list = Petoncle.Db.Select<User>(u => u.FirstName.NotLike("%ob")).ToList();
        Assert.That(list.Count, Is.EqualTo(2));
        foreach (User user in list)
        {
            Assert.That(!user.FirstName.EndsWith("ob"), Is.True);
        }
    }
    
    #region Date
    [Test]
    public void Function_GetDate()
    {
        Assert.Throws<PetoncleException>(() => { DateTime.Now.Day(); });
        Assert.That(Petoncle.Db.Select<User>(u=>Pf.GetDate().IsDate() == true).Count, Is.EqualTo(_nbRows));
    }
    
    [Test]
    public void Function_Day()
    {
        Assert.Throws<PetoncleException>(() => { 0.Day(); });
        Assert.That(Petoncle.Db.Select<User>(u=>u.CreateDate.Day() == DateTime.Now.Day).Count, Is.EqualTo(1));
    }
    
    [Test]
    public void Function_Month()
    {
        Assert.Throws<PetoncleException>(() => { 0.Month(); });
        Assert.That(Petoncle.Db.Select<User>(u=>u.CreateDate.Month() == DateTime.Now.Month).Count, Is.EqualTo(1));
    }
    
    [Test]
    public void Function_Year()
    {
        Assert.Throws<PetoncleException>(() => { 0.Year(); });
        Assert.That(Petoncle.Db.Select<User>(u=>u.CreateDate.Year() == DateTime.Now.Year).Count, Is.EqualTo(1));
    }
    
    [Test]
    public void Function_DateAdd()
    {
        Assert.Throws<PetoncleException>(() => { 0.DateAdd(DatePart.Day, 1); });
        Assert.That(Petoncle.Db.Select<User>(u=>u.CreateDate.DateAdd(DatePart.Day,1).Day() == DateTime.Now.AddDays(1).Day).Count, Is.EqualTo(1));
    }
    
    [Test]
    public void Function_DateDiff()
    {
        Assert.Throws<PetoncleException>(() => { Pf.DateDiff(DatePart.Year, DateTime.Now, DateTime.Now); });
        Assert.That(Petoncle.Db.Select<User>(u=>Pf.DateDiff(DatePart.Year, DateTime.Now, DateTime.Now.AddYears(10)) == 10).Count, Is.EqualTo(_nbRows));
    }
    #endregion

    #region String
    
    [Test]
    public void Function_Ascii()
    {
        Assert.Throws<PetoncleException>(() => { "".Ascii(); });
        List<User> list = Petoncle.Db.Select<User>(u => u.FirstName.Ascii() == 74).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age == 18);
    }
    
    [Test]
    public void Function_Char()
    {
        Assert.Throws<PetoncleException>(() => { 1.Char(); });
        List<User> list = Petoncle.Db.Select<User>(u => u.FirstName.Ascii().Char() == "J").ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age == 18);
    }
    
    [Test]
    public void Function_CharIndex()
    {
        Assert.Throws<PetoncleException>(() => { 1.Char(); });
        List<User> list = Petoncle.Db.Select<User>(u => u.FirstName.CharIndex("o",0) == 2 && u.FirstName.CharIndex("h", 0) == 3).ToList();
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Age == 18);
    }
    
    #endregion
}