using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Models
{
    public class WebhookDetailedPayload : WebhookBasicPayload
    {
        public WebhookDetailedPayload(IContent content, string eventType, DateTime eventTime) : base(content, eventType, eventTime)
        {
        }
        public IEnumerable<ContentInfo> ReferencedBy { get; set; }
        public ContentApiModel Content { get; set; }
        public Dictionary<string, object> ExtraData { get; set; }
    }
}