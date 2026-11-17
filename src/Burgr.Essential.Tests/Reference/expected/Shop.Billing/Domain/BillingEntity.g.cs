// Reference template: one class per aggregate root and entity.
namespace Acme.Shop.Billing.Domain
{
    // Object [AG][EN]
    // Invoice (invoice)
    public partial class Invoice
    {

        public Guid Id { get; set; }

        // Property [S][NO][PUO]
        public System.String Number { get; set; } = string.Empty;

        public System.DateTime IssuedOn { get; set; } = default(System.DateTime);

        public System.Decimal Total { get; set; } = default(System.Decimal);

        // Property [M][LNA]
        public List<InvoiceLine> Lines { get; set; } = [];

        public override string ToString() => Number;

    }

    // InvoiceLine (invoice_line)
    public partial class InvoiceLine
    {

        public Guid Id { get; set; }

        // Property [S][NO][PUO]
        public System.String Label { get; set; } = string.Empty;

        public System.Decimal Amount { get; set; } = default(System.Decimal);

        // Property [M][R][NO][NAR]
        public Guid InvoiceId { get; set; }

    }

}