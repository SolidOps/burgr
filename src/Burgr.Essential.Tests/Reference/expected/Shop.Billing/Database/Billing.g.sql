-- Reference template: one table per persisted object.
-- Object [AG][EN]
CREATE TABLE invoice (
-- Property [S][NO]
    Number VARCHAR(FIELDSIZE),
    IssuedOn VARCHAR(FIELDSIZE),
    Total VARCHAR(FIELDSIZE),
    PRIMARY KEY (id)
);
CREATE TABLE invoice_line (
-- Property [S][NO]
    Label VARCHAR(FIELDSIZE),
    Amount VARCHAR(FIELDSIZE),
-- Property [M][R][NO][NAR]
    Invoice_id CHAR(36),
    PRIMARY KEY (id)
);