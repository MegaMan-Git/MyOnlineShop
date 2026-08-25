--( Products N--1 Categories) 
ALTER TABLE Products
ADD CONSTRAINT fk_Products_Categories 
FOREIGN KEY (CategoryId) REFERENCES Categories(Id)

GO
--( )