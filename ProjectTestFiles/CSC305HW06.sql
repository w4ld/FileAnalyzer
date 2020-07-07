USE TSQL;
GO

SELECT O.OrderId AS [Order ID], OD.ProductId as [Product ID], OD.unitprice AS [Unit Price], OD.qty AS [Quantity], 
	(OD.unitprice * OD.qty) AS [Line Total], -- linetotal product of unitprice and qty 
	SUM(OD.unitprice * OD.qty) OVER(PARTITION BY O.orderid ORDER BY OD.productid) as [Running Total] --runningtotal partition by orderid and order by productid
FROM Sales.Orders as O
JOIN Sales.OrderDetails AS OD ON O.orderid=OD.orderid --join to get orderdetails
WHERE O.orderid=10312	--only order 10312 for our report
