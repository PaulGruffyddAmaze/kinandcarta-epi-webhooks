using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Models
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class Webhook : IDynamicData
    {
        public Identity Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Parent Node")]
        public int ParentId { get; set; }

        [Display(Name = "Content Types")]
        public int[] ContentTypes { get; set; }

        [Display(Name = "EventTypes")]
        public string[] EventTypes { get; set; }

        [Display(Name = "Custom Headers")]
        public Dictionary<string, string> Headers { get; set; }

        public DateTime LastExecuted { get; set; }
        public string LastResult { get; set; }
    }
}