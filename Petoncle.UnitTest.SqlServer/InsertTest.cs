using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Insert")]
public class InsertTest : General
{
    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
    }

    [Test]
    public void InsertPetoncleObject()
    {
        Assert.Throws<PetoncleException>(() => { Petoncle.Db.Insert(new {Age = 23}); });
        Assert.Throws<PetoncleException>(() => { Petoncle.Db.Insert(0); });
        Petoncle.Db.Truncate<User>();
        int cnt = Petoncle.Db.Insert(new User {FirstName = "UT_INSERT_PTCL_OBJ_FN", LastName = "UT_INSERT_PTCL_OBJ_LN", Age = 30});
        Assert.That(cnt, Is.EqualTo(1));
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(1));
    }

    [Test]
    public void InsertAnonymousObject()
    {
        Petoncle.Db.Truncate<User>();
        int cnt = Petoncle.Db.Insert("users", new  {FirstName = "UT_INSERT_PTCL_OBJ_FN", LastName = "UT_INSERT_PTCL_OBJ_LN", Age = 30});
        Assert.That(cnt, Is.EqualTo(1));
        Assert.That(Petoncle.Db.Select<User>().Count, Is.EqualTo(1));
    }
}
