namespace Bazaar.Shared.Infrastructure.TimeProvider
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        DateTimeOffset UtcTimeNow { get; }
        DateTimeOffset LocalTimeNow { get; }
    }
}
