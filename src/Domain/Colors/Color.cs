namespace WomensWiki.Domain.Colors;

public class Color : Entity {
    public string Name { get; private set; } = null!;
    public string Value { get; private set; } = null!;

    public static Color Create(string name, string value) {
        return new Color {
            Name = name,
            Value = value
        };
    }
}