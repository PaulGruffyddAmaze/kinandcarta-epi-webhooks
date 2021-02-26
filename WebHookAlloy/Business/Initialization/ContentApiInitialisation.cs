using System;
using System.Linq;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace WebHookAlloy.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ContentApiInitialisation : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            //Add initialization logic, this method is called once after CMS has been initialized
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.Configure<ContentApiConfiguration>(config =>
            {
                config.Default().SetMinimumRoles(string.Empty).SetFlattenPropertyModel(true);
            });
        }

    }
}