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
    }
    
    [Test]
    public void TruncateWithTableName()
    {
        Petoncle.Db.Truncate("users");
    }
    
    [Test]
    public void TruncateWithSchemaAndTableName()
    {
        Petoncle.Db.Truncate("dbo", "users");
    }
}