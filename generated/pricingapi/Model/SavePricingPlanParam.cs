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
    /// SavePricingPlanParam
    /// </summary>
    [DataContract(Name = "SavePricingPlanParam")]
    public partial class SavePricingPlanParam : IEquatable<SavePricingPlanParam>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SavePricingPlanParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected SavePricingPlanParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SavePricingPlanParam" /> class.
        /// </summary>
        /// <param name="name">Pricing plan name (required).</param>
        /// <param name="displayName">Pricing plan display name (required).</param>
        /// <param name="description">Pricing plan description (required).</param>
        /// <param name="menuIds">Menu ID to be added to the pricing plan (required).</param>
        public SavePricingPlanParam(string name = default(string), string displayName = default(string), string description = default(string), List<string> menuIds = default(List<string>))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new ArgumentNullException("name is a required property for SavePricingPlanParam and cannot be null");
            }
            this.Name = name;
            // to ensure "displayName" is required (not null)
            if (displayName == null)
            {
                throw new ArgumentNullException("displayName is a required property for SavePricingPlanParam and cannot be null");
            }
            this.DisplayName = displayName;
            // to ensure "description" is required (not null)
            if (description == null)
            {
                throw new ArgumentNullException("description is a required property for SavePricingPlanParam and cannot be null");
            }
            this.Description = description;
            // to ensure "menuIds" is required (not null)
            if (menuIds == null)
            {
                throw new ArgumentNullException("menuIds is a required property for SavePricingPlanParam and cannot be null");
            }
            this.MenuIds = menuIds;
        }

        /// <summary>
        /// Pricing plan name
        /// </summary>
        /// <value>Pricing plan name</value>
        /// <example>Standard Plan</example>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// Pricing plan display name
        /// </summary>
        /// <value>Pricing plan display name</value>
        /// <example>スタンダードプラン</example>
        [DataMember(Name = "display_name", IsRequired = true, EmitDefaultValue = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Pricing plan description
        /// </summary>
        /// <value>Pricing plan description</value>
        /// <example>Standard Plan for normal use</example>
        [DataMember(Name = "description", IsRequired = true, EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// Menu ID to be added to the pricing plan
        /// </summary>
        /// <value>Menu ID to be added to the pricing plan</value>
        [DataMember(Name = "menu_ids", IsRequired = true, EmitDefaultValue = true)]
        public List<string> MenuIds { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SavePricingPlanParam {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  MenuIds: ").Append(MenuIds).Append("\n");
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
            return this.Equals(input as SavePricingPlanParam);
        }

        /// <summary>
        /// Returns true if SavePricingPlanParam instances are equal
        /// </summary>
        /// <param name="input">Instance of SavePricingPlanParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SavePricingPlanParam input)
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
                    this.MenuIds == input.MenuIds ||
                    this.MenuIds != null &&
                    input.MenuIds != null &&
                    this.MenuIds.SequenceEqual(input.MenuIds)
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
                if (this.MenuIds != null)
                {
                    hashCode = (hashCode * 59) + this.MenuIds.GetHashCode();
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
