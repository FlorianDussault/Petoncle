using PetoncleUT.SqlServer.Objects;

namespace PetoncleUT.SqlServer;

public class General
{
    protected static readonly string ConnectionString = "Server=localhost\\sqlexpress;Database=Petoncle;TrustServerCertificate=Yes;Trusted_Connection=True";

    protected void InitializePetoncle()
    {
        if (!Petoncle.IsInitialized)
            Petoncle.Initialize<User, UserAnonymous>(DatabaseType.SqlServer, () => new SqlConnection(ConnectionString));
    }
}