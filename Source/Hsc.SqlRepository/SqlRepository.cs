﻿using System.Data.SqlClient;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;
using Hsc.Repository;
using Hsc.SqlRepository.Meta;

namespace Hsc.SqlRepository
{
    public class SqlRepository : ISqlRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ISqlEntityTypeRepository _entityTypeRepository;
        private readonly ISqlAttributeTypeRepository _attributeTypeRepository;
        private readonly IEntityAttributeTypeRepository _entityAttributeTypeRepository;
        private readonly IEntityRepository _entityRepository;

        public SqlRepository(IConnectionProvider connectionProvider, 
            ISqlEntityTypeRepository entityTypeRepository,
            ISqlAttributeTypeRepository attributeTypeRepository,
            IEntityAttributeTypeRepository entityAttributeTypeRepository,
            IEntityRepository entityRepository)
        {
            _connectionProvider = connectionProvider;
            _entityTypeRepository = entityTypeRepository;
            _attributeTypeRepository = attributeTypeRepository;
            _entityAttributeTypeRepository = entityAttributeTypeRepository;
            _entityRepository = entityRepository;
            EntityTypes = entityTypeRepository;
            Entities = entityRepository;
        }

        public IEntityRepository Entities { get; set; }
        public IEntityTypeRepository EntityTypes { get; set; }
        public void InitializeDatabase()
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                CreateMetaSchema(connection);
                CreateInstanceSchema(connection);
                _entityTypeRepository.CreateTable();
                _attributeTypeRepository.CreateTable();
                _entityAttributeTypeRepository.CreateTable();
            }
        }

        public enum Gender
        {
            Male, 
            Female
        }

        public void PopulateWithMockTypes()
        {
            //var gender = new EntityType("Gender");

            var person = new EntityType("Person");
            person.Attributes.Add(new AttributeType("Name", DataType.String256));
            person.Attributes.Add(new AttributeType("Age", DataType.Integer));
            person.Attributes.Add(new AttributeType("IsMale", DataType.Boolean));
            //person.Attributes.Add(new EntityAttributeType("Gender", gender));
            person.Attributes.Add(new EntityAttributeType("Spouse", person));
            
            _entityTypeRepository.Create(person);

        }

        public void PopulateWithMockData()
        {
            EntityType personType = _entityTypeRepository.Get("Person");

            var rune = new Entity(personType);
            rune.Attributes["Name"].Value = "Rune";
            rune.Attributes["Age"].Value = 34;
            rune.Attributes["IsMale"].Value = true;

            var nina = new Entity(personType);
            nina.Attributes["Name"].Value = "Nina";
            nina.Attributes["Age"].Value = 30;
            nina.Attributes["IsMale"].Value = false;

            rune.Attributes["Spouse"].Value = nina;
            nina.Attributes["Spouse"].Value = rune;

            _entityRepository.Add(rune);
            _entityRepository.Add(nina);
        }

        private void CreateMetaSchema(SqlConnection sqlConnection)
        {
            using (var sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = "IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'exm')" +
                                         "DROP SCHEMA exm";
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = "CREATE SCHEMA exm";
                sqlCommand.ExecuteNonQuery();
            }
        }

        private void CreateInstanceSchema(SqlConnection sqlConnection)
        {
            using (var sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = "IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'exi')" +
                                         "DROP SCHEMA exi";
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = "CREATE SCHEMA exi";
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}