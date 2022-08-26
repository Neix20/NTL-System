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
SELECT TOP 1 p.name AS platform,
(
    SELECT COUNT(*) AS today_runtime
    FROM dbo.TNtlSeleniumLog
    WHERE 1 = 1
    AND type = 2
    AND end_date >= '2022-08-19 00:00:00' AND end_date < '2022-08-19 23:59:59'
) AS today_runtime,
(
    SELECT COUNT(*)
    FROM dbo.TNtlOrder 
    WHERE 1=1
    AND created_date >= '2022-08-19 00:00:00' AND created_date < '2022-08-19 23:59:59'
) AS ntl_sale,
(
    SELECT COUNT(*)
    FROM dbo.TNtlOrder 
    WHERE 1=1
    AND created_date >= '2022-07-01' AND created_date <= '2022-07-31'
) AS lmd_sale,
(
    SELECT COUNT(*)
    FROM dbo.TNtlOrder 
    WHERE 1=1
    AND created_date >= '2022-08-01' AND created_date <= '2022-08-31'
) AS mtd_sale,
(
    SELECT COUNT(*)
    FROM dbo.TNtlOrder
    WHERE 1=1
    AND created_date >= '2022-08-11' AND created_date <= '2022-08-16'
) AS l5_sale,
sl.status,
sl.end_date AS last_runtime
FROM dbo.TNtlSeleniumLog sl
LEFT JOIN dbo.TNtlPlatform p
ON sl.platform_id = p.id
WHERE 1 = 1
AND sl.type = 2;

-- Platform, Status, Last Runtime
SELECT TOP 1 p.name AS platform,
sl.status,
sl.end_date AS last_runtime
FROM dbo.TNtlSeleniumLog sl
LEFT JOIN dbo.TNtlPlatform p
ON sl.platform_id = p.id
WHERE 1 = 1
AND sl.type = 2;

-- Today Runtime
SELECT COUNT(*) AS today_runtime
FROM dbo.TNtlSeleniumLog
WHERE 1 = 1
AND type = 2
AND end_date >= '2022-08-19 00:00:00' AND end_date < '2022-08-19 23:59:59'

-- NTL Sales
SELECT COUNT(*)
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-08-19 00:00:00' AND created_date < '2022-08-19 23:59:59'

-- LMD
SELECT COUNT(*)
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-07-01' AND created_date <= '2022-07-31'

-- MTD
SELECT COUNT(*)
FROM dbo.TNtlOrder 
WHERE 1=1
AND created_date >= '2022-08-01' AND created_date <= '2022-08-31'

-- L5
SELECT COUNT(*)
FROM dbo.TNtlOrder
WHERE 1=1
AND created_date >= '2022-08-11' AND created_date <= '2022-08-16'

-- Procedure
CREATE OR ALTER PROCEDURE dbo.NSP_TNtlOrderAutomation_View
(
    @td_start_dt AS DATETIME,
    @td_end_dt AS DATETIME,
    @lm_start_dt AS DATETIME,
    @lm_end_dt AS DATETIME,
    @tm_start_dt AS DATETIME,
    @tm_end_dt AS DATETIME,
    @l5_start_dt AS DATETIME,
    @l5_end_dt AS DATETIME
)
AS
BEGIN
    SELECT TOP 1 p.name AS platform,
    (
        SELECT COUNT(*) AS today_runtime
        FROM dbo.TNtlSeleniumLog
        WHERE 1 = 1
        AND type = 2
        AND end_date >= @td_start_dt AND end_date < @td_end_dt
    ) AS today_runtime,
    (
        SELECT COUNT(*)
        FROM dbo.TNtlOrder 
        WHERE 1=1
        AND created_date >= @td_start_dt AND created_date < @td_end_dt
    ) AS ntl_sale,
    (
        SELECT COUNT(*)
        FROM dbo.TNtlOrder 
        WHERE 1=1
        AND created_date >= @lm_start_dt AND created_date <= @lm_end_dt
    ) AS lmd_sale,
    (
        SELECT COUNT(*)
        FROM dbo.TNtlOrder 
        WHERE 1=1
        AND created_date >= @tm_start_dt AND created_date <= @tm_end_dt
    ) AS mtd_sale,
    (
        SELECT COUNT(*)
        FROM dbo.TNtlOrder
        WHERE 1=1
        AND created_date >= @l5_start_dt AND created_date <= @l5_end_dt
    ) AS l5_sale,
    sl.status,
    sl.end_date AS last_runtime
    FROM dbo.TNtlSeleniumLog sl
    LEFT JOIN dbo.TNtlPlatform p
    ON sl.platform_id = p.id
    WHERE 1 = 1
    AND sl.type = 2;
END

EXEC dbo.NSP_TNtlOrderAutomation_View
'2022-08-19 00:00:00', '2022-08-19 23:59:59',
'2022-07-01', '2022-07-31',
'2022-08-01', '2022-08-31',
'2022-08-11', '2022-08-16';

-- Postgres
SELECT COUNT(*) AS odoo_sales
FROM public.sale_order
WHERE 1 = 1
AND create_date >= '2022-08-19 00:00:00' AND create_date < '2022-08-19 23:59:59';

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


