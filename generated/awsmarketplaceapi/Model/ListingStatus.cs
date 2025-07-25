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
    /// Defines ListingStatus
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ListingStatus
    {
        /// <summary>
        /// Enum NoListing for value: no_listing
        /// </summary>
        [EnumMember(Value = "no_listing")]
        NoListing = 1,

        /// <summary>
        /// Enum FirstStepWorking for value: first_step_working
        /// </summary>
        [EnumMember(Value = "first_step_working")]
        FirstStepWorking = 2,

        /// <summary>
        /// Enum FirstStepCompleted for value: first_step_completed
        /// </summary>
        [EnumMember(Value = "first_step_completed")]
        FirstStepCompleted = 3,

        /// <summary>
        /// Enum SecondStepWorking for value: second_step_working
        /// </summary>
        [EnumMember(Value = "second_step_working")]
        SecondStepWorking = 4,

        /// <summary>
        /// Enum SecondStepPlanCreated for value: second_step_plan_created
        /// </summary>
        [EnumMember(Value = "second_step_plan_created")]
        SecondStepPlanCreated = 5,

        /// <summary>
        /// Enum SecondStepCompleted for value: second_step_completed
        /// </summary>
        [EnumMember(Value = "second_step_completed")]
        SecondStepCompleted = 6,

        /// <summary>
        /// Enum ThirdStepWorking for value: third_step_working
        /// </summary>
        [EnumMember(Value = "third_step_working")]
        ThirdStepWorking = 7,

        /// <summary>
        /// Enum ThirdStepCompleted for value: third_step_completed
        /// </summary>
        [EnumMember(Value = "third_step_completed")]
        ThirdStepCompleted = 8,

        /// <summary>
        /// Enum Limited for value: limited
        /// </summary>
        [EnumMember(Value = "limited")]
        Limited = 9,

        /// <summary>
        /// Enum Restricted for value: restricted
        /// </summary>
        [EnumMember(Value = "restricted")]
        Restricted = 10,

        /// <summary>
        /// Enum Public for value: public
        /// </summary>
        [EnumMember(Value = "public")]
        Public = 11
    }

}
