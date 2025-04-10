using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SigurSeacherAvalonia.Configurations;

internal sealed class Configuration
{
    private readonly IConfiguration _config;

    public static Configuration Instance { get; } = new Configuration ();

    private Configuration ()
    {
        _config = new ConfigurationBuilder ()
            .AddJsonFile (Path.Combine(Environment.CurrentDirectory, "Resources", "appsettings.json"))
            .Build ();
    }

    public string ConnectionString { get => _config.GetSection ("Settings") ["ConnectionString"]; }
    public string Password { get => _config.GetSection ("Settings") ["Password"]; }
}