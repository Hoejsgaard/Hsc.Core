using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;

namespace Hsc.SqlRepository.Instance
{
    public class EntityRepository : ISqlEntityRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public EntityRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        
        public int Create(Entity entity)
        {
            if (entity.Id != 0)
            {
                return entity.Id;
            }
            using (var connection = _connectionProvider.GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = CreateBaseAttributesQuery(entity);
                    var id = (int)command.ExecuteScalar();
                    entity.Id = id;

                    command.CommandText = CreateReferenceAttributesQuery(entity);
                    command.ExecuteNonQuery();
                }
            }
            return entity.Id;
        }

        private string CreateReferenceAttributesQuery(Entity entity)
        {
            return string.Format("UPDATE exi.{0} SET {1} WHERE Id={2}", entity.EntityType.Name, CreateReferenceKeyValues(entity), entity.Id);
        }

        private string CreateReferenceKeyValues(Entity entity)
        {
            var stringBuilder = new StringBuilder();
            foreach (var attribute in entity.Attributes)
            {
                if (attribute.AttributeType.DataType == DataType.Entity)
                {
                    var childEntity = (Entity)attribute.Value;
                    if (childEntity.Id == 0)
                    {
                        Create(childEntity);
                    }
                    stringBuilder.Append(string.Format("{0}={1}", attribute.Name, childEntity.Id));
                }
            }
            return stringBuilder.ToString();
        }

        private string CreateBaseAttributesQuery(Entity entity)
        {
            return string.Format("INSERT INTO exi.{0} ({1}) OUTPUT inserted.Id VALUES ({2})", 
                entity.EntityType.Name, 
                GetColumns(entity.Attributes), 
                GetValues(entity.Attributes));
        }
        
        private object GetValues(AttributeCollection attributes)
        {
            var stringBuilder = new StringBuilder();
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeType.DataType != DataType.Entity)
                {
                    stringBuilder.Append(string.Format("'{0}', ", attribute.Value));
                }
            }
            string query = stringBuilder.ToString();
            return query.Substring(0, query.Length - 2);            
        }

        private string GetColumns(AttributeCollection attributes)
        {
            var stringBuilder = new StringBuilder();
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeType.DataType != DataType.Entity)
                {
                    stringBuilder.Append(attribute.Name + ", ");
                }
            }
            string query = stringBuilder.ToString();
            return query.Substring(0, query.Length - 2);
        }

        public void Update(Entity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(EntityType entityType, int id)
        {
            throw new System.NotImplementedException();
        }

        public Entity Read(EntityType entityType, int id)
        {
            return Get(entityType, id, new List<Entity>());
        }

        public Entity Get(EntityType entityType, int id, List<Entity> loadingEntities)
        {
            Entity entity = GetLoadedEntity(loadingEntities, entityType, id);
            if (entity == null)
            {
                using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = string.Format("SELECT * FROM exi.{0} WHERE Id={1}", entityType.Name, id);
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            reader.Read();
                            entity = ReadEntity(reader, entityType, loadingEntities);
                        }
                    }
                }
            }
            return entity;
        }

        private Entity ReadEntity(SqlDataReader reader, EntityType entityType, List<Entity> loadingEntities)
        {
            List<string> fields = GetAttributes(reader);
            var entity = new Entity(entityType);
            foreach (string field in fields)
            {
                if (field == "Id")
                {
                    entity.Id = (int) reader["Id"];
                    loadingEntities.Add(entity);
                    continue;
                }
                if (entityType.Attributes[field].DataType == DataType.Entity)
                {
                    var entityAttributeType = (EntityAttributeType)entityType.Attributes[field];
                    Entity child = Get(entityAttributeType.OfType, (int) reader[field], loadingEntities);
                    entity.Attributes[field].Value = child;
                }
                else
                {
                    entity.Attributes[field].Value = reader[field];
                }
            }
            
            return entity;
        }

        private Entity GetLoadedEntity(List<Entity> loadedEntities, EntityType entityType, int id)
        {
            foreach (Entity loadedEntity in loadedEntities)
            {
                if (loadedEntity.EntityType == entityType && loadedEntity.Id == id)
                {
                    return loadedEntity;
                }
            }
            return null;
        }

        public List<Entity> ReadAll(EntityType entityType)
        {
            var entities = new List<Entity>();
            using (SqlConnection sqlConnection = _connectionProvider.GetConnection())
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = string.Format("SELECT * FROM exi.{0}", entityType.Name);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var loadedEntitites = new List<Entity>();
                        while (reader.Read())
                        {
                            Entity entity = ReadEntity(reader, entityType, loadedEntitites);
                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

        private List<string> GetAttributes(SqlDataReader reader)
        {
            var fields = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fields.Add(reader.GetName(i));
            }
            return fields;
        }
    }
}