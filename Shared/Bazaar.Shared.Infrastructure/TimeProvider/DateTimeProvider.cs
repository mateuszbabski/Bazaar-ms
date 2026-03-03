namespace Bazaar.Shared.Infrastructure.TimeProvider
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTimeOffset UtcTimeNow => DateTimeOffset.UtcNow;
        public DateTimeOffset LocalTimeNow => DateTimeOffset.Now;
    }
}
