using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Select")]
public class SelectTest : General
{
    [SetUp]
    public void Setup()
    {
        InitializePetoncle();
    }

    [Test]
    public void SelectPetoncleObject()
    {
        List<User> a = Petoncle.Db.Select<User>().ToList();
    }
    
    [Test]
    public void SelectAnonymousObject()
    {
        List<UserAnonymous> a = Petoncle.Db.Select<UserAnonymous>("users").ToList();
    }
    
    [Test]
    public void SelectDynamicObject()
    {
        // TODO: Reactivate
        List<dynamic> a = Petoncle.Db.Select<dynamic>("users").ToList();
    }
    
}