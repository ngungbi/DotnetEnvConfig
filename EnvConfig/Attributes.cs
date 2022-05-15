namespace Ngb.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public class FromConfigAttribute : Attribute {
    internal string? VariableName { get; }
    public string? Section { get; init; }
    public string? Key { get; init; }

    public FromConfigAttribute() { }

    public FromConfigAttribute(string variable) {
        VariableName = variable;
    }
}
