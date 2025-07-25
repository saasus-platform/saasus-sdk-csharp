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
    /// UpdateListingStatusParam
    /// </summary>
    [DataContract(Name = "UpdateListingStatusParam")]
    public partial class UpdateListingStatusParam : IEquatable<UpdateListingStatusParam>, IValidatableObject
    {

        /// <summary>
        /// Gets or Sets ListingStatus
        /// </summary>
        [DataMember(Name = "listing_status", IsRequired = true, EmitDefaultValue = true)]
        public ListingStatus ListingStatus { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateListingStatusParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected UpdateListingStatusParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateListingStatusParam" /> class.
        /// </summary>
        /// <param name="listingStatus">listingStatus (required).</param>
        public UpdateListingStatusParam(ListingStatus listingStatus = default(ListingStatus))
        {
            this.ListingStatus = listingStatus;
        }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class UpdateListingStatusParam {\n");
            sb.Append("  ListingStatus: ").Append(ListingStatus).Append("\n");
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
            return this.Equals(input as UpdateListingStatusParam);
        }

        /// <summary>
        /// Returns true if UpdateListingStatusParam instances are equal
        /// </summary>
        /// <param name="input">Instance of UpdateListingStatusParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpdateListingStatusParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.ListingStatus == input.ListingStatus ||
                    this.ListingStatus.Equals(input.ListingStatus)
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
                hashCode = (hashCode * 59) + this.ListingStatus.GetHashCode();
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
