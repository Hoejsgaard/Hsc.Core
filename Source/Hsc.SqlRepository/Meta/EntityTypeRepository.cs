using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Hsc.Model.Knowledge;

namespace Hsc.SqlRepository.Meta
{
    public class EntityTypeRepository : ISqlEntityTypeRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ISqlAttributeTypeRepository _sqlAttributeTypeRepository;

        public EntityTypeRepository(IConnectionProvider connectionProvider, ISqlAttributeTypeRepository sqlAttributeTypeRepository)
        {
            _connectionProvider = connectionProvider;
            _sqlAttributeTypeRepository = sqlAttributeTypeRepository;
        }

        #region IEntityTypeRepository Members

        public void Create(EntityType entityType)
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = GetInsertMetaInstanceQuery(entityType.Name);
                    var id = (int)sqlCommand.ExecuteScalar();
                    entityType.Id = id;
                }
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = GetCreateInstanceTableQuery(entityType.Name);
                    sqlCommand.ExecuteNonQuery();
                }
                
                _sqlAttributeTypeRepository.Create(entityType.Attributes, entityType);
            }
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }

        private EntityType Get(int id)
        {
            var entityType = new EntityType();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("SELECT * FROM exm.EntityTypes WHERE Id='{0}'", id);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        reader.Read();
                        entityType.Id = (int) reader["id"];
                        entityType.Name = (string) reader["Name"];
                    }
                }
            }

            entityType.AddAttributes(_sqlAttributeTypeRepository.Read(entityType, this));
            entityType.Attributes.Add(new AttributeType("Id", DataType.Integer));

            return entityType;
        }

        public EntityType Get(string name)
        {
            var entityType = new EntityType();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("SELECT * FROM exm.EntityTypes WHERE Name='{0}'", name);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        reader.Read();
                        entityType.Id = (int) reader["id"];
                        entityType.Name = (string) reader["Name"];
                    }
                }
            }

            entityType.AddAttributes(_sqlAttributeTypeRepository.Read(entityType, this));

            return entityType;
        }

        public void Update(EntityType entityType, AttributeType attributeType)
        {
            
        }

        public List<EntityType> GetAll()
        {
            var entityTypes = new List<EntityType>();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM exm.EntityTypes";
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entityType = new EntityType
                                                 {
                                                     Id = (int) reader["id"], 
                                                     Name = (string) reader["Name"]
                                                 };
                            entityType.AddAttributes(_sqlAttributeTypeRepository.Read(entityType, this));
                            entityTypes.Add(entityType);
                        }
                    }
                }
            }

            return entityTypes;
        }

        private string GetInsertMetaInstanceQuery(string name)
        {
            return string.Format("INSERT INTO exm.EntityTypes (Name) OUTPUT inserted.Id VALUES ('{0}')", name);
        }

        private string GetCreateInstanceTableQuery(string name)
        {
            return string.Format("CREATE TABLE exi.{0} (Id int PRIMARY KEY CLUSTERED IDENTITY(1,1))", name);
        }

        public void CreateTable()
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "CREATE TABLE exm.EntityTypes " +
                                             "(Id int PRIMARY KEY CLUSTERED IDENTITY(1,1), " +
                                             "Name varchar(255) NOT NULL UNIQUE)";
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}