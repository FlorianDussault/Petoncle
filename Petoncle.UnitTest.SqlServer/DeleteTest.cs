using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "DeleteTest")]
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
        Petoncle.Db.Delete<User>();
    }
}