using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Models
{
    public class WebhookExecutionResponse
    {
        public bool Success { get; set; }
        public string Response { get; set; }
    }
}