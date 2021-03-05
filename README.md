# Kin + Carta Webhooks
This project adds webhook capabilities to Episerver sites, allowing events relating to IContent stored in Episerver to be handled by external systems. 

After installing the nuget package, a new CMS navigation item is added which takes users to the Webhooks UI. The UI allows for webhooks to be created, tested, updated and deleted as well as allowing for an at-a-glance view of the last result for each webhook.

When creating a webhook, tokens can be used within the URL and custom header fields to allow information such as keys or environment-specific values to be pulled from AppSettings in web.config. Tokens are in the format ${TokenName} where TokenName is a key in appsettings, the value of which will replace the token. In the event that the token name doesn't match a key, the token will be left as-is.

## Supported events
Out of the box, the add on supports 5 content events:
* Publish - Called once a page has been published
* Moving - Called before a page has been moved
* Move - Called after a page has been moved
* Recycled - Called after a page has been moved to the recycle bin
* Deleted - Called before a page has been deleted

Content save events have been intentionally ignored as they occur almost constantly while editing a page however it is possible to register your own events if this is required. 

When one of the supported events is triggered by a content event, any webhooks targetted to that event, content location and content type will be retrieved and a POST request will be made to the defined URL. The body of the post request contains information about the event fired, the content that triggered it and any other content items impacted by the event. The JSON serialiser from the content delivery API is used to serialise the content and so the structure of the serialised content data should match data retrieved from the content delivery API, including customisations such as flattening the structure.

## Project information
The solution includes the add-on itself plus an alloy site to which changes will be deployed on build, allowing for simpler debugging without the need to build and install the package.

The project is released under an MIT license and, as such, is provided as-is, without warranty of any kind, express or implied. For full details see the license file.