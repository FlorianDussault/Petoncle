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
        Petoncle.Db.Insert(new User {FirstName = "UT_INSERT_PTCL_OBJ_FN", LastName = "UT_INSERT_PTCL_OBJ_LN", Age = 30});
    }
    
    [Ignore("")]
    public void InsertAnonymousObject()
    {
        throw new NotImplementedException();
        // var a = Petoncle.Db.Select<UserAnonymous>("users", u=>u.Id == 1).ToList();
    }
}