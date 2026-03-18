namespace Bazaar.Products.Service.Domain.ErrorCodes
{
    public enum ErrorCode
    {
        UnknownError,        
        ValidationFailed,
        CustomerNotFound,
        ProductNotFound,
        ProductsNotFound,
        CategoryNotFound,
        InvalidProductData,
        InvalidCurrency
    }
}
