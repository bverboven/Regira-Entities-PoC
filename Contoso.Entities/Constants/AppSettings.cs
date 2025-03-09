namespace Contoso.Constants;

public static class AppSettings
{
    public static string DataFolder => new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "../../../../data")).FullName;
    public static string ConnectionString => $"DataSource={DataFolder}/contoso.db";
}