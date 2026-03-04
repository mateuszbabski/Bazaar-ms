namespace Bazaar.Shared.Infrastructure.Auth
{
    public class ServiceTokenSettings
    {
        public string Key { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int DurationInMinutes { get; init; }
    }
}
