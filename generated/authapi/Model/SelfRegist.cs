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
    /// self sign-up permission
    /// </summary>
    [DataContract(Name = "SelfRegist")]
    public partial class SelfRegist : IEquatable<SelfRegist>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfRegist" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected SelfRegist() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfRegist" /> class.
        /// </summary>
        /// <param name="enable">enable (required).</param>
        public SelfRegist(bool enable = default(bool))
        {
            this.Enable = enable;
        }

        /// <summary>
        /// Gets or Sets Enable
        /// </summary>
        /// <example>true</example>
        [DataMember(Name = "enable", IsRequired = true, EmitDefaultValue = true)]
        public bool Enable { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SelfRegist {\n");
            sb.Append("  Enable: ").Append(Enable).Append("\n");
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
            return this.Equals(input as SelfRegist);
        }

        /// <summary>
        /// Returns true if SelfRegist instances are equal
        /// </summary>
        /// <param name="input">Instance of SelfRegist to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SelfRegist input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Enable == input.Enable ||
                    this.Enable.Equals(input.Enable)
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
                hashCode = (hashCode * 59) + this.Enable.GetHashCode();
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
