using EPiServer.Core;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Models
{
    public class ContentInfo
    {
        public ContentInfo(IContent content)
        {
            ContentId = content.ContentLink.ID;
            ContentGuid = content.ContentGuid.ToString();
            ContentLanguage = (content as ILocalizable)?.Language?.ToString() ?? string.Empty;
            Name = content.Name;
            Url = UrlResolver.Current.GetUrl(content);
        }
        public int ContentId { get; set; }
        public string ContentGuid { get; set; }
        public string ContentLanguage { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}