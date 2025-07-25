/*
 * SaaSus Auth API Schema
 *
 * Schema
 *
 * The version of the OpenAPI document: 1.0.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = authapi.Client.OpenAPIDateConverter;

namespace authapi.Model
{
    /// <summary>
    /// UpdateSoftwareTokenParam
    /// </summary>
    [DataContract(Name = "UpdateSoftwareTokenParam")]
    public partial class UpdateSoftwareTokenParam : IEquatable<UpdateSoftwareTokenParam>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSoftwareTokenParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected UpdateSoftwareTokenParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSoftwareTokenParam" /> class.
        /// </summary>
        /// <param name="accessToken">access token (required).</param>
        /// <param name="verificationCode">verification code (required).</param>
        public UpdateSoftwareTokenParam(string accessToken = default(string), string verificationCode = default(string))
        {
            // to ensure "accessToken" is required (not null)
            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken is a required property for UpdateSoftwareTokenParam and cannot be null");
            }
            this.AccessToken = accessToken;
            // to ensure "verificationCode" is required (not null)
            if (verificationCode == null)
            {
                throw new ArgumentNullException("verificationCode is a required property for UpdateSoftwareTokenParam and cannot be null");
            }
            this.VerificationCode = verificationCode;
        }

        /// <summary>
        /// access token
        /// </summary>
        /// <value>access token</value>
        [DataMember(Name = "access_token", IsRequired = true, EmitDefaultValue = true)]
        public string AccessToken { get; set; }

        /// <summary>
        /// verification code
        /// </summary>
        /// <value>verification code</value>
        [DataMember(Name = "verification_code", IsRequired = true, EmitDefaultValue = true)]
        public string VerificationCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class UpdateSoftwareTokenParam {\n");
            sb.Append("  AccessToken: ").Append(AccessToken).Append("\n");
            sb.Append("  VerificationCode: ").Append(VerificationCode).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as UpdateSoftwareTokenParam);
        }

        /// <summary>
        /// Returns true if UpdateSoftwareTokenParam instances are equal
        /// </summary>
        /// <param name="input">Instance of UpdateSoftwareTokenParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpdateSoftwareTokenParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.AccessToken == input.AccessToken ||
                    (this.AccessToken != null &&
                    this.AccessToken.Equals(input.AccessToken))
                ) && 
                (
                    this.VerificationCode == input.VerificationCode ||
                    (this.VerificationCode != null &&
                    this.VerificationCode.Equals(input.VerificationCode))
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
                if (this.AccessToken != null)
                {
                    hashCode = (hashCode * 59) + this.AccessToken.GetHashCode();
                }
                if (this.VerificationCode != null)
                {
                    hashCode = (hashCode * 59) + this.VerificationCode.GetHashCode();
                }
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
