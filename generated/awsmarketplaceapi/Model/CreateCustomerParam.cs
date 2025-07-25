/*
 * SaaSus AWS Marketplace API Schema
 *
 * SaaSus AWS Marketplace API Schema
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
using OpenAPIDateConverter = awsmarketplaceapi.Client.OpenAPIDateConverter;

namespace awsmarketplaceapi.Model
{
    /// <summary>
    /// CreateCustomerParam
    /// </summary>
    [DataContract(Name = "CreateCustomerParam")]
    public partial class CreateCustomerParam : IEquatable<CreateCustomerParam>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateCustomerParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerParam" /> class.
        /// </summary>
        /// <param name="tenantId">tenantId (required).</param>
        /// <param name="registrationToken">registrationToken (required).</param>
        public CreateCustomerParam(string tenantId = default(string), string registrationToken = default(string))
        {
            // to ensure "tenantId" is required (not null)
            if (tenantId == null)
            {
                throw new ArgumentNullException("tenantId is a required property for CreateCustomerParam and cannot be null");
            }
            this.TenantId = tenantId;
            // to ensure "registrationToken" is required (not null)
            if (registrationToken == null)
            {
                throw new ArgumentNullException("registrationToken is a required property for CreateCustomerParam and cannot be null");
            }
            this.RegistrationToken = registrationToken;
        }

        /// <summary>
        /// Gets or Sets TenantId
        /// </summary>
        /// <example>69e732d6-8ecc-45c4-c2eb-8438f7ffe775</example>
        [DataMember(Name = "tenant_id", IsRequired = true, EmitDefaultValue = true)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or Sets RegistrationToken
        /// </summary>
        /// <example>registration_token</example>
        [DataMember(Name = "registration_token", IsRequired = true, EmitDefaultValue = true)]
        public string RegistrationToken { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateCustomerParam {\n");
            sb.Append("  TenantId: ").Append(TenantId).Append("\n");
            sb.Append("  RegistrationToken: ").Append(RegistrationToken).Append("\n");
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
            return this.Equals(input as CreateCustomerParam);
        }

        /// <summary>
        /// Returns true if CreateCustomerParam instances are equal
        /// </summary>
        /// <param name="input">Instance of CreateCustomerParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CreateCustomerParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.TenantId == input.TenantId ||
                    (this.TenantId != null &&
                    this.TenantId.Equals(input.TenantId))
                ) && 
                (
                    this.RegistrationToken == input.RegistrationToken ||
                    (this.RegistrationToken != null &&
                    this.RegistrationToken.Equals(input.RegistrationToken))
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
                if (this.TenantId != null)
                {
                    hashCode = (hashCode * 59) + this.TenantId.GetHashCode();
                }
                if (this.RegistrationToken != null)
                {
                    hashCode = (hashCode * 59) + this.RegistrationToken.GetHashCode();
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
