from datetime import datetime, timedelta
from decimal import Decimal

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
            obj_dict[col] = Decimal( 0.000000 )

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

def create_summary_item(name, sku, quantity, used, width, height):
    obj = get_col("TNtlSummaryItem")

    obj["name"] = name
    obj["sku"] = sku
    obj["quantity"] = quantity
    obj["used"] = used
    obj["width"] = width
    obj["height"] = height
    obj["created_date"] = datetime.now().strftime("%Y-%m-%d %H:%M:%S")

    # Status ID
    obj["status_id"] = 12

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

_str, stmt, stmt2 = "", "", ""

summary_item = ""

detail = create_detail("Admin")

stmt = stmt_name("TNtlDetail", "Insert")
stmt2 = stmt_value(detail)
_str += exec_stmt(stmt, stmt2) + "\n"

_str += "SET @detail_id = (SELECT IDENT_CURRENT('TNtlDetail'));" + "\n"

summary_item = create_summary_item("Plastic Squeegee", "", Decimal(1.000000), Decimal(1.000000), Decimal(0.000000), Decimal(0.000000))

stmt = stmt_name("TNtlSummaryItem", "Insert")
stmt2 = stmt_value(summary_item)
_str += exec_stmt(stmt, stmt2) + "\n"

detail = create_detail("Admin")

stmt = stmt_name("TNtlDetail", "Insert")
stmt2 = stmt_value(detail)
_str += exec_stmt(stmt, stmt2) + "\n"

_str += "SET @detail_id = (SELECT IDENT_CURRENT('TNtlDetail'));" + "\n"

summary_item = create_summary_item("Plastic Scraper", "", Decimal(1.000000), Decimal(1.000000), Decimal(0.000000), Decimal(0.000000))

stmt = stmt_name("TNtlSummaryItem", "Insert")
stmt2 = stmt_value(summary_item)
_str += exec_stmt(stmt, stmt2) + "\n"

detail = create_detail("Admin")

stmt = stmt_name("TNtlDetail", "Insert")
stmt2 = stmt_value(detail)
_str += exec_stmt(stmt, stmt2) + "\n"

_str += "SET @detail_id = (SELECT IDENT_CURRENT('TNtlDetail'));" + "\n"

summary_item = create_summary_item("Paper Cutter", "", Decimal(1.000000), Decimal(1.000000), Decimal(0.000000), Decimal(0.000000))

stmt = stmt_name("TNtlSummaryItem", "Insert")
stmt2 = stmt_value(summary_item)
_str += exec_stmt(stmt, stmt2) + "\n"

print(_str)
