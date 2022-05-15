using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Ngb.Configuration;

public class ConfigReader {
    /// <summary>
    /// Create a new instance of TConfig and load all configurations.
    /// </summary>
    /// <param name="configuration">Reference to existing IConfiguration object</param>
    /// <typeparam name="TConfig">Class of configuration object</typeparam>
    /// <returns></returns>
    public static TConfig ReadAll<TConfig>(IConfiguration configuration) where TConfig : class {
        var target = Activator.CreateInstance<TConfig>();
        return ReadAll(target, configuration);
    }

    /// <summary>
    /// Load all configurations to an existing TConfig target.
    /// </summary>
    /// <param name="target">Target object</param>
    /// <param name="configuration">Reference to existing IConfiguration object</param>
    /// <typeparam name="TConfig">Class of configuration object</typeparam>
    /// <returns></returns>
    public static TConfig ReadAll<TConfig>(TConfig target, IConfiguration? configuration = null) where TConfig : class {
        var props = typeof(TConfig).GetProperties();
        var hasConfig = configuration is not null;
        foreach (var propInfo in props) {
            if (propInfo.GetCustomAttribute(typeof(FromConfigAttribute)) is not FromConfigAttribute attr) continue;
            string? rawValue = null;

            // cek dulu dari environment
            if (attr.VariableName is not null) {
                rawValue = Environment.GetEnvironmentVariable(attr.VariableName);
            }

            // jika tidak ada coba cek di json file
            if (rawValue is null && hasConfig) {
                rawValue = GetValue(configuration!, attr.Key ?? propInfo.Name, attr.Section);
            }

            // jika masih tidak ada, abaikan
            if (rawValue is null) continue;

            // cari tipe aslinya jika nullable
            var propType = GetBaseType(propInfo.PropertyType);

            // untuk tipe string, langsung saja
            if (propType == typeof(string)) {
                propInfo.SetValue(target, rawValue);
                continue;
            }

            var value = propType.IsEnum ? ParseAsEnum(propType, rawValue) : Parse(propType, rawValue);

            if (value is not null) {
                propInfo.SetValue(target, value);
                // continue;
            }
        }

        return target;
    }

    private static Type GetBaseType(Type type) {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            type = new NullableConverter(type).UnderlyingType;
        }

        return type;
    }

    private static object? Parse(Type type, string? rawValue) {
        var paramTypes = new[] {typeof(string), type.MakeByRefType()};
        var parser = type.GetMethod("TryParse", paramTypes);

        if (parser is null) return null;

        var args = new object?[] {rawValue, null};
        var result = parser.Invoke(null, args);
        if (result is null || !(bool) result) return null;
        return args[1];
    }

    private static string? GetValue(IConfiguration configuration, string name, string? section) {
        // if (configuration is null) return null;
        // var env = Environment.GetEnvironmentVariable(name);
        // if (env is not null) return env;
        // Jika tidak ada di Environment, coba di appsettings
        return section is null ? configuration[name] : configuration.GetSection(section)[name];
    }

    private static object? ParseAsEnum(Type type, string? value) {
        return Enum.TryParse(type, value, true, out var result) ? result : null;
    }
}
