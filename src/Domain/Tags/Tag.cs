namespace WomensWiki.Domain.Tags;

public class Tag : Entity {
    public string Name { get; private set; } = null!;

    public static Tag Create(string name) {
        return new Tag {
            Name = name
        };
    }
}