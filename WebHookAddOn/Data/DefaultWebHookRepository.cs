using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data.Dynamic;
using KinAndCarta.Connect.Webhooks.Models;

namespace KinAndCarta.Connect.Webhooks.Data
{
    public class DefaultWebHookRepository : IWebhookRepository
    {
        private static DynamicDataStoreFactory _dataStoreFactory;
        private static IContentLoader _contentLoader;
        private IEnumerable<EventType> _eventTypes;
        private readonly Dictionary<string, string> _tokens;

        public DefaultWebHookRepository(DynamicDataStoreFactory dataStoreFactory, IContentLoader contentLoader)
        {
            _dataStoreFactory = dataStoreFactory;
            _contentLoader = contentLoader;
            _eventTypes = new List<EventType>();
            _tokens = new Dictionary<string, string>();
        }

        public void SaveWebhook(Webhook webhook)
        {
            using (var store = _dataStoreFactory.CreateStore(typeof(Webhook)))
            {
                store.Save(webhook);
            }
        }

        public void DeleteWebhook(Guid id)
        {
            using (var store = _dataStoreFactory.CreateStore(typeof(Webhook)))
            {
                var webhook = GetWebhook(id);
                store.Delete(webhook.Id);
            }
        }

        public Webhook GetWebhook(Guid id, DynamicDataStore storeIn = null)
        {
            var store = storeIn ?? _dataStoreFactory.CreateStore(typeof(Webhook));
            var item = store.Items<Webhook>().FirstOrDefault(x => x.Id.ExternalId.Equals(id));
            if (storeIn == null)
                store.Dispose();
            return item;
        }

        public IEnumerable<Webhook> GetWebhooksForContentEvent(IContent content, EventType eventType)
        {
            var ancestors = _contentLoader.GetAncestors(content.ContentLink).Select(x => x.ContentLink.ID);
            var lang = (content as ILocalizable)?.Language.Name ?? "-";
            IEnumerable<Webhook> hooks;
            using (var store = _dataStoreFactory.CreateStore(typeof(Webhook)))
            {
                hooks = store.Items<Webhook>().Where(x => ancestors.Contains(x.ParentId) || x.ParentId.Equals(content.ContentLink.ID)).ToList();
            }
            return hooks.Where(x => (x.ContentTypes == null
                                     || x.ContentTypes.Length.Equals(0)
                                     || x.ContentTypes.Contains(content.ContentTypeID)) && (x.EventTypes == null
                                                                                            || x.EventTypes.Length.Equals(0)
                                                                                            || x.EventTypes.Contains(eventType.Key)));
        }

        public IEnumerable<Webhook> ListWebhooks()
        {
            IEnumerable<Webhook> rtn;
            using (var store = _dataStoreFactory.CreateStore(typeof(Webhook)))
            {
                rtn = store.LoadAll<Webhook>();
            }
            return rtn;
        }

        public void UpdateWebhook(Webhook webhook)
        {
            using (var store = _dataStoreFactory.CreateStore(typeof(Webhook)))
            {
                var currentHook = store.Load<Webhook>(webhook.Id);
                currentHook.Name = webhook.Name;
                currentHook.ParentId = webhook.ParentId;
                currentHook.Url = webhook.Url;
                store.Save(currentHook);
            }
        }

        public void RegisterEventType(EventType key)
        {
            _eventTypes = _eventTypes.Append(key);
        }

        public IEnumerable<EventType> GetEventTypes()
        {
            return _eventTypes;
        }

        public void RegisterPlaceholder(string token, string value)
        {
            if (_tokens.ContainsKey(token))
            {
                _tokens[token] = value;
            }
            else
            {
                _tokens.Add(token, value);
            }
        }

        public string ReplacePlaceholders(string originalString)
        {
            var rtn = originalString;
            var tokens = Regex.Matches(rtn, "\\$\\{[a-zA-Z0-9-_:]+\\}");
            foreach (Match token in tokens)
            {
                var key = token.Value.Substring(2,token.Value.Length - 3);
                if (!_tokens.ContainsKey(key))
                {
                    continue;
                }
                rtn = rtn.Replace(token.Value, _tokens[key]);
            }
            return rtn;
        }

    }
}