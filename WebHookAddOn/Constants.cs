using KinAndCarta.Connect.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks
{
    public class Constants
    {
        //Events
        public static readonly EventType PublishEvent = new EventType { Key = "PUBLISH", ImpactsDescendants = false };
        public static readonly EventType MoveEvent = new EventType { Key = "MOVE", ImpactsDescendants = true };
        public static readonly EventType MovingEvent = new EventType { Key = "MOVING", ImpactsDescendants = true };
        public static readonly EventType RecycleEvent = new EventType { Key = "RECYCLE", ImpactsDescendants = true };
        public static readonly EventType DeleteEvent = new EventType { Key = "DELETE", ImpactsDescendants = true };

        //Config Keys
        public const string AutoRegisterAppSettingsKey = "WH:AutoRegisterAppSettings";
    }
}