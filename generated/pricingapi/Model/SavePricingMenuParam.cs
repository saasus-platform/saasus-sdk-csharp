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
    /// SavePricingMenuParam
    /// </summary>
    [DataContract(Name = "SavePricingMenuParam")]
    public partial class SavePricingMenuParam : IEquatable<SavePricingMenuParam>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SavePricingMenuParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected SavePricingMenuParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SavePricingMenuParam" /> class.
        /// </summary>
        /// <param name="name">Menu name (required).</param>
        /// <param name="displayName">Menu display name (required).</param>
        /// <param name="description">Menu description (required).</param>
        /// <param name="unitIds">Unit IDs to add (required).</param>
        public SavePricingMenuParam(string name = default(string), string displayName = default(string), string description = default(string), List<string> unitIds = default(List<string>))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new ArgumentNullException("name is a required property for SavePricingMenuParam and cannot be null");
            }
            this.Name = name;
            // to ensure "displayName" is required (not null)
            if (displayName == null)
            {
                throw new ArgumentNullException("displayName is a required property for SavePricingMenuParam and cannot be null");
            }
            this.DisplayName = displayName;
            // to ensure "description" is required (not null)
            if (description == null)
            {
                throw new ArgumentNullException("description is a required property for SavePricingMenuParam and cannot be null");
            }
            this.Description = description;
            // to ensure "unitIds" is required (not null)
            if (unitIds == null)
            {
                throw new ArgumentNullException("unitIds is a required property for SavePricingMenuParam and cannot be null");
            }
            this.UnitIds = unitIds;
        }

        /// <summary>
        /// Menu name
        /// </summary>
        /// <value>Menu name</value>
        /// <example>Ten Users Menu</example>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// Menu display name
        /// </summary>
        /// <value>Menu display name</value>
        /// <example>10ユーザーメニュー</example>
        [DataMember(Name = "display_name", IsRequired = true, EmitDefaultValue = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Menu description
        /// </summary>
        /// <value>Menu description</value>
        /// <example>Can use 10 users.</example>
        [DataMember(Name = "description", IsRequired = true, EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// Unit IDs to add
        /// </summary>
        /// <value>Unit IDs to add</value>
        [DataMember(Name = "unit_ids", IsRequired = true, EmitDefaultValue = true)]
        public List<string> UnitIds { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SavePricingMenuParam {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  UnitIds: ").Append(UnitIds).Append("\n");
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
            return this.Equals(input as SavePricingMenuParam);
        }

        /// <summary>
        /// Returns true if SavePricingMenuParam instances are equal
        /// </summary>
        /// <param name="input">Instance of SavePricingMenuParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SavePricingMenuParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.DisplayName == input.DisplayName ||
                    (this.DisplayName != null &&
                    this.DisplayName.Equals(input.DisplayName))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.UnitIds == input.UnitIds ||
                    this.UnitIds != null &&
                    input.UnitIds != null &&
                    this.UnitIds.SequenceEqual(input.UnitIds)
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
                if (this.Name != null)
                {
                    hashCode = (hashCode * 59) + this.Name.GetHashCode();
                }
                if (this.DisplayName != null)
                {
                    hashCode = (hashCode * 59) + this.DisplayName.GetHashCode();
                }
                if (this.Description != null)
                {
                    hashCode = (hashCode * 59) + this.Description.GetHashCode();
                }
                if (this.UnitIds != null)
                {
                    hashCode = (hashCode * 59) + this.UnitIds.GetHashCode();
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
