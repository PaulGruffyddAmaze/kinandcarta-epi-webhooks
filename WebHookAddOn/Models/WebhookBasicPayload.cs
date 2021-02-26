using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Models
{
    public class WebhookBasicPayload
    {
        public WebhookBasicPayload(IContent content, string eventType, DateTime eventTime)
        {
            ContentInfo = new ContentInfo(content);
            EventType = eventType;
            EventTime = eventTime;
        }
        public ContentInfo ContentInfo { get; set; }
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
    }
}