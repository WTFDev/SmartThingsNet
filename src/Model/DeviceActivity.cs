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
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = SmartThingsNet.Client.OpenAPIDateConverter;

namespace SmartThingsNet.Model
{
    /// <summary>
    /// DeviceActivity
    /// </summary>
    [DataContract]
    public partial class DeviceActivity :  IEquatable<DeviceActivity>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceActivity" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected DeviceActivity() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceActivity" /> class.
        /// </summary>
        /// <param name="deviceId">Device ID.</param>
        /// <param name="deviceName">Device nick name.</param>
        /// <param name="locationId">Location ID.</param>
        /// <param name="locationName">Location name.</param>
        /// <param name="time">The IS0-8601 date time strings in UTC of the activity.</param>
        /// <param name="text">Translated human readable string (localized).</param>
        /// <param name="component">device component ID. Not nullable. (required).</param>
        /// <param name="componentLabel">device component label. Nullable..</param>
        /// <param name="capability">capability name.</param>
        /// <param name="attribute">attribute name.</param>
        /// <param name="value">attribute value.</param>
        /// <param name="unit">unit.</param>
        /// <param name="data">data.</param>
        /// <param name="translatedAttributeName">translated attribute name based on &#39;Accept-Language&#39; requested in header.</param>
        /// <param name="translatedAttributeValue">translated attribute value based on &#39;Accept-Language&#39; requested in header.</param>
        public DeviceActivity(string deviceId = default(string), string deviceName = default(string), string locationId = default(string), string locationName = default(string), DateTime time = default(DateTime), string text = default(string), string component = default(string), string componentLabel = default(string), string capability = default(string), string attribute = default(string), Object value = default(Object), string unit = default(string), Dictionary<string, Object> data = default(Dictionary<string, Object>), string translatedAttributeName = default(string), string translatedAttributeValue = default(string))
        {
            // to ensure "component" is required (not null)
            this.Component = component ?? throw new ArgumentNullException("component is a required property for DeviceActivity and cannot be null");
            this.DeviceId = deviceId;
            this.DeviceName = deviceName;
            this.LocationId = locationId;
            this.LocationName = locationName;
            this.Time = time;
            this.Text = text;
            this.ComponentLabel = componentLabel;
            this.Capability = capability;
            this.Attribute = attribute;
            this.Value = value;
            this.Unit = unit;
            this.Data = data;
            this.TranslatedAttributeName = translatedAttributeName;
            this.TranslatedAttributeValue = translatedAttributeValue;
        }
        
        /// <summary>
        /// Device ID
        /// </summary>
        /// <value>Device ID</value>
        [DataMember(Name="deviceId", EmitDefaultValue=false)]
        public string DeviceId { get; set; }

        /// <summary>
        /// Device nick name
        /// </summary>
        /// <value>Device nick name</value>
        [DataMember(Name="deviceName", EmitDefaultValue=false)]
        public string DeviceName { get; set; }

        /// <summary>
        /// Location ID
        /// </summary>
        /// <value>Location ID</value>
        [DataMember(Name="locationId", EmitDefaultValue=false)]
        public string LocationId { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        /// <value>Location name</value>
        [DataMember(Name="locationName", EmitDefaultValue=false)]
        public string LocationName { get; set; }

        /// <summary>
        /// The IS0-8601 date time strings in UTC of the activity
        /// </summary>
        /// <value>The IS0-8601 date time strings in UTC of the activity</value>
        [DataMember(Name="time", EmitDefaultValue=false)]
        public DateTime Time { get; set; }

        /// <summary>
        /// Translated human readable string (localized)
        /// </summary>
        /// <value>Translated human readable string (localized)</value>
        [DataMember(Name="text", EmitDefaultValue=false)]
        public string Text { get; set; }

        /// <summary>
        /// device component ID. Not nullable.
        /// </summary>
        /// <value>device component ID. Not nullable.</value>
        [DataMember(Name="component", EmitDefaultValue=false)]
        public string Component { get; set; }

        /// <summary>
        /// device component label. Nullable.
        /// </summary>
        /// <value>device component label. Nullable.</value>
        [DataMember(Name="componentLabel", EmitDefaultValue=false)]
        public string ComponentLabel { get; set; }

        /// <summary>
        /// capability name
        /// </summary>
        /// <value>capability name</value>
        [DataMember(Name="capability", EmitDefaultValue=false)]
        public string Capability { get; set; }

        /// <summary>
        /// attribute name
        /// </summary>
        /// <value>attribute name</value>
        [DataMember(Name="attribute", EmitDefaultValue=false)]
        public string Attribute { get; set; }

        /// <summary>
        /// attribute value
        /// </summary>
        /// <value>attribute value</value>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public Object Value { get; set; }

        /// <summary>
        /// Gets or Sets Unit
        /// </summary>
        [DataMember(Name="unit", EmitDefaultValue=false)]
        public string Unit { get; set; }

        /// <summary>
        /// Gets or Sets Data
        /// </summary>
        [DataMember(Name="data", EmitDefaultValue=false)]
        public Dictionary<string, Object> Data { get; set; }

        /// <summary>
        /// translated attribute name based on &#39;Accept-Language&#39; requested in header
        /// </summary>
        /// <value>translated attribute name based on &#39;Accept-Language&#39; requested in header</value>
        [DataMember(Name="translatedAttributeName", EmitDefaultValue=false)]
        public string TranslatedAttributeName { get; set; }

        /// <summary>
        /// translated attribute value based on &#39;Accept-Language&#39; requested in header
        /// </summary>
        /// <value>translated attribute value based on &#39;Accept-Language&#39; requested in header</value>
        [DataMember(Name="translatedAttributeValue", EmitDefaultValue=false)]
        public string TranslatedAttributeValue { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DeviceActivity {\n");
            sb.Append("  DeviceId: ").Append(DeviceId).Append("\n");
            sb.Append("  DeviceName: ").Append(DeviceName).Append("\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  LocationName: ").Append(LocationName).Append("\n");
            sb.Append("  Time: ").Append(Time).Append("\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("  Component: ").Append(Component).Append("\n");
            sb.Append("  ComponentLabel: ").Append(ComponentLabel).Append("\n");
            sb.Append("  Capability: ").Append(Capability).Append("\n");
            sb.Append("  Attribute: ").Append(Attribute).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  Unit: ").Append(Unit).Append("\n");
            sb.Append("  Data: ").Append(Data).Append("\n");
            sb.Append("  TranslatedAttributeName: ").Append(TranslatedAttributeName).Append("\n");
            sb.Append("  TranslatedAttributeValue: ").Append(TranslatedAttributeValue).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as DeviceActivity);
        }

        /// <summary>
        /// Returns true if DeviceActivity instances are equal
        /// </summary>
        /// <param name="input">Instance of DeviceActivity to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DeviceActivity input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.DeviceId == input.DeviceId ||
                    (this.DeviceId != null &&
                    this.DeviceId.Equals(input.DeviceId))
                ) && 
                (
                    this.DeviceName == input.DeviceName ||
                    (this.DeviceName != null &&
                    this.DeviceName.Equals(input.DeviceName))
                ) && 
                (
                    this.LocationId == input.LocationId ||
                    (this.LocationId != null &&
                    this.LocationId.Equals(input.LocationId))
                ) && 
                (
                    this.LocationName == input.LocationName ||
                    (this.LocationName != null &&
                    this.LocationName.Equals(input.LocationName))
                ) && 
                (
                    this.Time == input.Time ||
                    (this.Time != null &&
                    this.Time.Equals(input.Time))
                ) && 
                (
                    this.Text == input.Text ||
                    (this.Text != null &&
                    this.Text.Equals(input.Text))
                ) && 
                (
                    this.Component == input.Component ||
                    (this.Component != null &&
                    this.Component.Equals(input.Component))
                ) && 
                (
                    this.ComponentLabel == input.ComponentLabel ||
                    (this.ComponentLabel != null &&
                    this.ComponentLabel.Equals(input.ComponentLabel))
                ) && 
                (
                    this.Capability == input.Capability ||
                    (this.Capability != null &&
                    this.Capability.Equals(input.Capability))
                ) && 
                (
                    this.Attribute == input.Attribute ||
                    (this.Attribute != null &&
                    this.Attribute.Equals(input.Attribute))
                ) && 
                (
                    this.Value == input.Value ||
                    (this.Value != null &&
                    this.Value.Equals(input.Value))
                ) && 
                (
                    this.Unit == input.Unit ||
                    (this.Unit != null &&
                    this.Unit.Equals(input.Unit))
                ) && 
                (
                    this.Data == input.Data ||
                    this.Data != null &&
                    input.Data != null &&
                    this.Data.SequenceEqual(input.Data)
                ) && 
                (
                    this.TranslatedAttributeName == input.TranslatedAttributeName ||
                    (this.TranslatedAttributeName != null &&
                    this.TranslatedAttributeName.Equals(input.TranslatedAttributeName))
                ) && 
                (
                    this.TranslatedAttributeValue == input.TranslatedAttributeValue ||
                    (this.TranslatedAttributeValue != null &&
                    this.TranslatedAttributeValue.Equals(input.TranslatedAttributeValue))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.DeviceId != null)
                    hashCode = hashCode * 59 + this.DeviceId.GetHashCode();
                if (this.DeviceName != null)
                    hashCode = hashCode * 59 + this.DeviceName.GetHashCode();
                if (this.LocationId != null)
                    hashCode = hashCode * 59 + this.LocationId.GetHashCode();
                if (this.LocationName != null)
                    hashCode = hashCode * 59 + this.LocationName.GetHashCode();
                if (this.Time != null)
                    hashCode = hashCode * 59 + this.Time.GetHashCode();
                if (this.Text != null)
                    hashCode = hashCode * 59 + this.Text.GetHashCode();
                if (this.Component != null)
                    hashCode = hashCode * 59 + this.Component.GetHashCode();
                if (this.ComponentLabel != null)
                    hashCode = hashCode * 59 + this.ComponentLabel.GetHashCode();
                if (this.Capability != null)
                    hashCode = hashCode * 59 + this.Capability.GetHashCode();
                if (this.Attribute != null)
                    hashCode = hashCode * 59 + this.Attribute.GetHashCode();
                if (this.Value != null)
                    hashCode = hashCode * 59 + this.Value.GetHashCode();
                if (this.Unit != null)
                    hashCode = hashCode * 59 + this.Unit.GetHashCode();
                if (this.Data != null)
                    hashCode = hashCode * 59 + this.Data.GetHashCode();
                if (this.TranslatedAttributeName != null)
                    hashCode = hashCode * 59 + this.TranslatedAttributeName.GetHashCode();
                if (this.TranslatedAttributeValue != null)
                    hashCode = hashCode * 59 + this.TranslatedAttributeValue.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
