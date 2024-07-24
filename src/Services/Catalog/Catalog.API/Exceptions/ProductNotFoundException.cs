namespace Catalog.API.Exceptions
{
    public sealed class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base("Product not found!") 
        { }
    }
}
