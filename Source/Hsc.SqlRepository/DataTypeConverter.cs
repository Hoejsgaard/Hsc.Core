using System;
using System.Data;
using Hsc.Model.Knowledge;

namespace Hsc.SqlRepository
{
    public class DataTypeConverter : IDataTypeConverter
    {
        public string ToSqlType(DataType type)
        {
            switch (type)
            {
                case DataType.Boolean:
                    return "Bit";
                case DataType.String256:
                    return "Varchar(256)";
                case DataType.String2048:
                    return "Varchar(2048)";
                case DataType.String8000:
                    return "Varchar(8000)";
                case DataType.Integer:
                    return "Int";
                case DataType.Double:
                    return "Float";
                case DataType.Date:
                    return "DateTime";
                case DataType.DateTime:
                    return "DateTime";
                case DataType.Entity:
                    return "Int";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public DataType ToDataType(string sqlDataType)
        {
            if (sqlDataType == "Bit")
            {
                return DataType.Boolean;
            }
            if (sqlDataType == "Varchar(256)")
            {
                return DataType.String256;
            }
            if (sqlDataType == "Varchar(2480)")
            {
                return DataType.String2048;
            }
            if (sqlDataType == "Varchar(8000)")
            {
                return DataType.String8000;
            }
            if (sqlDataType == "Int")
            {
                return DataType.Integer;
            }
            if (sqlDataType == "Float")
            {
                return DataType.Integer;
            }
            if (sqlDataType == "DateTime")
            {
                return DataType.Integer;
            }
            if (sqlDataType == "Int")
            {
                return DataType.Integer;
            }
            throw new DataException();
        }
    }
}