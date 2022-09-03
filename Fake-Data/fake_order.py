from datetime import datetime, timedelta
from decimal import Decimal
from faker import Faker
from faker.providers import DynamicProvider

import pyodbc
import math

# Global Variable
db_ssms_driver = "SQL Server Native Client 11.0"
db_ssms_host = "47.254.234.86"
db_ssms_name = "NTL" 
db_ssms_username = "NTL"
db_ssms_pwd = "ILoveVigtech88!"

conn = pyodbc.connect(
    'Driver={' + db_ssms_driver + '};'
    f'Server={db_ssms_host};'
    f'Database={db_ssms_name};'
    f'uid={db_ssms_username};'
    f'pwd={db_ssms_pwd}'
)

fake = Faker()

def get_product(sku):
    cursor = conn.cursor()

    cursor.execute(
        f"""
        SELECT *
        FROM dbo.TNtlProduct
        WHERE 1 = 1
        AND sku = '{sku}';
        """
    )

    obj_list = [{col[0]:val for col, val in zip(cursor.description, _obj)} for _obj in cursor]
    obj = obj_list[0]

    # Randomize Sell Price
    obj["sell_price"] = fake.pydecimal(3, 2, False, 50, 500)

    return obj

def get_status():
    cursor = conn.cursor()

    cursor.execute(
        f"""
        SELECT id 
        FROM dbo.TNtlStatus 
        WHERE 1=1 
        AND type='Order';        
        """
    )

    obj_list = [{col[0]:val for col, val in zip(cursor.description, _obj)} for _obj in cursor]
    return obj_list

def get_customer():
    cursor = conn.cursor()

    cursor.execute(
        f"""
        SELECT TOP 3 id 
        FROM dbo.TNtlCustomer;
        """
    )

    obj_list = [{col[0]:val for col, val in zip(cursor.description, _obj)} for _obj in cursor]
    return obj_list

# Get Random Products From SKU
skuLs = [ "FGSTSB060", "FGSTSB150030", "FGSTSL050", "FGSTSL050110" ];
productLs = [get_product(sku) for sku in skuLs]
statusLs = get_status()
customerLs = get_customer()

ntl_product_provider = DynamicProvider(
    provider_name = "ntl_product",
    elements = productLs
)

ntl_status_provider = DynamicProvider(
    provider_name = "ntl_status",
    elements = statusLs
)

ntl_customer_provider = DynamicProvider(
    provider_name = "ntl_customer",
    elements = customerLs
)

fake.add_provider(ntl_product_provider)
fake.add_provider(ntl_status_provider)
fake.add_provider(ntl_customer_provider)

def get_obj(stmt):
    cursor = conn.cursor()

    cursor.execute(stmt)

    obj_list = [{col[0]:val for col, val in zip(cursor.description, _obj)} for _obj in cursor]

    return obj_list

def get_col(tbl_name):
    cursor = conn.cursor()

    cursor.execute(
        f"""
        SELECT c.name, t.name
        FROM sys.columns c
        LEFT JOIN sys.types t
        ON c.system_type_id = t.system_type_id
        WHERE object_id = OBJECT_ID('dbo.{tbl_name}')
        """
    )

    obj_list = [_obj for _obj in cursor]

    obj_dict = {}

    for obj in obj_list:
        col, dtype = obj

        if dtype in ['varchar', 'datetime']:
            obj_dict[col] = ""
        elif dtype in ['int']:
            obj_dict[col] = 0
        else:
            obj_dict[col] = Decimal( 0.000001 )

    return obj_dict

def get_latest_id(tblName):
    cursor = conn.cursor()

    cursor.execute(
        f"""
        SELECT IDENT_CURRENT('{tblName}');
        """
    )

    obj_list = [{col[0]:val for col, val in zip(cursor.description, _obj)} for _obj in cursor]

    return obj_list[0]["id"]

def convert_str_to_datetime(dtStr):
    return datetime.strptime(dtStr, '%Y-%m-%d')

def create_order_item(product):
    obj = get_col("TNtlOrderItem")
    
    # Randomize
    # Quantity
    obj["quantity"] = fake.pydecimal(2,0, False, 1, 50)

    # Unit Price
    obj["unit_price"] = product["sell_price"]

    # Product ID
    obj["product_id"] = product["id"]

    # Uom ID
    obj["uom_id"] = product["uom_id"]

    # Name
    obj["name"] = product["name"]

    # SKU
    obj["sku"] = product["SKU"]

    # Sub Total Price
    obj["sub_total_price"] = Decimal(obj["quantity"]) * Decimal(obj["unit_price"]) 
    obj["sub_total_price"] = round(obj["sub_total_price"], 6) + Decimal(0.000001)

    # Total Price
    obj["total_price"] = obj["sub_total_price"] + Decimal(0.000001)

    # Order ID
    obj["order_id"] = "@order_id"

    # Detail_ID
    obj["detail_id"] = "@detail_id"
    return obj

def create_order():
    obj = get_col("TNtlOrder")
    
    startDt = convert_str_to_datetime("2022-08-26")
    endDt = startDt + timedelta(days = 7)

    # Randomize
    # Name
    obj["name"] = f"O{fake.pystr(6, 6)}"

    # Created Date
    obj["created_date"] = fake.date_between(startDt, endDt)

    # Code
    obj["code"] = obj["name"]

    # Sub Total Price
    obj["sub_total_price"] = Decimal(0.000001)

    # Total Price
    obj["total_price"] = Decimal(0.000001)

    # Status ID
    obj["status_id"] = fake.ntl_status()["id"]

    # Customer Id
    obj["customer_id"] = fake.ntl_customer()["id"]

    # Detail_ID
    obj["detail_id"] = "@detail_id"

    return obj

def create_detail(username):
    obj = get_col("TNtlDetail")

    obj["created_by"] = username
    obj["last_updated_by"] = username
    obj["created_date"] = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    obj["last_updated_date"] = datetime.now().strftime("%Y-%m-%d %H:%M:%S")

    return obj

def stmt_name(table_name, function):
    return f"NSP_{table_name}_{function}"

def stmt_elem_value(elem):
    _str = ""
    if isinstance(elem, Decimal) or isinstance(elem, float):
        elem = round(elem, 6)
        _str = f"{elem}"
    elif isinstance(elem, int):
        _str = f"{elem}"
    elif isinstance(elem, str) and '@' in elem:
        _str = f"{elem}"
    else:
        _str = f"'{elem}'"
    return _str


def stmt_value(obj):
    arr = list(obj.values())

    # Remove First Column
    arr = arr[1:]

    arr = [stmt_elem_value(elem) for elem in arr]

    return ", ".join(arr)

def exec_stmt(stmt1, stmt2):
    return f"EXEC {stmt1} {stmt2};"

def genDummyNtlOrder():
    _str = ""

    stmt, stmt2 = "", ""

    # Create Order
    order = create_order()
    
    detail = create_detail("Admin")

    stmt = stmt_name("TNtlDetail", "Insert")
    stmt2 = stmt_value(detail)
    
    _str += exec_stmt(stmt, stmt2) + "\n"
    _str += "SET @detail_id = (SELECT IDENT_CURRENT('TNtlDetail'));" + "\n"

    stmt = stmt_name("TNtlOrder", "Insert")
    stmt2 = stmt_value(order)

    _str += exec_stmt(stmt, stmt2) + "\n"
    _str += "\nSET @order_id = (SELECT IDENT_CURRENT('TNtlOrder'));\n" 

    sub_total_price = 0

    # Create Order Item
    for (ind, product) in enumerate(productLs):
        if ind >= 1 and fake.pybool():
            continue

        orderItem = create_order_item(product)
        sub_total_price += orderItem["total_price"]

        detail = create_detail("Admin")

        _str += "\n"
        stmt = stmt_name("TNtlDetail", "Insert")
        stmt2 = stmt_value(detail)
        
        _str += exec_stmt(stmt, stmt2) + "\n"
        _str += "SET @detail_id = (SELECT IDENT_CURRENT('TNtlDetail'));" + "\n"

        stmt = stmt_name("TNtlOrderItem", "Insert")
        stmt2 = stmt_value(orderItem)

        _str += exec_stmt(stmt, stmt2) + "\n"

    _str += "\n"

    order["sub_total_price"] = round(sub_total_price, 6)
    order["total_price"] = round(sub_total_price, 6)

    # Update Order Total Price
    stmt = stmt_name("TNtlOrder", "Update")
    stmt2 = f"@order_id, " + stmt_value(order)

    _str += exec_stmt(stmt, stmt2) + "\n"

    return _str + "\n"

numOfOrder = 300

print("DECLARE @detail_id INT;")
print("DECLARE @order_id INT;")
print()

for i in range(numOfOrder):
    stmt = genDummyNtlOrder()
    print(stmt, end="")
