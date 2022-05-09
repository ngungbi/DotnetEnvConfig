using System;
using EnvConfig;
using Microsoft.Extensions.Configuration;

namespace Test;

public enum EnumTest {
    Satu,
    Dua,
    Tiga
}

public class Config {
    [FromEnv("TEST_STRING")]
    public string? TestString { get; init; }

    [FromEnv("TEST_STRING_DEFAULT")]
    public string? TestStringDefault { get; init; } = "DefaultString";

    [FromEnv("TEST_INTEGER")]
    public int TestInteger { get; init; }

    [FromEnv("INT_NULLABLE")]
    public int? TestIntNull { get; init; }

    [FromEnv("TEST_ENUM")]
    public EnumTest TestEnum { get; init; }

    [FromEnv("TEST_DATE")]
    public DateOnly TestDate { get; init; }

    [FromEnv("TIMESTAMP")]
    public DateTime TestDateTime { get; init; }

    [FromEnv(Section = "ConnectionStrings")]
    public string DatabaseConnection { get; init; } = string.Empty;

    public Config() : this(null) { }

    public Config(IConfiguration? configuration) {
        EnvConfigHelper.ReadAll(this, configuration);
    }
}
