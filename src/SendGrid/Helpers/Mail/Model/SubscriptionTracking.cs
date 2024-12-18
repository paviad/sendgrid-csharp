﻿// <copyright file="SubscriptionTracking.cs" company="Twilio SendGrid">
// Copyright (c) Twilio SendGrid. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

#if NETSTANDARD2_0
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif

namespace SendGrid.Helpers.Mail
{
    /// <summary>
    /// Allows you to insert a subscription management link at the bottom of the text and html bodies of your email. If you would like to specify the location of the link within your email, you may use the substitution_tag.
    /// </summary>
#if NETSTANDARD2_0
    // Globally defined by setting ReferenceHandler on the JsonSerializerOptions object
#else
    [JsonObject(IsReference = false)]
#endif
    public class SubscriptionTracking
    {
        /// <summary>
        /// Gets or sets a value indicating whether this setting is enabled.
        /// </summary>
#if NETSTANDARD2_0
        [JsonPropertyName("enable")]
#else
        [JsonProperty(PropertyName = "enable")]
#endif
        public bool Enable { get; set; }

        /// <summary>
        /// Gets or sets the text to be appended to the email, with the subscription tracking link. You may control where the link is by using the tag (percent symbol) (percent symbol).
        /// </summary>
#if NETSTANDARD2_0
        [JsonPropertyName("text")]
#else
        [JsonProperty(PropertyName = "text")]
#endif
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the HTML to be appended to the email, with the subscription tracking link. You may control where the link is by using the tag (percent symbol) (percent symbol).
        /// </summary>
#if NETSTANDARD2_0
        [JsonPropertyName("html")]
#else
        [JsonProperty(PropertyName = "html")]
#endif
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets a tag that will be replaced with the unsubscribe URL. for example: [unsubscribe_url]. If this parameter is used, it will override both the textand html parameters. The URL of the link will be placed at the substitution tag’s location, with no additional formatting.
        /// </summary>
#if NETSTANDARD2_0
        [JsonPropertyName("substitution_tag")]
#else
        [JsonProperty(PropertyName = "substitution_tag")]
#endif
        public string SubstitutionTag { get; set; }
    }
}
