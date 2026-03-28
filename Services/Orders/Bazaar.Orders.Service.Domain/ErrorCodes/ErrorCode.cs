namespace Bazaar.Orders.Service.Domain.ErrorCodes
{
    public enum ErrorCode
    {
        UnknownError,
        ValidationFailed,
        OrderNotFound,
        OrdersNotFound,
        InvalidOrderStatus,
        Forbidden
    }
}
