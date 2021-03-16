using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using KinAndCarta.Connect.Webhooks.Data;
using KinAndCarta.Connect.Webhooks.Extensions;
using System.Configuration;

namespace KinAndCarta.Connect.Webhooks.Setup
{
    [InitializableModule]
    public class InitialiseWebhooks : IConfigurableModule
    {
        private IWebhookRepository _webhookRepo;
        private IContentLoader _contentLoader;
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += (o, e) =>
            {
                context.Services.AddSingleton<IWebhookRepository, DefaultWebHookRepository>();
            };
        }

        public void Initialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            _webhookRepo = context.Locate.Advanced.GetInstance<IWebhookRepository>();
            _contentLoader = context.Locate.Advanced.GetInstance<IContentLoader>();

            //Register basic content events
            _webhookRepo.RegisterEventType(Constants.PublishEvent);
            _webhookRepo.RegisterEventType(Constants.MovingEvent);
            _webhookRepo.RegisterEventType(Constants.MoveEvent);
            _webhookRepo.RegisterEventType(Constants.RecycleEvent);
            _webhookRepo.RegisterEventType(Constants.DeleteEvent);

            //Setup Event handlers
            contentEvents.PublishedContent += ContentPublished;
            contentEvents.MovingContent += ContentMoving;
            contentEvents.MovedContent += ContentMoved;
            contentEvents.DeletingContent += ContentDeleted;

            //Register Tokens
            if (ConfigurationManager.AppSettings[Constants.AutoRegisterAppSettingsKey]?.Equals("true", System.StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                {
                    _webhookRepo.RegisterPlaceholder(key, ConfigurationManager.AppSettings[key]);

                }
            }
        }

        private void ContentPublished(object sender, ContentEventArgs e)
        {
            e.Content.TriggerWebhooks(Constants.PublishEvent, _webhookRepo);
        }

        private void ContentMoving(object sender, ContentEventArgs e)
        {
            e.Content.TriggerWebhooks(Constants.MovingEvent, _webhookRepo);
        }

        private void ContentMoved(object sender, ContentEventArgs e)
        {
            var args = e as MoveContentEventArgs;
            
            e.Content.TriggerWebhooks(args.TargetLink.ID == ContentReference.WasteBasket.ID ? Constants.RecycleEvent : Constants.MoveEvent, _webhookRepo);
        }

        private void ContentDeleted(object sender, ContentEventArgs e)
        {
            var args = e as DeleteContentEventArgs;
            var content = e.Content;
            if (e.ContentLink == ContentReference.WasteBasket)
            {
                foreach (var item in args.DeletedDescendents)
                {
                    _contentLoader.Get<IContent>(item).TriggerWebhooks(Constants.DeleteEvent, _webhookRepo);
                }
                return;
            }
            if (content == null)
            {
                content = _contentLoader.Get<IContent>(e.ContentLink);
            }
            content.TriggerWebhooks(Constants.DeleteEvent, _webhookRepo);
        }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.PublishedContent -= ContentPublished;
            contentEvents.MovingContent -= ContentMoving;
            contentEvents.MovedContent -= ContentMoved;
            contentEvents.DeletingContent -= ContentDeleted;
        }
    }
}