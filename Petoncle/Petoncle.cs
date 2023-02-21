using System;
using System.Data;

namespace PetoncleDb;

public sealed class Petoncle
{
    #region Singleton

    private static Petoncle _instance;
    
    private static Petoncle Instance
    {
        get { return _instance ??= new Petoncle(); }
        set => _instance = value;
    }
    private Petoncle()
    {
    }
    
    #if DEBUG
    public static void Reset()
    {
        Instance = null;
    }
    #endif

    #endregion

    #region Initialization

    private bool _isInitialized;

    public static bool IsInitialized => Instance._isInitialized;
    
    internal Connection Connection { get; private set; }
    internal ObjectManager ObjectManager{ get; private set; }
    
    public static void Initialize(DatabaseType databaseType, Func<IDbConnection> onConnection) => Instance.InternalInitialize(databaseType, onConnection);

    private void InternalInitialize(DatabaseType databaseType, Func<IDbConnection> onConnection)
    {
        if (_isInitialized) throw new PetoncleException("Petoncle is already initialized");
        ObjectManager = new ObjectManager();
        ObjectManager.CommitTables();
        InternalInitializePetoncle(databaseType, onConnection);        
    }
    
    public static void Initialize<T>(DatabaseType databaseType, Func<IDbConnection> onConnection) => Instance.InternalInitialize<T>(databaseType, onConnection);

    private void InternalInitialize<T>(DatabaseType databaseType, Func<IDbConnection> onConnection)
    {
        if (_isInitialized) throw new PetoncleException("Petoncle is already initialized");
        ObjectManager = new ObjectManager();
        ObjectManager.RegisterObject<T>();
        ObjectManager.CommitTables();
        InternalInitializePetoncle(databaseType, onConnection);        
    }
    
    public static void Initialize<T1, T2>(DatabaseType databaseType, Func<IDbConnection> onConnection) => Instance.InternalInitialize<T1, T2>(databaseType, onConnection);

    private void InternalInitialize<T1, T2>(DatabaseType databaseType, Func<IDbConnection> onConnection)
    {
        if (_isInitialized) throw new PetoncleException("Petoncle is already initialized");
        ObjectManager = new ObjectManager();
        ObjectManager.RegisterObject<T1>();
        ObjectManager.RegisterObject<T2>();
        ObjectManager.CommitTables();
        InternalInitializePetoncle(databaseType, onConnection);        
    }

    private void InternalInitializePetoncle(DatabaseType databaseType, Func<IDbConnection> onConnection)
    {
        if (onConnection == null) throw new ArgumentNullException(nameof(onConnection));
        Connection = new Connection(databaseType, onConnection);
        _dbEntryPoint = new DbEntryPoint(this, Connection);
        _isInitialized = true;
    }
    
    #endregion

    #region Entry Point
    private DbEntryPoint _dbEntryPoint;
    public static EntryPoint Db
    {
        get
        {
            if (Instance == null || !Instance._isInitialized)
                throw new PetoncleException("Petoncle is not Initialized");
            return Instance._dbEntryPoint;
        }
    }

    #endregion
    
    
}