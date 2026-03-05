namespace Bazaar.Customers.Service.Commands.Responses
{
    // move to shared library if needed by other services
    public enum ErrorCode
    {
        EmailAlreadyRegistered,
        ValidationFailed,
        CustomerNotFound,
        InvalidCredentials,
        UnknownError
    }
}
