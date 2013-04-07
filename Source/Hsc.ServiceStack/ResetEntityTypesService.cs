using Hsc.Model.Knowledge;
using Hsc.Repository;
using ServiceStack.ServiceInterface;

namespace Hsc.ServiceStack
{
    public class ResetEntityTypesService : RestServiceBase<ResetEntityTypes>
    {
        public IEntityTypeRepository _entityTypeRepository { get; set; }

        private void PopulateRepository()
        {
            var task = new EntityType("Task");
            task.Attributes.Add(new StringAttribute("Description"));
            task.Attributes.Add(new IntAttribute("Estimate"));

            var project = new EntityType("Project");
            project.Attributes.Add(new BoolAttribute("IsClosed"));
            project.Attributes.Add(new IntAttribute("Budget"));
            project.Attributes.Add(new EntityAttribute("MainTask", task));
            project.Attributes.Add(new EntityAttribute("SubProject", project));

            var employee = new EntityType("Employee");
            employee.Attributes.Add(new StringAttribute("Name"));
            employee.Attributes.Add(new BoolAttribute("IsMale"));

            _entityTypeRepository.Create(task);
            _entityTypeRepository.Create(project);
            _entityTypeRepository.Create(employee);
        }

        public override object OnPost(ResetEntityTypes request)
        {
            PopulateRepository();

            return new ResetEntityTypesResponse();
        }
    }
}