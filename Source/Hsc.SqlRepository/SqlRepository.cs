using System.Data.SqlClient;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;
using Hsc.Repository;
using Hsc.SqlRepository.Knowledge;

namespace Hsc.SqlRepository
{
    public class SqlRepository : ISqlRepository
    {
        private readonly ISqlAttributeTypeRepository _attributeTypeRepository;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IEntityAttributeTypeRepository _entityAttributeTypeRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly ISqlEntityTypeRepository _entityTypeRepository;

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
            using (SqlConnection connection = _connectionProvider.GetConnection())
            {
                CreateKnowledgeSchema(connection);
                CreateOperationSchema(connection);
                _entityTypeRepository.CreateTable();
                _attributeTypeRepository.CreateTable();
                _entityAttributeTypeRepository.CreateTable();
            }
        }

        public void PopulateWithMockTypes()
        {
            var person = new EntityType("Person");
            person.Attributes.Add(new AttributeType("Name", DataType.String256));
            person.Attributes.Add(new AttributeType("Age", DataType.Integer));
            person.Attributes.Add(new AttributeType("IsMale", DataType.Boolean));
            person.Attributes.Add(new EntityAttributeType("Spouse", person));

            _entityTypeRepository.Create(person);
        }

        public void PopulateWithMockData()
        {
            EntityType personType = _entityTypeRepository.Read("Person");

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

            _entityRepository.Create(rune);
            _entityRepository.Create(nina);
        }

        private void CreateKnowledgeSchema(SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = "IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'hsck')" +
                                         "DROP SCHEMA hsck";
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = "CREATE SCHEMA hsck";
                sqlCommand.ExecuteNonQuery();
            }
        }

        private void CreateOperationSchema(SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = "IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'hsco')" +
                                         "DROP SCHEMA hsco";
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = "CREATE SCHEMA hsco";
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}