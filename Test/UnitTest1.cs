using System;
using EnvConfig;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Test;

public class Tests {
    [SetUp]
    public void Setup() {
        SetEnvironments();
    }

    [Test]
    public void Test1() {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appsettings.json");
        var config = EnvConfigHelper.ReadAll<Config>(configBuilder.Build());
        Assert.IsTrue(config.TestString == "1234");
        Assert.IsTrue(config.TestInteger == 1234);
        Assert.IsTrue(config.TestEnum == EnumTest.Dua);
        Assert.AreEqual(config.DatabaseConnection, "1234");
        Assert.AreEqual(config.TestDate, new DateOnly(2022, 2, 20));

        Assert.Pass();
    }

    private static void SetEnvironments() {
        Environment.SetEnvironmentVariable("TEST_STRING", "1234");
        Environment.SetEnvironmentVariable("TEST_INTEGER", "1234");
        Environment.SetEnvironmentVariable("INT_NULLABLE", "1234");
        Environment.SetEnvironmentVariable("TEST_ENUM", "Dua");
        Environment.SetEnvironmentVariable("TEST_DATE", "2022-02-20");
    }
}
