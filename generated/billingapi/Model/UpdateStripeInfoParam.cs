/*
 * SaaSus Billing API Schema
 *
 * SaaSus Billing API Schema
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
using OpenAPIDateConverter = billingapi.Client.OpenAPIDateConverter;

namespace billingapi.Model
{
    /// <summary>
    /// UpdateStripeInfoParam
    /// </summary>
    [DataContract(Name = "UpdateStripeInfoParam")]
    public partial class UpdateStripeInfoParam : IEquatable<UpdateStripeInfoParam>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateStripeInfoParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected UpdateStripeInfoParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateStripeInfoParam" /> class.
        /// </summary>
        /// <param name="secretKey">secret key (required).</param>
        public UpdateStripeInfoParam(string secretKey = default(string))
        {
            // to ensure "secretKey" is required (not null)
            if (secretKey == null)
            {
                throw new ArgumentNullException("secretKey is a required property for UpdateStripeInfoParam and cannot be null");
            }
            this.SecretKey = secretKey;
        }

        /// <summary>
        /// secret key
        /// </summary>
        /// <value>secret key</value>
        /// <example>sk_test_09l3shTSTKHYCzzZZsiLl2vA</example>
        [DataMember(Name = "secret_key", IsRequired = true, EmitDefaultValue = true)]
        public string SecretKey { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class UpdateStripeInfoParam {\n");
            sb.Append("  SecretKey: ").Append(SecretKey).Append("\n");
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
            return this.Equals(input as UpdateStripeInfoParam);
        }

        /// <summary>
        /// Returns true if UpdateStripeInfoParam instances are equal
        /// </summary>
        /// <param name="input">Instance of UpdateStripeInfoParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpdateStripeInfoParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.SecretKey == input.SecretKey ||
                    (this.SecretKey != null &&
                    this.SecretKey.Equals(input.SecretKey))
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
                if (this.SecretKey != null)
                {
                    hashCode = (hashCode * 59) + this.SecretKey.GetHashCode();
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
