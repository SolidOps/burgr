namespace Acme.Shop.Application
{
    // Service 
    public partial interface IInventoryService
    {

        Task Restock(System.Guid productId, System.Int32 quantity); // POST shop/inventory/restock

        Task<object> CountInStock(); // shop/inventory/count-in-stock

        Task<object[]> FindFeatured(); // shop/inventory/find-featured

    }

}