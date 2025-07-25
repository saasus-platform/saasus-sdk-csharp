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
    /// CreateTenantUserRolesParam
    /// </summary>
    [DataContract(Name = "CreateTenantUserRolesParam")]
    public partial class CreateTenantUserRolesParam : IEquatable<CreateTenantUserRolesParam>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTenantUserRolesParam" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateTenantUserRolesParam() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTenantUserRolesParam" /> class.
        /// </summary>
        /// <param name="roleNames">Role Info (required).</param>
        public CreateTenantUserRolesParam(List<string> roleNames = default(List<string>))
        {
            // to ensure "roleNames" is required (not null)
            if (roleNames == null)
            {
                throw new ArgumentNullException("roleNames is a required property for CreateTenantUserRolesParam and cannot be null");
            }
            this.RoleNames = roleNames;
        }

        /// <summary>
        /// Role Info
        /// </summary>
        /// <value>Role Info</value>
        [DataMember(Name = "role_names", IsRequired = true, EmitDefaultValue = true)]
        public List<string> RoleNames { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateTenantUserRolesParam {\n");
            sb.Append("  RoleNames: ").Append(RoleNames).Append("\n");
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
            return this.Equals(input as CreateTenantUserRolesParam);
        }

        /// <summary>
        /// Returns true if CreateTenantUserRolesParam instances are equal
        /// </summary>
        /// <param name="input">Instance of CreateTenantUserRolesParam to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CreateTenantUserRolesParam input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.RoleNames == input.RoleNames ||
                    this.RoleNames != null &&
                    input.RoleNames != null &&
                    this.RoleNames.SequenceEqual(input.RoleNames)
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
                if (this.RoleNames != null)
                {
                    hashCode = (hashCode * 59) + this.RoleNames.GetHashCode();
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
