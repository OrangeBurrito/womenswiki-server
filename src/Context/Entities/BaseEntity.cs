public class BaseEntity {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
}