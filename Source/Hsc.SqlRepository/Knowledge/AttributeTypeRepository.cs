using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Hsc.Model.Knowledge;
using Hsc.Repository;

namespace Hsc.SqlRepository.Knowledge
{
    public class AttributeTypeRepository : ISqlAttributeTypeRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly IDataTypeConverter _dataTypeConverter;
        private readonly IEntityAttributeTypeRepository _entityAttributeTypeRepository;

        public AttributeTypeRepository(IConnectionProvider connectionProvider, IDataTypeConverter dataTypeConverter,
                                       IEntityAttributeTypeRepository entityAttributeTypeRepository)
        {
            _connectionProvider = connectionProvider;
            _dataTypeConverter = dataTypeConverter;
            _entityAttributeTypeRepository = entityAttributeTypeRepository;
        }

        public AttributeType Read(int id)
        {
            throw new NotImplementedException();
        }

        public AttributeTypeCollection Read(EntityType entityType, IEntityTypeRepository entityTypeRepository)
        {
            var attributes = new AttributeTypeCollection();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("SELECT * FROM hsck.AttributeTypes WHERE EntityTypeId='{0}'", entityType.Id);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dataType = (DataType) Enum.Parse(typeof (DataType), (string) reader["DataType"]);
                            var attributeType = new AttributeType
                                                    {
                                                        Id = (int) reader["Id"],
                                                        Name = (string) reader["Name"],
                                                        DataType = dataType
                                                    };
                            attributes.Add(attributeType);
                        }
                    }
                }
            }
            attributes.Add(_entityAttributeTypeRepository.Read(entityType, entityTypeRepository));
            return attributes;
        }

        public void Create(AttributeType attributeType, EntityType onEntity)
        {
            if (attributeType.Name == "Id")
            {
                return;
            }
            if (attributeType is EntityAttributeType)
            {
                _entityAttributeTypeRepository.Create((EntityAttributeType) attributeType, onEntity);
                return;
            }
            CreateValueAttribute(attributeType, onEntity);
        }


        public List<AttributeType> ReadAll()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateTable()
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "CREATE TABLE hsck.AttributeTypes " +
                                             "(Id int PRIMARY KEY CLUSTERED IDENTITY(1,1), " +
                                             "Name varchar(255) NOT NULL," +
                                             "DataType varchar(255) NOT NULL," +
                                             "EntityTypeId int NOT NULL," +
                                             "FOREIGN KEY (EntityTypeId) references hsck.EntityTypes (Id))";
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void Create(AttributeTypeCollection attributeTypeCollection, EntityType entityType)
        {
            foreach (AttributeType attributeType in attributeTypeCollection)
            {
                Create(attributeType, entityType);
            }
        }

        private void CreateValueAttribute(AttributeType attributeType, EntityType onEntity)
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText =
                        string.Format("INSERT INTO hsck.AttributeTypes (Name, DataType, EntityTypeId) " +
                                      "VALUES ('{0}', '{1}', {2})",
                                      attributeType.Name,
                                      attributeType.DataType,
                                      onEntity.Id);
                    sqlCommand.ExecuteNonQuery();
                }
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("ALTER TABLE hsco.{0} ADD {1} {2}",
                                                           onEntity.Name,
                                                           attributeType.Name,
                                                           _dataTypeConverter.ToSqlType(attributeType.DataType));
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}