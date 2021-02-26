using EPiServer.Core;

namespace WebHookAlloy.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
