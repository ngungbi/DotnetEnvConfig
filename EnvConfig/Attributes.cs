namespace EnvConfig;

[AttributeUsage(AttributeTargets.Property)]
public class FromEnvAttribute : Attribute {
    internal string? VariableName { get; }
    public string? Section { get; init; }

    public FromEnvAttribute() { }

    public FromEnvAttribute(string variable) {
        VariableName = variable;
    }
}
