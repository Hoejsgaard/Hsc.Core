using Hsc.Model.Knowledge;

namespace Hsc.SqlRepository
{
    public interface IDataTypeConverter
    {
        string ToSqlType(DataType type);
        DataType ToDataType(string sqlDataType);
    }
}