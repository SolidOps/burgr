// Reference template: TypeScript DTOs, written with LF line endings.
// Enum 
export enum ProductStatus {
  Draft = 0,
Published = 1,
Archived = 2
}
// Object [AG][EN][EXP]
export interface ProductDTO {

  id: string;

  // Property [S][NO][PUO]
  name: string;

  description: string;

  price: number;

  stock: number;

  weight: number;

  isFeatured: boolean;

  availableFrom: string;

  supplierReference: string;

  // Property [M][R][NO][NAR]
  categoryId: string;

}
export interface CategoryDTO {

  id: string;

  // Property [S][NO][PUO]
  name: string;

}
export interface ProductTagDTO {

  // Property [S][NO][PUO]
  tag: string;

  position: number;

  // Property [M][R][NO][NAR]
  productId: string;

}