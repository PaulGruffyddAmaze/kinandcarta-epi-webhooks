using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KinAndCarta.Connect.Webhooks.Models
{
    public class EditWebhookViewModel
    {
        public WebhookPostModel Webhook { get; set; }
        public List<SelectListItem> ContentTypes { get; set; }
        public string CurrentContentName { get; set; }
        public List<int> CurrentContentAncestors { get; set; }
        public List<SelectListItem> EventTypes { get; set; }

    }
}