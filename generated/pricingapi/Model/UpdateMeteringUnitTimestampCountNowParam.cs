/*
 * SaaSus Pricing API Schema
 *
 * SaaSus Pricing API Schema
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
using OpenAPIDateConverter = pricingapi.Client.OpenAPIDateConverter;

namespace pricingapi.Model
{
    /// <summary>
    /// UpdateMeteringUnitTimestampCountNowParam
    /// </summary>
    [DataContract(Name = "UpdateMeteringUnitTimestampCountNowParam")]
    public partial class UpdateMeteringUnitTimestampCountNowParam : IEquatable<UpdateMeteringUnitTimestampCountNowParam>, IValidatableObject
    {

        /// <summary>
        /// Gets or Sets Method
        /// </summary>
        [DataMember(Name = "method", IsRequired = true, EmitDefaultValue = true)]
        public UpdateMeteringUnitTimestampCountMethod Method { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMeteringUnitTimestampCountNowParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected UpdateMeteringUnitTimestampCountNowParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMeteringUnitTimestampCountNowParam" /> class.
        /// </summary>
        /// <param name="method">method (required).</param>
        /// <param name="count">Count (required).</param>
        public UpdateMeteringUnitTimestampCountNowParam(UpdateMeteringUnitTimestampCountMethod method = default(UpdateMeteringUnitTimestampCountMethod), int count = default(int))
        {
            this.Method = method;
            this.Count = count;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <value>Count</value>
        /// <example>10000</example>
        [DataMember(Name = "count", IsRequired = true, EmitDefaultValue = true)]
        public int Count { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class UpdateMeteringUnitTimestampCountNowParam {\n");
            sb.Append("  Method: ").Append(Method).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
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
            return this.Equals(input as UpdateMeteringUnitTimestampCountNowParam);
        }

        /// <summary>
        /// Returns true if UpdateMeteringUnitTimestampCountNowParam instances are equal
        /// </summary>
        /// <param name="input">Instance of UpdateMeteringUnitTimestampCountNowParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpdateMeteringUnitTimestampCountNowParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Method == input.Method ||
                    this.Method.Equals(input.Method)
                ) && 
                (
                    this.Count == input.Count ||
                    this.Count.Equals(input.Count)
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
                hashCode = (hashCode * 59) + this.Method.GetHashCode();
                hashCode = (hashCode * 59) + this.Count.GetHashCode();
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
