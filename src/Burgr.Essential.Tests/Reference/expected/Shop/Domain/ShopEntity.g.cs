// Reference template: one class per aggregate root and entity.
namespace Acme.Shop.Domain
{
    // Object [AG][EN]
    // Product (product)
    public partial class Product
    {

        public Guid Id { get; set; }

        // Property [S][NO][PUO]
        public System.String Name { get; set; } = string.Empty;

        public String? Description { get; set; } = null;

        public System.Decimal Price { get; set; } = default(System.Decimal);

        public System.Int32 Stock { get; set; } = default(System.Int32);

        public System.Double Weight { get; set; } = default(System.Double);

        public System.Boolean IsFeatured { get; set; } = default(System.Boolean);

        public System.DateTime AvailableFrom { get; set; } = default(System.DateTime);

        public System.Guid SupplierReference { get; set; } = default(System.Guid);

        // Property [S][NO][PRO]
        internal System.String InternalCode { get; set; }

        // Property [E][NO]
        public ProductStatusEnum Status { get; set; }

        // Property [M][R][NO][NAR]
        public Guid CategoryId { get; set; }

        // Property [M][LNA]
        public List<ProductTag> Tags { get; set; } = [];

        public override string ToString() => Name;

    }

    // Category (category)
    public partial class Category
    {

        public Guid Id { get; set; }

        // Property [S][NO][PUO]
        public System.String Name { get; set; } = string.Empty;

        // Property [M][LNA]
        public List<Product> Products { get; set; } = [];

        public override string ToString() => Name;

    }

    // ProductTag (product_tag)
    public partial class ProductTag
    {

        public string ComposedId { get; set; }

        // Property [S][NO][PUO]
        public System.String Tag { get; set; } = string.Empty;

        public System.Int32 Position { get; set; } = default(System.Int32);

        // Property [M][R][NO][NAR]
        public Guid ProductId { get; set; }

        // Property [K]
        // key part 0: Product

        // key part 1: Tag

    }

}