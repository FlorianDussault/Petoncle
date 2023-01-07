using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

[TestFixture(TestName = "Initialize")]
public class InitializeTest : General
{
    [SetUp]
    public void Setup()
    {
    }

    private void Reset()
    {
#if DEBUG
        Petoncle.Reset();
#endif
    }
    
    [Test]
    public void InitializeWithoutConnection()
    {
        Reset();
        Assert.Throws<ArgumentNullException>(() => Petoncle.Initialize(DatabaseType.SqlServer, null));
        Reset();
        Assert.Throws<ArgumentNullException>(() => Petoncle.Initialize<User>(DatabaseType.SqlServer, null));
    }
    
    [Test]
    public void InitializePetoncleObject()
    {
        Reset();
        Petoncle.Initialize<User>(DatabaseType.SqlServer, ()=>new SqlConnection(ConnectionString));
    }
    
    [Test]
    public void InitializeWithAnonymousObject()
    {
        Reset();
        Petoncle.Initialize<Department>(DatabaseType.SqlServer, () => new SqlConnection(ConnectionString));
    }
    
    [Test]
    public void InitializeWithTypeObject()
    {
        Reset();
        Assert.Throws<PetoncleException>(() => Petoncle.Initialize<object>(DatabaseType.SqlServer, () => new SqlConnection(ConnectionString)));
    }
}