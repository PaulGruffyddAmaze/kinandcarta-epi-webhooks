using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Models
{
    public class WebhookPostModel
    {
        public WebhookPostModel()
        {
            ContentTypes = new int[0];
        }

        public WebhookPostModel(Webhook webhook)
        {
            Id = webhook.Id.ExternalId;
            Name = webhook.Name;
            Url = webhook.Url;
            ParentId = webhook.ParentId;
            ContentTypes = webhook.ContentTypes ?? new int[0];
            Events = webhook.EventTypes;
            var headerCount = webhook.Headers?.Count ?? 0;
            if (headerCount > 0)
            {
                var header = webhook.Headers.First();
                HeaderKey1 = header.Key;
                HeaderVal1 = header.Value;
            }
            if (headerCount > 1)
            {
                var header = webhook.Headers.Skip(1).First();
                HeaderKey2 = header.Key;
                HeaderVal2 = header.Value;
            }
            if (headerCount > 2)
            {
                var header = webhook.Headers.Skip(2).First();
                HeaderKey3 = header.Key;
                HeaderVal3 = header.Value;
            }
            if (headerCount > 3)
            {
                var header = webhook.Headers.Skip(3).First();
                HeaderKey4 = header.Key;
                HeaderVal4 = header.Value;
            }
            if (headerCount > 4)
            {
                var header = webhook.Headers.Skip(4).First();
                HeaderKey5 = header.Key;
                HeaderVal5 = header.Value;
            }
        }
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Parent Node")]
        public int ParentId { get; set; }

        [Display(Name = "Content Types")]
        public int[] ContentTypes { get; set; }

        [Display(Name = "Events")]
        public string[] Events { get; set; }

        public string HeaderKey1 { get; set; }
        public string HeaderVal1 { get; set; }
        public string HeaderKey2 { get; set; }
        public string HeaderVal2 { get; set; }
        public string HeaderKey3 { get; set; }
        public string HeaderVal3 { get; set; }
        public string HeaderKey4 { get; set; }
        public string HeaderVal4 { get; set; }
        public string HeaderKey5 { get; set; }
        public string HeaderVal5 { get; set; }
    }
}