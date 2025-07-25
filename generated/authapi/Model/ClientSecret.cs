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
    /// ClientSecret
    /// </summary>
    [DataContract(Name = "ClientSecret")]
    public partial class ClientSecret : IEquatable<ClientSecret>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecret" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ClientSecret() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecret" /> class.
        /// </summary>
        /// <param name="varClientSecret">Client Secret (required).</param>
        public ClientSecret(string varClientSecret = default(string))
        {
            // to ensure "varClientSecret" is required (not null)
            if (varClientSecret == null)
            {
                throw new ArgumentNullException("varClientSecret is a required property for ClientSecret and cannot be null");
            }
            this.VarClientSecret = varClientSecret;
        }

        /// <summary>
        /// Client Secret
        /// </summary>
        /// <value>Client Secret</value>
        /// <example>d9eH6xcJ5OU4tUff</example>
        [DataMember(Name = "client_secret", IsRequired = true, EmitDefaultValue = true)]
        public string VarClientSecret { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ClientSecret {\n");
            sb.Append("  VarClientSecret: ").Append(VarClientSecret).Append("\n");
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
            return this.Equals(input as ClientSecret);
        }

        /// <summary>
        /// Returns true if ClientSecret instances are equal
        /// </summary>
        /// <param name="input">Instance of ClientSecret to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ClientSecret input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.VarClientSecret == input.VarClientSecret ||
                    (this.VarClientSecret != null &&
                    this.VarClientSecret.Equals(input.VarClientSecret))
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
                if (this.VarClientSecret != null)
                {
                    hashCode = (hashCode * 59) + this.VarClientSecret.GetHashCode();
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
