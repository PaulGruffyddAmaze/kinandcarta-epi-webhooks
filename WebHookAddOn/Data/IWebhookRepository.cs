using EPiServer.Core;
using EPiServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinAndCarta.Connect.Webhooks.Models;
using EPiServer.Data.Dynamic;

namespace KinAndCarta.Connect.Webhooks.Data
{
    public interface IWebhookRepository
    {
        IEnumerable<Webhook> ListWebhooks();
        IEnumerable<Webhook> GetWebhooksForContentEvent(IContent content, EventType eventType);
        Webhook GetWebhook(Guid id, DynamicDataStore storeIn = null);
        void DeleteWebhook(Guid id);
        void SaveWebhook(Webhook webhook);
        void UpdateWebhook(Webhook webhook);
        void RegisterEventType(EventType key);
        IEnumerable<EventType> GetEventTypes();
        void RegisterPlaceholder(string token, string value);
        string ReplacePlaceholders(string originalString);
    }
}
