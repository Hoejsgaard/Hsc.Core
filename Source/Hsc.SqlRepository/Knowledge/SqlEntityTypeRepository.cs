﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Hsc.Model.Knowledge;

namespace Hsc.SqlRepository.Knowledge
{
    public class SqlEntityTypeRepository : ISqlEntityTypeRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ISqlAttributeTypeRepository _sqlAttributeTypeRepository;

        public SqlEntityTypeRepository(IConnectionProvider connectionProvider,
                                    ISqlAttributeTypeRepository sqlAttributeTypeRepository)
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
                    sqlCommand.CommandText = GetInsertKnowledgeQuery(entityType.Name);
                    var id = (int) sqlCommand.ExecuteScalar();
                    entityType.Id = id;
                }
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = GetCreateOperationTableQuery(entityType.Name);
                    sqlCommand.ExecuteNonQuery();
                }

                _sqlAttributeTypeRepository.Create(entityType.Attributes, entityType);
            }
        }

        public void Update(EntityType entityType)
        {
            throw new NotImplementedException();
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }

        public EntityType Read(string name)
        {
            var entityType = new EntityType();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("SELECT * FROM hsck.EntityTypes WHERE Name='{0}'", name);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
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

        public List<EntityType> ReadAll()
        {
            var entityTypes = new List<EntityType>();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM hsck.EntityTypes";
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
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

        public void CreateTable()
        {
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "CREATE TABLE hsck.EntityTypes " +
                                             "(Id int PRIMARY KEY CLUSTERED IDENTITY(1,1), " +
                                             "Name varchar(255) NOT NULL UNIQUE)";
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        private EntityType Get(int id)
        {
            var entityType = new EntityType();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("SELECT * FROM hsck.EntityTypes WHERE Id='{0}'", id);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
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

        public void Update(EntityType entityType, AttributeType attributeType)
        {
        }

        private string GetInsertKnowledgeQuery(string name)
        {
            return string.Format("INSERT INTO hsck.EntityTypes (Name) OUTPUT inserted.Id VALUES ('{0}')", name);
        }

        private string GetCreateOperationTableQuery(string name)
        {
            return string.Format("CREATE TABLE hsco.{0} (Id int PRIMARY KEY CLUSTERED IDENTITY(1,1))", name);
        }

        #endregion
    }
}