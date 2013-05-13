using System.Data.SqlClient;
using Hsc.Model.Knowledge;
using Hsc.Repository;

namespace Hsc.SqlRepository.Knowledge
{
    public class EntityAttributeTypeRepository : IEntityAttributeTypeRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly IDataTypeConverter _dataTypeConverter;

        public EntityAttributeTypeRepository(IConnectionProvider connectionProvider,
                                             IDataTypeConverter dataTypeConverter)
        {
            _connectionProvider = connectionProvider;
            _dataTypeConverter = dataTypeConverter;
        }

        #region IEntityAttributeTypeRepository Members

        public void Create(AttributeTypeCollection attributeTypeCollection, EntityType onEntity)
        {
            foreach (AttributeType attributeType in attributeTypeCollection)
            {
                if (attributeType is EntityAttributeType)
                {
                    Create(attributeType as EntityAttributeType, onEntity);
                }
            }
        }

        public AttributeTypeCollection Read(EntityType entityType, IEntityTypeRepository entityTypeRepository)
        {
            var attributes = new AttributeTypeCollection();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText =
                        string.Format(
                            "SELECT * FROM hsck.EntityAttributeTypes WHERE ParentEntityTypeName='{0}'",
                            entityType.Name);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entityTypeName = (string) reader["ChildEntityTypeName"];
                            var attributeName = (string) reader["Name"];
                            EntityType ofType = entityTypeName == entityType.Name
                                                    ? entityType
                                                    : entityTypeRepository.Read(entityTypeName);
                            var entityAttributeType =
                                new EntityAttributeType
                                    {
                                        Id = (int) reader["Id"],
                                        Name = attributeName,
                                        OfType = ofType
                                    };
                            attributes.Add(entityAttributeType);
                        }
                    }
                }
            }
            return attributes;
        }

        public void Create(EntityAttributeType entityAttributeType, EntityType onEntity)
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText =
                        string.Format(
                            "INSERT INTO hsck.EntityAttributeTypes (Name, ChildEntityTypeName, ParentEntityTypeName)" +
                            "VALUES ('{0}', '{1}', '{2}')",
                            entityAttributeType.Name,
                            entityAttributeType.OfType.Name,
                            onEntity.Name);
                    sqlCommand.ExecuteNonQuery();
                }
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("ALTER TABLE hsco.{0} ADD {1} {2}",
                                                           onEntity.Name,
                                                           entityAttributeType.Name,
                                                           _dataTypeConverter.ToSqlType(DataType.Entity));
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        #endregion

        public void CreateTable()
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "CREATE TABLE hsck.EntityAttributeTypes " +
                                             "(Id int PRIMARY KEY CLUSTERED IDENTITY(1,1)," +
                                             "Name varchar(255) NOT NULL, " +
                                             "ChildEntityTypeName varchar(255) NOT NULL, " +
                                             "ParentEntityTypeName varchar(255) NOT NULL," +
                                             "FOREIGN KEY (ChildEntityTypeName) references hsck.EntityTypes (Name)," +
                                             "FOREIGN KEY (ParentEntityTypeName) references hsck.EntityTypes (Name))";
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}