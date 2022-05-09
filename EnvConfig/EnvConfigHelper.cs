using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace EnvConfig;

public class EnvConfigHelper {
    // public static TConfig ReadAll<TConfig>(IConfiguration configuration) where TConfig : class {
    //     configuration.
    // }
    public static TConfig ReadAll<TConfig>(IConfiguration? configuration = null) where TConfig : class {
        var config = Activator.CreateInstance<TConfig>();
        var props = typeof(TConfig).GetProperties();
        var hasConfig = configuration is not null;
        foreach (var propInfo in props) {
            if (propInfo.GetCustomAttribute(typeof(FromEnvAttribute)) is not FromEnvAttribute attr || attr.VariableName is null) continue;
            string? rawValue = null;

            // cek dulu dari environment
            if (attr.VariableName is not null) {
                rawValue = Environment.GetEnvironmentVariable(attr.VariableName);
            }

            // jika tidak ada coba cek di json file
            if (rawValue is null && hasConfig) {
                rawValue = GetValue(configuration!, propInfo.Name, attr.Section);
            }

            // propInfo.PropertyType;
            var propType = propInfo.PropertyType;
            if (propType == typeof(string)) {
                propInfo.SetValue(config, rawValue);
                continue;
            }

            var isNullable = propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable) {
                propType = new NullableConverter(propType).UnderlyingType;
            }

            object? value;

            if (propType.IsEnum) {
                value = ParseAsEnum(propType, rawValue);
            } else {
                var paramTypes = new[] {typeof(string), propType.MakeByRefType()};
                var parser = propType.GetMethod("TryParse", paramTypes);

                if (parser is null) continue;

                var args = new object?[] {rawValue, null};
                var result = parser.Invoke(null, args);
                if (result is null || !(bool) result) continue;
                value = args[1];
            }

            if (value is not null) {
                propInfo.SetValue(config, value);
                // continue;
            }
        }

        return config;
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
