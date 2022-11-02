using Microsoft.Extensions.Configuration;

namespace BulkyBook.Configuration;

public class ConnectionStrings
{
    private static readonly IConfiguration Configuration;

    public static string Default => Configuration.GetConnectionString("Default");

    static ConnectionStrings()
    {
        var basePath = Path.GetDirectoryName(typeof(ConnectionStrings).Assembly.Location);

        Configuration =
            new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("db_settings.json")
                .Build();
    }
}