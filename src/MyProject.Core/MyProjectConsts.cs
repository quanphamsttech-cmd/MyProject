using MyProject.Debugging;

namespace MyProject;

public class MyProjectConsts
{
    public const string LocalizationSourceName = "MyProject";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "b025352f30c34fa8b8e09331751720d1";
}
