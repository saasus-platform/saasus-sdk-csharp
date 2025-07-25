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
    /// MeteringUnitDateCounts
    /// </summary>
    [DataContract(Name = "MeteringUnitDateCounts")]
    public partial class MeteringUnitDateCounts : IEquatable<MeteringUnitDateCounts>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeteringUnitDateCounts" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected MeteringUnitDateCounts() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MeteringUnitDateCounts" /> class.
        /// </summary>
        /// <param name="counts">counts (required).</param>
        public MeteringUnitDateCounts(List<MeteringUnitDateCount> counts = default(List<MeteringUnitDateCount>))
        {
            // to ensure "counts" is required (not null)
            if (counts == null)
            {
                throw new ArgumentNullException("counts is a required property for MeteringUnitDateCounts and cannot be null");
            }
            this.Counts = counts;
        }

        /// <summary>
        /// Gets or Sets Counts
        /// </summary>
        [DataMember(Name = "counts", IsRequired = true, EmitDefaultValue = true)]
        public List<MeteringUnitDateCount> Counts { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class MeteringUnitDateCounts {\n");
            sb.Append("  Counts: ").Append(Counts).Append("\n");
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
            return this.Equals(input as MeteringUnitDateCounts);
        }

        /// <summary>
        /// Returns true if MeteringUnitDateCounts instances are equal
        /// </summary>
        /// <param name="input">Instance of MeteringUnitDateCounts to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(MeteringUnitDateCounts input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Counts == input.Counts ||
                    this.Counts != null &&
                    input.Counts != null &&
                    this.Counts.SequenceEqual(input.Counts)
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
                if (this.Counts != null)
                {
                    hashCode = (hashCode * 59) + this.Counts.GetHashCode();
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
