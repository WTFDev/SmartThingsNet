/* 
 * SmartThings API
 *
 * # Overview  This is the reference documentation for the SmartThings API.  The SmartThings API supports [REST](https://en.wikipedia.org/wiki/Representational_state_transfer), resources are protected with [OAuth 2.0 Bearer Tokens](https://tools.ietf.org/html/rfc6750#section-2.1), and all responses are sent as [JSON](http://www.json.org/).  # Authentication  All SmartThings resources are protected with [OAuth 2.0 Bearer Tokens](https://tools.ietf.org/html/rfc6750#section-2.1) sent on the request as an `Authorization: Bearer <TOKEN>` header, and operations require specific OAuth scopes that specify the exact permissions authorized by the user.  ## Token types  There are two types of tokens: SmartApp tokens, and personal access tokens.  SmartApp tokens are used to communicate between third-party integrations, or SmartApps, and the SmartThings API. When a SmartApp is called by the SmartThings platform, it is sent an authorization token that can be used to interact with the SmartThings API.  Personal access tokens are used to interact with the API for non-SmartApp use cases. They can be created and managed on the [personal access tokens page](https://account.smartthings.com/tokens).  ## OAuth2 scopes  Operations may be protected by one or more OAuth security schemes, which specify the required permissions. Each scope for a given scheme is required. If multiple schemes are specified (not common), you may use either scheme.  SmartApp token scopes are derived from the permissions requested by the SmartApp and granted by the end-user during installation. Personal access token scopes are associated with the specific permissions authorized when creating them.  Scopes are generally in the form `permission:entity-type:entity-id`.  **An `*` used for the `entity-id` specifies that the permission may be applied to all entities that the token type has access to, or may be replaced with a specific ID.**  For more information about authrization and permissions, please see the [Authorization and permissions guide](https://smartthings.developer.samsung.com/develop/guides/smartapps/auth-and-permissions.html).  <!- - ReDoc-Inject: <security-definitions> - ->  # Errors  The SmartThings API uses conventional HTTP response codes to indicate the success or failure of a request. In general, a `2XX` response code indicates success, a `4XX` response code indicates an error given the inputs for the request, and a `5XX` response code indicates a failure on the SmartThings platform.  API errors will contain a JSON response body with more information about the error:  ```json {   \"requestId\": \"031fec1a-f19f-470a-a7da-710569082846\"   \"error\": {     \"code\": \"ConstraintViolationError\",     \"message\": \"Validation errors occurred while process your request.\",     \"details\": [       { \"code\": \"PatternError\", \"target\": \"latitude\", \"message\": \"Invalid format.\" },       { \"code\": \"SizeError\", \"target\": \"name\", \"message\": \"Too small.\" },       { \"code\": \"SizeError\", \"target\": \"description\", \"message\": \"Too big.\" }     ]   } } ```  ## Error Response Body  The error response attributes are:  | Property | Type | Required | Description | | - -- | - -- | - -- | - -- | | requestId | String | No | A request identifier that can be used to correlate an error to additional logging on the SmartThings servers. | error | Error | **Yes** | The Error object, documented below.  ## Error Object  The Error object contains the following attributes:  | Property | Type | Required | Description | | - -- | - -- | - -- | - -- | | code | String | **Yes** | A SmartThings-defined error code that serves as a more specific indicator of the error than the HTTP error code specified in the response. See [SmartThings Error Codes](#section/Errors/SmartThings-Error-Codes) for more information. | message | String | **Yes** | A description of the error, intended to aid developers in debugging of error responses. | target | String | No | The target of the particular error. For example, it could be the name of the property that caused the error. | details | Error[] | No | An array of Error objects that typically represent distinct, related errors that occurred during the request. As an optional attribute, this may be null or an empty array.  ## Standard HTTP Error Codes  The following table lists the most common HTTP error response:  | Code | Name | Description | | - -- | - -- | - -- | | 400 | Bad Request | The client has issued an invalid request. This is commonly used to specify validation errors in a request payload. | 401 | Unauthorized | Authorization for the API is required, but the request has not been authenticated. | 403 | Forbidden | The request has been authenticated but does not have appropriate permissions, or a requested resource is not found. | 404 | Not Found | Specifies the requested path does not exist. | 406 | Not Acceptable | The client has requested a MIME type via the Accept header for a value not supported by the server. | 415 | Unsupported Media Type | The client has defined a contentType header that is not supported by the server. | 422 | Unprocessable Entity | The client has made a valid request, but the server cannot process it. This is often used for APIs for which certain limits have been exceeded. | 429 | Too Many Requests | The client has exceeded the number of requests allowed for a given time window. | 500 | Internal Server Error | An unexpected error on the SmartThings servers has occurred. These errors should be rare. | 501 | Not Implemented | The client request was valid and understood by the server, but the requested feature has yet to be implemented. These errors should be rare.  ## SmartThings Error Codes  SmartThings specifies several standard custom error codes. These provide more information than the standard HTTP error response codes. The following table lists the standard SmartThings error codes and their description:  | Code | Typical HTTP Status Codes | Description | | - -- | - -- | - -- | | PatternError | 400, 422 | The client has provided input that does not match the expected pattern. | ConstraintViolationError | 422 | The client has provided input that has violated one or more constraints. | NotNullError | 422 | The client has provided a null input for a field that is required to be non-null. | NullError | 422 | The client has provided an input for a field that is required to be null. | NotEmptyError | 422 | The client has provided an empty input for a field that is required to be non-empty. | SizeError | 400, 422 | The client has provided a value that does not meet size restrictions. | Unexpected Error | 500 | A non-recoverable error condition has occurred. Indicates a problem occurred on the SmartThings server that is no fault of the client. | UnprocessableEntityError | 422 | The client has sent a malformed request body. | TooManyRequestError | 429 | The client issued too many requests too quickly. | LimitError | 422 | The client has exceeded certain limits an API enforces. | UnsupportedOperationError | 400, 422 | The client has issued a request to a feature that currently isn't supported by the SmartThings platform. These should be rare.  ## Custom Error Codes  An API may define its own error codes where appropriate. These custom error codes are documented as part of that specific API's documentation.  # Warnings The SmartThings API issues warning messages via standard HTTP Warning headers. These messages do not represent a request failure, but provide additional information that the requester might want to act upon. For instance a warning will be issued if you are using an old API version.  # API Versions  The SmartThings API supports both path and header-based versioning. The following are equivalent:  - https://api.smartthings.com/v1/locations - https://api.smartthings.com/locations with header `Accept: application/vnd.smartthings+json;v=1`  Currently, only version 1 is available.  # Paging  Operations that return a list of objects return a paginated response. The `_links` object contains the items returned, and links to the next and previous result page, if applicable.  ```json {   \"items\": [     {       \"locationId\": \"6b3d1909-1e1c-43ec-adc2-5f941de4fbf9\",       \"name\": \"Home\"     },     {       \"locationId\": \"6b3d1909-1e1c-43ec-adc2-5f94d6g4fbf9\",       \"name\": \"Work\"     }     ....   ],   \"_links\": {     \"next\": {       \"href\": \"https://api.smartthings.com/v1/locations?page=3\"     },     \"previous\": {       \"href\": \"https://api.smartthings.com/v1/locations?page=1\"     }   } } ```  # Localization  Some SmartThings API's support localization. Specific information regarding localization endpoints are documented in the API itself. However, the following should apply to all endpoints that support localization.  ## Fallback Patterns  When making a request with the `Accept-Language` header, this fallback pattern is observed. * Response will be translated with exact locale tag. * If a translation does not exist for the requested language and region, the translation for the language will be returned. * If a translation does not exist for the language, English (en) will be returned. * Finally, an untranslated response will be returned in the absense of the above translations.  ## Accept-Language Header The format of the `Accept-Language` header follows what is defined in [RFC 7231, section 5.3.5](https://tools.ietf.org/html/rfc7231#section-5.3.5)  ## Content-Language The `Content-Language` header should be set on the response from the server to indicate which translation was given back to the client. The absense of the header indicates that the server did not recieve a request with the `Accept-Language` header set. 
 *
 * The version of the OpenAPI document: 1.0-PREVIEW
 * 
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using SmartThingsNet.Client;
using SmartThingsNet.Model;

namespace SmartThingsNet.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICapabilitiesApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Create a capability
        /// </summary>
        /// <remarks>
        /// Create a capability.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>Capability</returns>
        Capability CreateCapability (CreateCapabilityRequest capabilityRequest, string _namespace = default(string));

        /// <summary>
        /// Create a capability
        /// </summary>
        /// <remarks>
        /// Create a capability.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>ApiResponse of Capability</returns>
        ApiResponse<Capability> CreateCapabilityWithHttpInfo (CreateCapabilityRequest capabilityRequest, string _namespace = default(string));
        /// <summary>
        /// Create a capability presentation
        /// </summary>
        /// <remarks>
        /// Create a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>CapabilityPresentation</returns>
        CapabilityPresentation CreateCustomCapabilityPresentation (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest);

        /// <summary>
        /// Create a capability presentation
        /// </summary>
        /// <remarks>
        /// Create a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>ApiResponse of CapabilityPresentation</returns>
        ApiResponse<CapabilityPresentation> CreateCustomCapabilityPresentationWithHttpInfo (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest);
        /// <summary>
        /// Delete a capability by id and version.
        /// </summary>
        /// <remarks>
        /// Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Object</returns>
        Object DeleteCapability (string capabilityId, int capabilityVersion);

        /// <summary>
        /// Delete a capability by id and version.
        /// </summary>
        /// <remarks>
        /// Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> DeleteCapabilityWithHttpInfo (string capabilityId, int capabilityVersion);
        /// <summary>
        /// Get a capability by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability by ID.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Capability</returns>
        Capability GetCapability (string capabilityId, int capabilityVersion);

        /// <summary>
        /// Get a capability by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability by ID.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>ApiResponse of Capability</returns>
        ApiResponse<Capability> GetCapabilityWithHttpInfo (string capabilityId, int capabilityVersion);
        /// <summary>
        /// Get a capability presentation by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>CapabilityPresentation</returns>
        CapabilityPresentation GetCustomCapabilityPresentation (string capabilityId, int capabilityVersion);

        /// <summary>
        /// Get a capability presentation by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>ApiResponse of CapabilityPresentation</returns>
        ApiResponse<CapabilityPresentation> GetCustomCapabilityPresentationWithHttpInfo (string capabilityId, int capabilityVersion);
        /// <summary>
        /// All capabilities.
        /// </summary>
        /// <remarks>
        /// All capabilities.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>PagedCapabilities</returns>
        PagedCapabilities ListCapabilities ();

        /// <summary>
        /// All capabilities.
        /// </summary>
        /// <remarks>
        /// All capabilities.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>ApiResponse of PagedCapabilities</returns>
        ApiResponse<PagedCapabilities> ListCapabilitiesWithHttpInfo ();
        /// <summary>
        /// List capabilities by namespace.
        /// </summary>
        /// <remarks>
        /// Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>PagedCapabilities</returns>
        PagedCapabilities ListNamespacedCapabilities (string _namespace);

        /// <summary>
        /// List capabilities by namespace.
        /// </summary>
        /// <remarks>
        /// Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>ApiResponse of PagedCapabilities</returns>
        ApiResponse<PagedCapabilities> ListNamespacedCapabilitiesWithHttpInfo (string _namespace);
        /// <summary>
        /// Update a capability
        /// </summary>
        /// <remarks>
        /// Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>Capability</returns>
        Capability UpdateCapability (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest);

        /// <summary>
        /// Update a capability
        /// </summary>
        /// <remarks>
        /// Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>ApiResponse of Capability</returns>
        ApiResponse<Capability> UpdateCapabilityWithHttpInfo (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest);
        /// <summary>
        /// Update a capability presentation
        /// </summary>
        /// <remarks>
        /// Update a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>CapabilityPresentation</returns>
        CapabilityPresentation UpdateCustomCapabilityPresentation (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT);

        /// <summary>
        /// Update a capability presentation
        /// </summary>
        /// <remarks>
        /// Update a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>ApiResponse of CapabilityPresentation</returns>
        ApiResponse<CapabilityPresentation> UpdateCustomCapabilityPresentationWithHttpInfo (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICapabilitiesApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Create a capability
        /// </summary>
        /// <remarks>
        /// Create a capability.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>Task of Capability</returns>
        System.Threading.Tasks.Task<Capability> CreateCapabilityAsync (CreateCapabilityRequest capabilityRequest, string _namespace = default(string));

        /// <summary>
        /// Create a capability
        /// </summary>
        /// <remarks>
        /// Create a capability.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>Task of ApiResponse (Capability)</returns>
        System.Threading.Tasks.Task<ApiResponse<Capability>> CreateCapabilityAsyncWithHttpInfo (CreateCapabilityRequest capabilityRequest, string _namespace = default(string));
        /// <summary>
        /// Create a capability presentation
        /// </summary>
        /// <remarks>
        /// Create a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>Task of CapabilityPresentation</returns>
        System.Threading.Tasks.Task<CapabilityPresentation> CreateCustomCapabilityPresentationAsync (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest);

        /// <summary>
        /// Create a capability presentation
        /// </summary>
        /// <remarks>
        /// Create a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>Task of ApiResponse (CapabilityPresentation)</returns>
        System.Threading.Tasks.Task<ApiResponse<CapabilityPresentation>> CreateCustomCapabilityPresentationAsyncWithHttpInfo (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest);
        /// <summary>
        /// Delete a capability by id and version.
        /// </summary>
        /// <remarks>
        /// Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<Object> DeleteCapabilityAsync (string capabilityId, int capabilityVersion);

        /// <summary>
        /// Delete a capability by id and version.
        /// </summary>
        /// <remarks>
        /// Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteCapabilityAsyncWithHttpInfo (string capabilityId, int capabilityVersion);
        /// <summary>
        /// Get a capability by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability by ID.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of Capability</returns>
        System.Threading.Tasks.Task<Capability> GetCapabilityAsync (string capabilityId, int capabilityVersion);

        /// <summary>
        /// Get a capability by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability by ID.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of ApiResponse (Capability)</returns>
        System.Threading.Tasks.Task<ApiResponse<Capability>> GetCapabilityAsyncWithHttpInfo (string capabilityId, int capabilityVersion);
        /// <summary>
        /// Get a capability presentation by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of CapabilityPresentation</returns>
        System.Threading.Tasks.Task<CapabilityPresentation> GetCustomCapabilityPresentationAsync (string capabilityId, int capabilityVersion);

        /// <summary>
        /// Get a capability presentation by ID and Version.
        /// </summary>
        /// <remarks>
        /// Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of ApiResponse (CapabilityPresentation)</returns>
        System.Threading.Tasks.Task<ApiResponse<CapabilityPresentation>> GetCustomCapabilityPresentationAsyncWithHttpInfo (string capabilityId, int capabilityVersion);
        /// <summary>
        /// All capabilities.
        /// </summary>
        /// <remarks>
        /// All capabilities.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>Task of PagedCapabilities</returns>
        System.Threading.Tasks.Task<PagedCapabilities> ListCapabilitiesAsync ();

        /// <summary>
        /// All capabilities.
        /// </summary>
        /// <remarks>
        /// All capabilities.
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>Task of ApiResponse (PagedCapabilities)</returns>
        System.Threading.Tasks.Task<ApiResponse<PagedCapabilities>> ListCapabilitiesAsyncWithHttpInfo ();
        /// <summary>
        /// List capabilities by namespace.
        /// </summary>
        /// <remarks>
        /// Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>Task of PagedCapabilities</returns>
        System.Threading.Tasks.Task<PagedCapabilities> ListNamespacedCapabilitiesAsync (string _namespace);

        /// <summary>
        /// List capabilities by namespace.
        /// </summary>
        /// <remarks>
        /// Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>Task of ApiResponse (PagedCapabilities)</returns>
        System.Threading.Tasks.Task<ApiResponse<PagedCapabilities>> ListNamespacedCapabilitiesAsyncWithHttpInfo (string _namespace);
        /// <summary>
        /// Update a capability
        /// </summary>
        /// <remarks>
        /// Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>Task of Capability</returns>
        System.Threading.Tasks.Task<Capability> UpdateCapabilityAsync (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest);

        /// <summary>
        /// Update a capability
        /// </summary>
        /// <remarks>
        /// Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>Task of ApiResponse (Capability)</returns>
        System.Threading.Tasks.Task<ApiResponse<Capability>> UpdateCapabilityAsyncWithHttpInfo (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest);
        /// <summary>
        /// Update a capability presentation
        /// </summary>
        /// <remarks>
        /// Update a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>Task of CapabilityPresentation</returns>
        System.Threading.Tasks.Task<CapabilityPresentation> UpdateCustomCapabilityPresentationAsync (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT);

        /// <summary>
        /// Update a capability presentation
        /// </summary>
        /// <remarks>
        /// Update a capability presentation. Note: This API functionality is in BETA 
        /// </remarks>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>Task of ApiResponse (CapabilityPresentation)</returns>
        System.Threading.Tasks.Task<ApiResponse<CapabilityPresentation>> UpdateCustomCapabilityPresentationAsyncWithHttpInfo (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICapabilitiesApi : ICapabilitiesApiSync, ICapabilitiesApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class CapabilitiesApi : ICapabilitiesApi
    {
        private SmartThingsNet.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CapabilitiesApi() : this((string) null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CapabilitiesApi(String basePath)
        {
            this.Configuration = SmartThingsNet.Client.Configuration.MergeConfigurations(
                SmartThingsNet.Client.GlobalConfiguration.Instance,
                new SmartThingsNet.Client.Configuration { BasePath = basePath }
            );
            this.Client = new SmartThingsNet.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new SmartThingsNet.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = SmartThingsNet.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CapabilitiesApi(SmartThingsNet.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = SmartThingsNet.Client.Configuration.MergeConfigurations(
                SmartThingsNet.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.Client = new SmartThingsNet.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new SmartThingsNet.Client.ApiClient(this.Configuration.BasePath);
            ExceptionFactory = SmartThingsNet.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public CapabilitiesApi(SmartThingsNet.Client.ISynchronousClient client,SmartThingsNet.Client.IAsynchronousClient asyncClient, SmartThingsNet.Client.IReadableConfiguration configuration)
        {
            if(client == null) throw new ArgumentNullException("client");
            if(asyncClient == null) throw new ArgumentNullException("asyncClient");
            if(configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = SmartThingsNet.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public SmartThingsNet.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public SmartThingsNet.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public SmartThingsNet.Client.IReadableConfiguration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public SmartThingsNet.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Create a capability Create a capability.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>Capability</returns>
        public Capability CreateCapability (CreateCapabilityRequest capabilityRequest, string _namespace = default(string))
        {
             SmartThingsNet.Client.ApiResponse<Capability> localVarResponse = CreateCapabilityWithHttpInfo(capabilityRequest, _namespace);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create a capability Create a capability.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>ApiResponse of Capability</returns>
        public SmartThingsNet.Client.ApiResponse< Capability > CreateCapabilityWithHttpInfo (CreateCapabilityRequest capabilityRequest, string _namespace = default(string))
        {
            // verify the required parameter 'capabilityRequest' is set
            if (capabilityRequest == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityRequest' when calling CapabilitiesApi->CreateCapability");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            if (_namespace != null)
            {
                localVarRequestOptions.QueryParameters.Add(SmartThingsNet.Client.ClientUtils.ParameterToMultiMap("", "namespace", _namespace));
            }
            localVarRequestOptions.Data = capabilityRequest;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post< Capability >("/capabilities", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a capability Create a capability.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>Task of Capability</returns>
        public async System.Threading.Tasks.Task<Capability> CreateCapabilityAsync (CreateCapabilityRequest capabilityRequest, string _namespace = default(string))
        {
             SmartThingsNet.Client.ApiResponse<Capability> localVarResponse = await CreateCapabilityAsyncWithHttpInfo(capabilityRequest, _namespace);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create a capability Create a capability.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityRequest"></param>
        /// <param name="_namespace">The namespace of the capability (optional)</param>
        /// <returns>Task of ApiResponse (Capability)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<Capability>> CreateCapabilityAsyncWithHttpInfo (CreateCapabilityRequest capabilityRequest, string _namespace = default(string))
        {
            // verify the required parameter 'capabilityRequest' is set
            if (capabilityRequest == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityRequest' when calling CapabilitiesApi->CreateCapability");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            if (_namespace != null)
            {
                localVarRequestOptions.QueryParameters.Add(SmartThingsNet.Client.ClientUtils.ParameterToMultiMap("", "namespace", _namespace));
            }
            localVarRequestOptions.Data = capabilityRequest;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.PostAsync<Capability>("/capabilities", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a capability presentation Create a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>CapabilityPresentation</returns>
        public CapabilityPresentation CreateCustomCapabilityPresentation (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest)
        {
             SmartThingsNet.Client.ApiResponse<CapabilityPresentation> localVarResponse = CreateCustomCapabilityPresentationWithHttpInfo(capabilityId, capabilityVersion, createCustomCapabilityPresentationRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create a capability presentation Create a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>ApiResponse of CapabilityPresentation</returns>
        public SmartThingsNet.Client.ApiResponse< CapabilityPresentation > CreateCustomCapabilityPresentationWithHttpInfo (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->CreateCustomCapabilityPresentation");

            // verify the required parameter 'createCustomCapabilityPresentationRequest' is set
            if (createCustomCapabilityPresentationRequest == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'createCustomCapabilityPresentationRequest' when calling CapabilitiesApi->CreateCustomCapabilityPresentation");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter
            localVarRequestOptions.Data = createCustomCapabilityPresentationRequest;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post< CapabilityPresentation >("/capabilities/{capabilityId}/{capabilityVersion}/presentation", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateCustomCapabilityPresentation", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a capability presentation Create a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>Task of CapabilityPresentation</returns>
        public async System.Threading.Tasks.Task<CapabilityPresentation> CreateCustomCapabilityPresentationAsync (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest)
        {
             SmartThingsNet.Client.ApiResponse<CapabilityPresentation> localVarResponse = await CreateCustomCapabilityPresentationAsyncWithHttpInfo(capabilityId, capabilityVersion, createCustomCapabilityPresentationRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create a capability presentation Create a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="createCustomCapabilityPresentationRequest"></param>
        /// <returns>Task of ApiResponse (CapabilityPresentation)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<CapabilityPresentation>> CreateCustomCapabilityPresentationAsyncWithHttpInfo (string capabilityId, int capabilityVersion, CreateCapabilityPresentationRequest createCustomCapabilityPresentationRequest)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->CreateCustomCapabilityPresentation");

            // verify the required parameter 'createCustomCapabilityPresentationRequest' is set
            if (createCustomCapabilityPresentationRequest == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'createCustomCapabilityPresentationRequest' when calling CapabilitiesApi->CreateCustomCapabilityPresentation");


            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter
            localVarRequestOptions.Data = createCustomCapabilityPresentationRequest;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.PostAsync<CapabilityPresentation>("/capabilities/{capabilityId}/{capabilityVersion}/presentation", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("CreateCustomCapabilityPresentation", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete a capability by id and version. Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Object</returns>
        public Object DeleteCapability (string capabilityId, int capabilityVersion)
        {
             SmartThingsNet.Client.ApiResponse<Object> localVarResponse = DeleteCapabilityWithHttpInfo(capabilityId, capabilityVersion);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Delete a capability by id and version. Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>ApiResponse of Object</returns>
        public SmartThingsNet.Client.ApiResponse< Object > DeleteCapabilityWithHttpInfo (string capabilityId, int capabilityVersion)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->DeleteCapability");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete< Object >("/capabilities/{capabilityId}/{capabilityVersion}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete a capability by id and version. Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Object> DeleteCapabilityAsync (string capabilityId, int capabilityVersion)
        {
             SmartThingsNet.Client.ApiResponse<Object> localVarResponse = await DeleteCapabilityAsyncWithHttpInfo(capabilityId, capabilityVersion);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Delete a capability by id and version. Delete a capability id and version (only if it is in a \&quot;proposed\&quot; status).
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<Object>> DeleteCapabilityAsyncWithHttpInfo (string capabilityId, int capabilityVersion)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->DeleteCapability");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/capabilities/{capabilityId}/{capabilityVersion}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get a capability by ID and Version. Get a capability by ID.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Capability</returns>
        public Capability GetCapability (string capabilityId, int capabilityVersion)
        {
             SmartThingsNet.Client.ApiResponse<Capability> localVarResponse = GetCapabilityWithHttpInfo(capabilityId, capabilityVersion);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get a capability by ID and Version. Get a capability by ID.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>ApiResponse of Capability</returns>
        public SmartThingsNet.Client.ApiResponse< Capability > GetCapabilityWithHttpInfo (string capabilityId, int capabilityVersion)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->GetCapability");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get< Capability >("/capabilities/{capabilityId}/{capabilityVersion}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get a capability by ID and Version. Get a capability by ID.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of Capability</returns>
        public async System.Threading.Tasks.Task<Capability> GetCapabilityAsync (string capabilityId, int capabilityVersion)
        {
             SmartThingsNet.Client.ApiResponse<Capability> localVarResponse = await GetCapabilityAsyncWithHttpInfo(capabilityId, capabilityVersion);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get a capability by ID and Version. Get a capability by ID.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of ApiResponse (Capability)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<Capability>> GetCapabilityAsyncWithHttpInfo (string capabilityId, int capabilityVersion)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->GetCapability");


            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.GetAsync<Capability>("/capabilities/{capabilityId}/{capabilityVersion}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get a capability presentation by ID and Version. Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>CapabilityPresentation</returns>
        public CapabilityPresentation GetCustomCapabilityPresentation (string capabilityId, int capabilityVersion)
        {
             SmartThingsNet.Client.ApiResponse<CapabilityPresentation> localVarResponse = GetCustomCapabilityPresentationWithHttpInfo(capabilityId, capabilityVersion);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get a capability presentation by ID and Version. Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>ApiResponse of CapabilityPresentation</returns>
        public SmartThingsNet.Client.ApiResponse< CapabilityPresentation > GetCustomCapabilityPresentationWithHttpInfo (string capabilityId, int capabilityVersion)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->GetCustomCapabilityPresentation");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get< CapabilityPresentation >("/capabilities/{capabilityId}/{capabilityVersion}/presentation", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetCustomCapabilityPresentation", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get a capability presentation by ID and Version. Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of CapabilityPresentation</returns>
        public async System.Threading.Tasks.Task<CapabilityPresentation> GetCustomCapabilityPresentationAsync (string capabilityId, int capabilityVersion)
        {
             SmartThingsNet.Client.ApiResponse<CapabilityPresentation> localVarResponse = await GetCustomCapabilityPresentationAsyncWithHttpInfo(capabilityId, capabilityVersion);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get a capability presentation by ID and Version. Get a capability presentation by ID. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <returns>Task of ApiResponse (CapabilityPresentation)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<CapabilityPresentation>> GetCustomCapabilityPresentationAsyncWithHttpInfo (string capabilityId, int capabilityVersion)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->GetCustomCapabilityPresentation");


            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.GetAsync<CapabilityPresentation>("/capabilities/{capabilityId}/{capabilityVersion}/presentation", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetCustomCapabilityPresentation", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// All capabilities. All capabilities.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>PagedCapabilities</returns>
        public PagedCapabilities ListCapabilities ()
        {
             SmartThingsNet.Client.ApiResponse<PagedCapabilities> localVarResponse = ListCapabilitiesWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// All capabilities. All capabilities.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>ApiResponse of PagedCapabilities</returns>
        public SmartThingsNet.Client.ApiResponse< PagedCapabilities > ListCapabilitiesWithHttpInfo ()
        {
            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get< PagedCapabilities >("/capabilities", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListCapabilities", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// All capabilities. All capabilities.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>Task of PagedCapabilities</returns>
        public async System.Threading.Tasks.Task<PagedCapabilities> ListCapabilitiesAsync ()
        {
             SmartThingsNet.Client.ApiResponse<PagedCapabilities> localVarResponse = await ListCapabilitiesAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// All capabilities. All capabilities.
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <returns>Task of ApiResponse (PagedCapabilities)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<PagedCapabilities>> ListCapabilitiesAsyncWithHttpInfo ()
        {
            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.GetAsync<PagedCapabilities>("/capabilities", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListCapabilities", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// List capabilities by namespace. Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>PagedCapabilities</returns>
        public PagedCapabilities ListNamespacedCapabilities (string _namespace)
        {
             SmartThingsNet.Client.ApiResponse<PagedCapabilities> localVarResponse = ListNamespacedCapabilitiesWithHttpInfo(_namespace);
             return localVarResponse.Data;
        }

        /// <summary>
        /// List capabilities by namespace. Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>ApiResponse of PagedCapabilities</returns>
        public SmartThingsNet.Client.ApiResponse< PagedCapabilities > ListNamespacedCapabilitiesWithHttpInfo (string _namespace)
        {
            // verify the required parameter '_namespace' is set
            if (_namespace == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter '_namespace' when calling CapabilitiesApi->ListNamespacedCapabilities");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("namespace", SmartThingsNet.Client.ClientUtils.ParameterToString(_namespace)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get< PagedCapabilities >("/capabilities/namespaces/{namespace}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListNamespacedCapabilities", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// List capabilities by namespace. Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>Task of PagedCapabilities</returns>
        public async System.Threading.Tasks.Task<PagedCapabilities> ListNamespacedCapabilitiesAsync (string _namespace)
        {
             SmartThingsNet.Client.ApiResponse<PagedCapabilities> localVarResponse = await ListNamespacedCapabilitiesAsyncWithHttpInfo(_namespace);
             return localVarResponse.Data;

        }

        /// <summary>
        /// List capabilities by namespace. Namespaces are used to organize a user&#39;s capabilities and provide a way to uniquely identify them. A user can retrieve all of the capabilities under their assigned namespace by referencing it. The namespace is recognizable as the first part of a capabilityId in the format &#x60;namespace.capabilityName&#x60;. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="_namespace">The namespace of the capability</param>
        /// <returns>Task of ApiResponse (PagedCapabilities)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<PagedCapabilities>> ListNamespacedCapabilitiesAsyncWithHttpInfo (string _namespace)
        {
            // verify the required parameter '_namespace' is set
            if (_namespace == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter '_namespace' when calling CapabilitiesApi->ListNamespacedCapabilities");


            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("namespace", SmartThingsNet.Client.ClientUtils.ParameterToString(_namespace)); // path parameter

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.GetAsync<PagedCapabilities>("/capabilities/namespaces/{namespace}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("ListNamespacedCapabilities", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a capability Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>Capability</returns>
        public Capability UpdateCapability (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest)
        {
             SmartThingsNet.Client.ApiResponse<Capability> localVarResponse = UpdateCapabilityWithHttpInfo(capabilityId, capabilityVersion, capabilityRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a capability Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>ApiResponse of Capability</returns>
        public SmartThingsNet.Client.ApiResponse< Capability > UpdateCapabilityWithHttpInfo (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->UpdateCapability");

            // verify the required parameter 'capabilityRequest' is set
            if (capabilityRequest == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityRequest' when calling CapabilitiesApi->UpdateCapability");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter
            localVarRequestOptions.Data = capabilityRequest;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Put< Capability >("/capabilities/{capabilityId}/{capabilityVersion}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("UpdateCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a capability Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>Task of Capability</returns>
        public async System.Threading.Tasks.Task<Capability> UpdateCapabilityAsync (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest)
        {
             SmartThingsNet.Client.ApiResponse<Capability> localVarResponse = await UpdateCapabilityAsyncWithHttpInfo(capabilityId, capabilityVersion, capabilityRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a capability Update a capability. Capabilities with a \&quot;proposed\&quot; status can be updated at will. Capabilities with a \&quot;live\&quot;, \&quot;deprecated\&quot;, or \&quot;dead\&quot; status are immutable and can&#39;t be updated, for the time being. Capability name cannot be changed, as the id is derived from the name, so that should just be a new capability. 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequest"></param>
        /// <returns>Task of ApiResponse (Capability)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<Capability>> UpdateCapabilityAsyncWithHttpInfo (string capabilityId, int capabilityVersion, UpdateCapabilityRequest capabilityRequest)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->UpdateCapability");

            // verify the required parameter 'capabilityRequest' is set
            if (capabilityRequest == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityRequest' when calling CapabilitiesApi->UpdateCapability");


            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter
            localVarRequestOptions.Data = capabilityRequest;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.PutAsync<Capability>("/capabilities/{capabilityId}/{capabilityVersion}", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("UpdateCapability", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a capability presentation Update a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>CapabilityPresentation</returns>
        public CapabilityPresentation UpdateCustomCapabilityPresentation (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT)
        {
             SmartThingsNet.Client.ApiResponse<CapabilityPresentation> localVarResponse = UpdateCustomCapabilityPresentationWithHttpInfo(capabilityId, capabilityVersion, capabilityRequestBodyForPUT);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a capability presentation Update a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>ApiResponse of CapabilityPresentation</returns>
        public SmartThingsNet.Client.ApiResponse< CapabilityPresentation > UpdateCustomCapabilityPresentationWithHttpInfo (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->UpdateCustomCapabilityPresentation");

            // verify the required parameter 'capabilityRequestBodyForPUT' is set
            if (capabilityRequestBodyForPUT == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityRequestBodyForPUT' when calling CapabilitiesApi->UpdateCustomCapabilityPresentation");

            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = SmartThingsNet.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = SmartThingsNet.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter
            localVarRequestOptions.Data = capabilityRequestBodyForPUT;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Put< CapabilityPresentation >("/capabilities/{capabilityId}/{capabilityVersion}/presentation", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("UpdateCustomCapabilityPresentation", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a capability presentation Update a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>Task of CapabilityPresentation</returns>
        public async System.Threading.Tasks.Task<CapabilityPresentation> UpdateCustomCapabilityPresentationAsync (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT)
        {
             SmartThingsNet.Client.ApiResponse<CapabilityPresentation> localVarResponse = await UpdateCustomCapabilityPresentationAsyncWithHttpInfo(capabilityId, capabilityVersion, capabilityRequestBodyForPUT);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a capability presentation Update a capability presentation. Note: This API functionality is in BETA 
        /// </summary>
        /// <exception cref="SmartThingsNet.Client.ApiException">Thrown when fails to make API call</exception>

        /// <param name="capabilityId">The ID of the capability.</param>
        /// <param name="capabilityVersion">The version of the capability</param>
        /// <param name="capabilityRequestBodyForPUT"></param>
        /// <returns>Task of ApiResponse (CapabilityPresentation)</returns>
        public async System.Threading.Tasks.Task<SmartThingsNet.Client.ApiResponse<CapabilityPresentation>> UpdateCustomCapabilityPresentationAsyncWithHttpInfo (string capabilityId, int capabilityVersion, InlineObject capabilityRequestBodyForPUT)
        {
            // verify the required parameter 'capabilityId' is set
            if (capabilityId == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityId' when calling CapabilitiesApi->UpdateCustomCapabilityPresentation");

            // verify the required parameter 'capabilityRequestBodyForPUT' is set
            if (capabilityRequestBodyForPUT == null)
                throw new SmartThingsNet.Client.ApiException(400, "Missing required parameter 'capabilityRequestBodyForPUT' when calling CapabilitiesApi->UpdateCustomCapabilityPresentation");


            SmartThingsNet.Client.RequestOptions localVarRequestOptions = new SmartThingsNet.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.PathParameters.Add("capabilityId", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityId)); // path parameter
            localVarRequestOptions.PathParameters.Add("capabilityVersion", SmartThingsNet.Client.ClientUtils.ParameterToString(capabilityVersion)); // path parameter
            localVarRequestOptions.Data = capabilityRequestBodyForPUT;

            // authentication (Bearer) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.PutAsync<CapabilityPresentation>("/capabilities/{capabilityId}/{capabilityVersion}/presentation", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("UpdateCustomCapabilityPresentation", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

    }
}
