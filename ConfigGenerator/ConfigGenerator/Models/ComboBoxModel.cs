namespace ConfigGenerator.Models;

public record ComboBoxModel(int Key, string Value)
{
    public string DisplayText => $"{Key}: {Value}";
}
