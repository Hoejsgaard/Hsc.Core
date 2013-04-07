using System;
using Hsc.Model.Knowledge;
using Hsc.ModelRepository;
using Hsc.Repository;
using ServiceStack.WebHost.Endpoints;
using Funq;

namespace Hsc.ServiceStack
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("ServiceStack REST", typeof(EntityTypeService).Assembly) { }

        public override void Configure(Container container)
        {
            EntityTypeRepository entityTypeRepository = new EntityTypeRepository();
            container.Register<IEntityTypeRepository>(entityTypeRepository);

            var restEntityTypeRepository = container.Resolve<ResetEntityTypesService>();
            restEntityTypeRepository.Post(null);

            Routes
                .Add<EntityType>("/EntityTypes", "POST,PUT")
                .Add<EntityType>("/EntityTypes/{Name}")
                .Add<EntityTypes>("/EntityTypes")
                .Add<EntityTypes>("/EntityTypes/filter/{Filter}");
        }
    }

    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
