using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Localization;
using KinAndCarta.Connect.Webhooks.Data;
using KinAndCarta.Connect.Webhooks.Extensions;
using KinAndCarta.Connect.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KinAndCarta.Connect.Webhooks.Controllers
{
    [Access(Roles = "Administrators,WebAdmins,WebhookAdmins")]
    public class WebhookAdminController : Controller
    {
        private IWebhookRepository _repo;
        private IContentTypeRepository _typeRepo;
        private IContentLoader _contentLoader;
        private LocalizationService _localizationService;
        public WebhookAdminController(IWebhookRepository repo, IContentTypeRepository typeRepo, IContentLoader contentLoader, LocalizationService localizationService)
        {
            _repo = repo;
            _typeRepo = typeRepo;
            _contentLoader = contentLoader;
            _localizationService = localizationService;
        }

        public ActionResult Index()
        {
            var webhooks = _repo.ListWebhooks();
            return View(webhooks);
        }

        [HttpGet]
        public JsonResult Tree(int parent = 0)
        {
            if (parent == 0) parent = ContentReference.RootPage.ID;
            return Json(_contentLoader.GetChildren<IContent>(new ContentReference(parent)).Where(x => !(x is BlockData) && !(x is MediaData)).Select(x => new { id = x.ContentLink.ID, name = x.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Guid? guid)
        {
            var vm = new EditWebhookViewModel();

            vm.Webhook = (guid == null) ? new WebhookPostModel() : new WebhookPostModel(_repo.GetWebhook(guid.Value));

            if (vm.Webhook.ParentId > 0)
            {
                var parentRef = new ContentReference(vm.Webhook.ParentId);
                if (!ContentReference.IsNullOrEmpty(parentRef))
                {
                    var parentContent = _contentLoader.Get<IContent>(parentRef);
                    if (parentContent != null)
                    {
                        vm.CurrentContentName = parentContent.Name;
                        vm.CurrentContentAncestors = _contentLoader.GetAncestors(parentRef).Select(x => x.ContentLink.ID).ToList();
                    }
                }
            }
            vm.ContentTypes = _typeRepo.List().Select(x => new SelectListItem { Text = x.DisplayName ?? x.Name, Value = x.ID.ToString() }).ToList();
            vm.EventTypes = new List<SelectListItem>();
            foreach (var eventType in _repo.GetEventTypes())
            {
                vm.EventTypes.Add(new SelectListItem { Value = eventType.Key, Text = _localizationService.GetString("/Webhooks/EventTypes/" + eventType.Key, eventType.Key) });
            }
            return View(vm);
        }

        public ActionResult Delete(Guid id)
        {
            _repo.DeleteWebhook(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Test(Guid id)
        {
            var errorResponse = new WebhookExecutionResponse { Success = false, Response = "Unable to execute webhook. This is probably caused by suitable content being unavailable below the selected parent." };
            var webhook = _repo.GetWebhook(id);
            if (webhook.ParentId > 0)
            {
                var parentRef = new ContentReference(webhook.ParentId);
                if (ContentReference.IsNullOrEmpty(parentRef)) return Json(errorResponse);
                var parentContent = _contentLoader.Get<IContent>(parentRef);
                if (parentContent == null) return Json(errorResponse);
                var testEventType = webhook.EventTypes?.Select(x => _repo.GetEventTypes().FirstOrDefault(y => y.Key.Equals(x)))?.FirstOrDefault(x => x != null) ?? Constants.PublishEvent;
                if (webhook.ContentTypes == null || !webhook.ContentTypes.Any() || webhook.ContentTypes.Contains(parentContent.ContentTypeID))
                {
                    return Json(webhook.Execute(parentContent, testEventType, _repo), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var childRef = _contentLoader.GetDescendents(parentRef).FirstOrDefault(x => webhook.ContentTypes.Contains(_contentLoader.Get<IContent>(x).ContentTypeID));
                    if (childRef != null)
                    {
                        return Json(webhook.Execute(_contentLoader.Get<IContent>(childRef), testEventType, _repo), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(errorResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Edit(WebhookPostModel webhook)
        {
            var originalWebhook = ((webhook?.Id ?? Guid.Empty) != Guid.Empty) ? _repo.GetWebhook(webhook.Id) : new Webhook();
            //update stuff
            originalWebhook.Name = webhook.Name;
            originalWebhook.Url = webhook.Url;
            originalWebhook.ContentTypes = webhook.ContentTypes;
            originalWebhook.ParentId = webhook.ParentId;
            originalWebhook.EventTypes = webhook.Events;
            originalWebhook.Headers = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(webhook.HeaderKey1) && !string.IsNullOrEmpty(webhook.HeaderVal1))
            {
                originalWebhook.Headers.Add(webhook.HeaderKey1, webhook.HeaderVal1);
            }
            if (!string.IsNullOrEmpty(webhook.HeaderKey2) && !string.IsNullOrEmpty(webhook.HeaderVal2))
            {
                originalWebhook.Headers.Add(webhook.HeaderKey2, webhook.HeaderVal2);
            }
            if (!string.IsNullOrEmpty(webhook.HeaderKey3) && !string.IsNullOrEmpty(webhook.HeaderVal3))
            {
                originalWebhook.Headers.Add(webhook.HeaderKey3, webhook.HeaderVal3);
            }
            if (!string.IsNullOrEmpty(webhook.HeaderKey4) && !string.IsNullOrEmpty(webhook.HeaderVal4))
            {
                originalWebhook.Headers.Add(webhook.HeaderKey4, webhook.HeaderVal4);
            }
            if (!string.IsNullOrEmpty(webhook.HeaderKey5) && !string.IsNullOrEmpty(webhook.HeaderVal5))
            {
                originalWebhook.Headers.Add(webhook.HeaderKey5, webhook.HeaderVal5);
            }
            _repo.SaveWebhook(originalWebhook);
            return RedirectToAction("Index");
        }
    }
}