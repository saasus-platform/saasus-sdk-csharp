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
    /// BillingAddress
    /// </summary>
    [DataContract(Name = "BillingAddress")]
    public partial class BillingAddress : IEquatable<BillingAddress>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BillingAddress" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected BillingAddress() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BillingAddress" /> class.
        /// </summary>
        /// <param name="street">Street address, apartment or suite number. (required).</param>
        /// <param name="city">City, district, suburb, town, or village. (required).</param>
        /// <param name="state">State name or abbreviation. (required).</param>
        /// <param name="country">Country of the address using ISO 3166-1 alpha-2 code. (required).</param>
        /// <param name="additionalAddressInfo">Additional information about the address, such as a building name, floor, or department name..</param>
        /// <param name="postalCode">ZIP or postal code. (required).</param>
        public BillingAddress(string street = default(string), string city = default(string), string state = default(string), string country = default(string), string additionalAddressInfo = default(string), string postalCode = default(string))
        {
            // to ensure "street" is required (not null)
            if (street == null)
            {
                throw new ArgumentNullException("street is a required property for BillingAddress and cannot be null");
            }
            this.Street = street;
            // to ensure "city" is required (not null)
            if (city == null)
            {
                throw new ArgumentNullException("city is a required property for BillingAddress and cannot be null");
            }
            this.City = city;
            // to ensure "state" is required (not null)
            if (state == null)
            {
                throw new ArgumentNullException("state is a required property for BillingAddress and cannot be null");
            }
            this.State = state;
            // to ensure "country" is required (not null)
            if (country == null)
            {
                throw new ArgumentNullException("country is a required property for BillingAddress and cannot be null");
            }
            this.Country = country;
            // to ensure "postalCode" is required (not null)
            if (postalCode == null)
            {
                throw new ArgumentNullException("postalCode is a required property for BillingAddress and cannot be null");
            }
            this.PostalCode = postalCode;
            this.AdditionalAddressInfo = additionalAddressInfo;
        }

        /// <summary>
        /// Street address, apartment or suite number.
        /// </summary>
        /// <value>Street address, apartment or suite number.</value>
        /// <example>神宮前1-1-1</example>
        [DataMember(Name = "street", IsRequired = true, EmitDefaultValue = true)]
        public string Street { get; set; }

        /// <summary>
        /// City, district, suburb, town, or village.
        /// </summary>
        /// <value>City, district, suburb, town, or village.</value>
        /// <example>渋谷区</example>
        [DataMember(Name = "city", IsRequired = true, EmitDefaultValue = true)]
        public string City { get; set; }

        /// <summary>
        /// State name or abbreviation.
        /// </summary>
        /// <value>State name or abbreviation.</value>
        /// <example>東京都</example>
        [DataMember(Name = "state", IsRequired = true, EmitDefaultValue = true)]
        public string State { get; set; }

        /// <summary>
        /// Country of the address using ISO 3166-1 alpha-2 code.
        /// </summary>
        /// <value>Country of the address using ISO 3166-1 alpha-2 code.</value>
        /// <example>JP</example>
        [DataMember(Name = "country", IsRequired = true, EmitDefaultValue = true)]
        public string Country { get; set; }

        /// <summary>
        /// Additional information about the address, such as a building name, floor, or department name.
        /// </summary>
        /// <value>Additional information about the address, such as a building name, floor, or department name.</value>
        /// <example>オフィスビル3階</example>
        [DataMember(Name = "additional_address_info", EmitDefaultValue = false)]
        public string AdditionalAddressInfo { get; set; }

        /// <summary>
        /// ZIP or postal code.
        /// </summary>
        /// <value>ZIP or postal code.</value>
        /// <example>123-4567</example>
        [DataMember(Name = "postal_code", IsRequired = true, EmitDefaultValue = true)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class BillingAddress {\n");
            sb.Append("  Street: ").Append(Street).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  Country: ").Append(Country).Append("\n");
            sb.Append("  AdditionalAddressInfo: ").Append(AdditionalAddressInfo).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
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
            return this.Equals(input as BillingAddress);
        }

        /// <summary>
        /// Returns true if BillingAddress instances are equal
        /// </summary>
        /// <param name="input">Instance of BillingAddress to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BillingAddress input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Street == input.Street ||
                    (this.Street != null &&
                    this.Street.Equals(input.Street))
                ) && 
                (
                    this.City == input.City ||
                    (this.City != null &&
                    this.City.Equals(input.City))
                ) && 
                (
                    this.State == input.State ||
                    (this.State != null &&
                    this.State.Equals(input.State))
                ) && 
                (
                    this.Country == input.Country ||
                    (this.Country != null &&
                    this.Country.Equals(input.Country))
                ) && 
                (
                    this.AdditionalAddressInfo == input.AdditionalAddressInfo ||
                    (this.AdditionalAddressInfo != null &&
                    this.AdditionalAddressInfo.Equals(input.AdditionalAddressInfo))
                ) && 
                (
                    this.PostalCode == input.PostalCode ||
                    (this.PostalCode != null &&
                    this.PostalCode.Equals(input.PostalCode))
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
                if (this.Street != null)
                {
                    hashCode = (hashCode * 59) + this.Street.GetHashCode();
                }
                if (this.City != null)
                {
                    hashCode = (hashCode * 59) + this.City.GetHashCode();
                }
                if (this.State != null)
                {
                    hashCode = (hashCode * 59) + this.State.GetHashCode();
                }
                if (this.Country != null)
                {
                    hashCode = (hashCode * 59) + this.Country.GetHashCode();
                }
                if (this.AdditionalAddressInfo != null)
                {
                    hashCode = (hashCode * 59) + this.AdditionalAddressInfo.GetHashCode();
                }
                if (this.PostalCode != null)
                {
                    hashCode = (hashCode * 59) + this.PostalCode.GetHashCode();
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
            if (this.Country != null) {
                // Country (string) pattern
                Regex regexCountry = new Regex(@"^[A-Z]{2}$", RegexOptions.CultureInvariant);
                if (!regexCountry.Match(this.Country).Success)
                {
                    yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Country, must match a pattern of " + regexCountry, new [] { "Country" });
                }
            }

            yield break;
        }
    }

}
