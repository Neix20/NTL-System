-- Run At MSSQL To Reset Everything
DELETE FROM dbo.TNtlJobBatch;
DELETE FROM dbo.TNtlJobOrder;
DELETE FROM dbo.TNtlJobOrderItem;

DELETE FROM dbo.TNtlSummaryItem;

DELETE FROM dbo.TNtlOrder;
DELETE FROM dbo.TNtlOrderItem;

DELETE FROM dbo.TNtlCustomer;
DELETE FROM dbo.TNtlCustomerChat;

DELETE FROM dbo.TNtlSeleniumLog;

-- Postgres Queries
SELECT a.name, a.sku, a.qty, b.name "uom" FROM (
	SELECT b.name "name", b.default_code "sku", SUM(c.quantity) "qty", b.uom_id
    FROM public.product_product a
    LEFT JOIN public.product_template b
    ON a.product_tmpl_id = b.id
    LEFT JOIN public.stock_quant c
    ON a.id = c.product_id
    WHERE 1 = 1 
    AND c.location_id = 8
    AND b.tracking IN ('serial', 'lot')
    GROUP BY b.id
    ORDER BY b.id
) a
LEFT JOIN public.uom_uom b
ON a.uom_id = b.id;

-- Warehouse Stock Item
DROP VIEW IF EXISTS VWarehouseStock;
CREATE OR REPLACE VIEW VWarehouseStock AS
    SELECT SUBSTRING(b.default_code, '([A-Za-z]+)\d+') AS sku, 
    CAST(SUBSTRING(b.default_code, '[A-Za-z]+(\d+)') AS INTEGER) AS size,  
    SUM(COALESCE(c.quantity, 0) - COALESCE(c.reserved_quantity, 0)) AS qty
    FROM public.product_template b
    LEFT JOIN public.product_product a
    ON a.product_tmpl_id = b.id
    LEFT JOIN public.stock_quant c
    ON a.id = c.product_id
    WHERE 1=1
    AND b.tracking IN ('serial', 'lot')
    AND (b.default_code LIKE 'FGST%' OR b.default_code LIKE 'WPST%' OR b.default_code LIKE 'RMST%')
    AND (c.location_id = 8 OR c.location_id is null)
    GROUP BY 1, 2
    ORDER BY 1, 2;

DROP VIEW IF EXISTS VWarehouseStockCT;
CREATE OR REPLACE VIEW VWarehouseStockCT AS
    SELECT *
    FROM CROSSTAB(
        'SELECT sku, ''W'' || Size AS size, qty
        FROM VWarehouseStock'
    )
AS ct (
    SKU TEXT,
    W50 NUMERIC,
    W60 NUMERIC,
    W70 NUMERIC,
    W80 NUMERIC,
    W90 NUMERIC,
    W150 NUMERIC
);

DROP VIEW IF EXISTS VWarehouseStockDesc;
CREATE OR REPLACE VIEW VWarehouseStockDesc AS
    SELECT SUBSTRING(default_code, 1, 2) AS model, 
    SUBSTRING(name, '.*-.*-(.*) \d+.*') AS description,
    SUBSTRING(default_code, '([A-Za-z]+)\d+') AS sku
    FROM public.product_template 
    WHERE 1=1
    AND default_code LIKE 'FGST%' OR default_code LIKE 'WPST%' OR default_code LIKE 'RMST%'
    GROUP BY 1, 2, 3
    ORDER BY 1;

SELECT a.model, a.description, b.*
FROM VWarehouseStockDesc a
LEFT JOIN VWarehouseStockCT b
ON a.sku = b.sku;

-- MSSQL Queries
SELECT COUNT(*) "Sales"
FROM dbo.TNtlOrder
WHERE 1=1
AND created_date >= '2022-07-01 00:00:00' AND created_date < '2022-07-02 00:00:00';

SELECT COUNT(*) "LMD"
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-07-01' AND created_date <= '2022-07-31';

SELECT COUNT(*) "MTD"
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-08-01' AND created_date <= '2022-08-31';

SELECT COUNT(*) "L5"
FROM dbo.TNtlOrder
WHERE 1=1
AND created_date >= '2022-08-11' AND created_date <= '2022-08-16';

SELECT COUNT(*) "Sales"
FROM dbo.TNtlOrder;

SELECT p.name "Platform", COUNT(*) "Sales",
(SELECT COUNT(*)
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-07-01' AND created_date <= '2022-07-31') "LMD",
(SELECT COUNT(*)
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-08-01' AND created_date <= '2022-08-31') "MTD",
(SELECT COUNT(*)
FROM dbo.TNtlOrder
WHERE 1=1
AND created_date >= '2022-08-11' AND created_date <= '2022-08-16') "L5"
FROM dbo.TNtlOrder o
LEFT JOIN dbo.TNtlCustomer c
ON o.customer_id = c.id
LEFT JOIN dbo.TNtlPlatform p
ON c.platform_id = p.id
GROUP BY p.name;

SELECT COUNT(*) "Total Runtime",
(
    SELECT TOP 1 end_date
    FROM dbo.TNtlSeleniumLog
    WHERE 1=1 
    AND type=2
    ORDER BY end_date DESC
) "Last Runtime"
FROM dbo.TNtlSeleniumLog
WHERE 1=1
AND type=2;

SELECT TOP 1 end_date
FROM dbo.TNtlSeleniumLog
WHERE type=2
ORDER BY end_date DESC;

CREATE OR ALTER VIEW VNtlDashboardProduct AS 
	SELECT SUBSTRING(p.name, CHARINDEX(']', p.name) + 2, LEN(p.name) - CHARINDEX(']', p.name) -1) "name", 
	CAST(SUBSTRING(p.SKU, LEN(p.SKU) - 2, 3) AS INT) "width",
	CAST(SUBSTRING(p.description, LEN(p.description) - 2, 3) AS INT) "length",
	si.quantity "quantity",
	p.sell_price "price"
	FROM dbo.TNtlProduct p
	LEFT JOIN dbo.TNtlStockItem si
	ON p.id = si.product_id;

SELECT 
	tab1.measurement AS measurement,
	SUM(case when tab1.name = 'Black Silver' then tab1.quantity else 0 end) AS black_silver,
	SUM(case when tab1.name = 'Deep Blue Silver' then tab1.quantity else 0 end) AS deep_blue_silver,
	SUM(case when tab1.name = 'Gold Silver' then tab1.quantity else 0 end) AS gold_silver,
	SUM(case when tab1.name = 'Green Silver' then tab1.quantity else 0 end) AS green_silver,
	SUM(case when tab1.name = 'Silver Light' then tab1.quantity else 0 end) AS silver_light,
	tab1.price AS price
FROM (
	SELECT name, CONCAT(width, ' x ', length) AS measurement, quantity, price
	FROM VNtlDashboardProduct
) tab1
GROUP BY tab1.measurement, tab1.price;

SELECT * FROM VNtlDashboardProduct;


SELECT DISTINCT name FROM VNtlDashboardProduct;


