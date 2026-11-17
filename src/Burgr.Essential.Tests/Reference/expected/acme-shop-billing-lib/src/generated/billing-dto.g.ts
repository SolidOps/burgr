// Reference template: TypeScript DTOs, written with LF line endings.
// Object [AG][EN][EXP]
export interface InvoiceDTO {

  id: string;

  // Property [S][NO][PUO]
  number: string;

  issuedOn: string;

  total: number;

}
export interface InvoiceLineDTO {

  id: string;

  // Property [S][NO][PUO]
  label: string;

  amount: number;

  // Property [M][R][NO][NAR]
  invoiceId: string;

}