# Configuration Helper
Read all configurations from environment and IConfiguration to a class.

## Features

- Simple configuration loader
- Auto load named variable to property
- Automatic type conversion

## Installation
```
dotnet add package Ngb.Configuration
```
Using package
```c#
using Ngb.Configuration;
```

## Common Usage
Example `appsettings.json`
```json
{
  "ConnectionStrings": {
    "Default": "",
    "Sqlite": ""
  },
  "ApiUrl": "https://example.com",
  "ServerPort": "8080"
}
```
Define object class
```c#
// example config class
public class GlobalConfig {
    // this will load environment variable API_URL or use default value if not exists
    [FromConfig("API_URL")]
    public string ApiUrl { get; set; } "http://localhost";
    
    [FromConfig("SERVER_PORT")
    public int ServerPort { get; set; }
}
```
Call from main
```c#
var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
var globalConfig = ConfigReader.ReadAll<GlobalConfig>(configuration);
```
## Object with constructor
Prepare class
```c#
public class GlobalConfig {
    // this will load environment variable API_URL
    [FromConfig("API_URL")]
    public string ApiUrl { get; set; }
    
    [FromConfig("SERVER_PORT")
    public int ServerPort { get; set; }
    
    public GlobalConfig(IConfiguration? configuration) {
        // set configuration to null to ignore IConfiguration
        ConfigReader.ReadAll(this, configuration);
    }
}
```
Call from main
```c#
var builder = WebApplication.CreateBuilder;
var globalConfig = new GlobalConfig(builder.Configuration);
```

## Sectional settings
```c#
// example config class
public class GlobalConfig {
    // this will load environment variable API_URL
    [FromConfig("DEFAULT_CONNECTION", Section = "ConnectionStrings", Key = "Default")]
    public string DatabaseServer { get; set; }
    
    [FromConfig("SQLITE_CONNECTION", Section = "ConnectionStrings")
    public string Sqlite { get; set; }
    
    public GlobalConfig(IConfiguration? configuration) {
        // set configuration to null to ignore IConfiguration
        ConfigReader.ReadAll(this, configuration);
    }
}
```

## Config object in config
```c#
public class GlobalConfig {
    // this will load environment variable API_URL
    [FromConfig("API_URL")]
    public string ApiUrl { get; set; }
    
    [FromConfig("SERVER_PORT")
    public int ServerPort { get; set; }
    
    DatabaseConfig DatabaseConnection { get; }
    
    public GlobalConfig(IConfiguration? configuration) {
        // set configuration to null to ignore IConfiguration
        ConfigReader.ReadAll(this, configuration);
        var section = configuration?.GetSection("ConnectionStrings");
        DatabaseConnection = new DatabaseConfig(section);
    }
}

public class DatabaseConfig {
    // this will load environment variable API_URL
    [FromConfig("DEFAULT_CONNECTION", Key = "Default")]
    public string DatabaseServer { get; set; }
    
    [FromConfig("SQLITE_CONNECTION")
    public string Sqlite { get; set; }
    
    public GlobalConfig(IConfiguration? configuration) {
        // set configuration to null to ignore IConfiguration
        ConfigReader.ReadAll(this, configuration);
    }
}
```

## Drawback
- No hot reload
