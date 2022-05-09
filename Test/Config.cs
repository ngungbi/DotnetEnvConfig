using System;
using EnvConfig;

namespace Test;

public enum EnumTest {
    Satu,
    Dua,
    Tiga
}

public class Config {
    [FromEnv("TEST_STRING")]
    public string? TestString { get; set; }

    [FromEnv("TEST_STRING_DEFAULT")]
    public string? TestStringDefault { get; set; } = "DefaultString";

    [FromEnv("TEST_INTEGER")]
    public int TestInteger { get; set; }

    [FromEnv("INT_NULLABLE")]
    public int? TestIntNull { get; set; }

    [FromEnv("TEST_ENUM")]
    public EnumTest TestEnum { get; set; }

    [FromEnv("TEST_DATE")]
    public DateOnly TestDate { get; set; }

    [FromEnv("TIMESTAMP")]
    public DateTime TestDateTime { get; set; }

    [FromEnv("DB_CONN", Section = "ConnectionStrings")]
    public string DatabaseConnection { get; set; } = string.Empty;
}
