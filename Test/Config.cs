using System;
using Microsoft.Extensions.Configuration;
using Ngb.Configuration;

namespace Test;

public enum EnumTest {
    Satu,
    Dua,
    Tiga
}

public class Config {
    [FromConfig("TEST_STRING")]
    public string? TestString { get; init; }

    [FromConfig("TEST_STRING_DEFAULT")]
    public string? TestStringDefault { get; init; } = "DefaultString";

    [FromConfig("TEST_INTEGER")]
    public int TestInteger { get; init; }

    [FromConfig("INT_NULLABLE")]
    public int? TestIntNull { get; init; }

    [FromConfig("TEST_ENUM")]
    public EnumTest TestEnum { get; init; }

    [FromConfig("TEST_DATE")]
    public DateOnly TestDate { get; init; }

    [FromConfig("TIMESTAMP")]
    public DateTime TestDateTime { get; init; }

    [FromConfig(Section = "ConnectionStrings", Key = "Default")]
    public string DatabaseConnection { get; init; } = string.Empty;

    public Config() : this(null) { }

    public Config(IConfiguration? configuration) {
        ConfigReader.ReadAll(this, configuration);
    }
}
