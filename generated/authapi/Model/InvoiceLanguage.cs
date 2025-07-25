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
    /// Language of invoice
    /// </summary>
    /// <value>Language of invoice</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvoiceLanguage
    {
        /// <summary>
        /// Enum JaJP for value: ja-JP
        /// </summary>
        [EnumMember(Value = "ja-JP")]
        JaJP = 1,

        /// <summary>
        /// Enum EnUS for value: en-US
        /// </summary>
        [EnumMember(Value = "en-US")]
        EnUS = 2
    }

}
