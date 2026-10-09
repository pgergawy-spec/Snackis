namespace SnackisAPI.Settings;

public class SeedSettings
{
    public const string SectionName = "Seed";

    public string AdminUserName { get; set; } = "admin";
    public string AdminEmail { get; set; } = "admin@snackis.local";
    public string AdminPassword { get; set; } = "";
    public string DemoPassword { get; set; } = "";
}
