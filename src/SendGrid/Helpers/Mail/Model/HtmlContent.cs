﻿// <copyright file="HtmlContent.cs" company="Twilio SendGrid">
// Copyright (c) Twilio SendGrid. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

#if NETSTANDARD2_0
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif

namespace SendGrid.Helpers.Mail.Model
{
    /// <summary>
    /// Helper class for plain html mime types.
    /// </summary>
#if NETSTANDARD2_0
    // Globally defined by setting ReferenceHandler on the JsonSerializerOptions object
#else
    [JsonObject(IsReference = false)]
#endif
    public class HtmlContent : Content
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlContent"/> class.
        /// </summary>
        /// <param name="value">The actual content of the specified mime type that you are including in your email.</param>
        public HtmlContent(string value)
        {
            this.Type = MimeType.Html;
            this.Value = value;
        }
    }
}
