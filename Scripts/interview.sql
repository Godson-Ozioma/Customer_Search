CREATE DATABASE Sales;
GO

USE Sales;
GO


CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Phone NVARCHAR(20),
    CustomerAddress NVARCHAR(255),
    PostCode NVARCHAR(20) NOT NULL,
    Country NVARCHAR(30)
);
CREATE INDEX IX_Customer_PostCode ON Customer(PostCode);
GO


CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    ProductDescription NVARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    ExpiryDate DATETIME NOT NULL,
    StockQuantity INT NOT NULL
);
CREATE INDEX IX_Product_ProductDescription ON Product(ProductDescription);
GO


CREATE TABLE [Order] (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);
GO


CREATE TABLE OrderProduct (
    OrderProductID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL, 
    FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
GO

CREATE PROCEDURE InsertIntoCustomer
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @CustomerAddress NVARCHAR(255),
    @Postcode NVARCHAR(20),
    @Country NVARCHAR(30)
AS
BEGIN
    INSERT INTO Customer (FirstName, LastName, Email, Phone, CustomerAddress, Postcode, Country)
    VALUES (@FirstName, @LastName, @Email, @Phone, @CustomerAddress, @Postcode, @Country);

    SELECT SCOPE_IDENTITY() AS NewCustomerID;
END;
GO


CREATE PROCEDURE InsertIntoProduct
    @ProductName NVARCHAR(100),
    @ProductDescription NVARCHAR(255),
    @Price DECIMAL(10, 2),
    @StockQuantity INT
AS
BEGIN
    INSERT INTO Product (ProductName, ProductDescription, Price, StockQuantity)
    VALUES (@ProductName, @ProductDescription, @Price, @StockQuantity);

    SELECT SCOPE_IDENTITY() AS NewProductID;
END;
GO


CREATE PROCEDURE InsertIntoOrder
    @CustomerID INT,
    @TotalAmount DECIMAL(10, 2)
AS
BEGIN
    INSERT INTO [Order] (CustomerID, TotalAmount)
    VALUES (@CustomerID, @TotalAmount);

    SELECT SCOPE_IDENTITY() AS NewOrderID;
END;
GO


CREATE PROCEDURE InsertIntoOrderProduct
    @OrderID INT,
    @ProductID INT,
    @Quantity INT,
    @Price DECIMAL(10, 2)
AS
BEGIN
    INSERT INTO OrderProduct (OrderID, ProductID, Quantity, Price)
    VALUES (@OrderID, @ProductID, @Quantity, @Price);

    SELECT SCOPE_IDENTITY() AS NewOrderProductID;
END;
GO


CREATE PROCEDURE SelectCustomer
    @CustomerID INT = NULL,
    @Postcode NVARCHAR(20) = NULL
AS
BEGIN
    SELECT * 
    FROM Customer
    WHERE (@CustomerID IS NULL OR CustomerID = @CustomerID)
      AND (@Postcode IS NULL OR Postcode = @Postcode);
END;
GO


CREATE PROCEDURE SelectProductByDescription
    @Description NVARCHAR(255)
AS
BEGIN
    SELECT * 
    FROM Product
    WHERE ProductDescription LIKE '%'+@Description+'%';
END;
GO