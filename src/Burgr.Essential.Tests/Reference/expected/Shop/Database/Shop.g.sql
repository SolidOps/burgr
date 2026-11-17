-- Reference template: one table per persisted object.
-- Object [AG][EN]
CREATE TABLE product (
-- Property [S][NO]
    Name VARCHAR(FIELDSIZE),
    Description VARCHAR(FIELDSIZE),
    InternalCode VARCHAR(FIELDSIZE),
    Price VARCHAR(FIELDSIZE),
    Stock VARCHAR(FIELDSIZE),
    Weight VARCHAR(FIELDSIZE),
    IsFeatured VARCHAR(FIELDSIZE),
    AvailableFrom VARCHAR(FIELDSIZE),
    SupplierReference VARCHAR(FIELDSIZE),
-- Property [M][R][NO][NAR]
    Category_id CHAR(36),
    PRIMARY KEY (id)
);
CREATE TABLE category (
-- Property [S][NO]
    Name VARCHAR(FIELDSIZE),
    PRIMARY KEY (id)
);
CREATE TABLE product_tag (
-- Property [S][NO]
    Tag VARCHAR(FIELDSIZE),
    Position VARCHAR(FIELDSIZE),
-- Property [M][R][NO][NAR]
    Product_id CHAR(36),
    PRIMARY KEY (product_id|tag)
);