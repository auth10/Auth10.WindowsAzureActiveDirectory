/// <copyright file="ErrorDetail.cs" company="Microsoft">
/// Copyright (c) Microsoft Corporation. All rights reserved.
/// </copyright>
/// <Summary> 
/// Project:      AzureAD Sample Application
/// Copyright (c) Microsoft Corporation.
/// 
/// This source is subject to the Sample Client End User License Agreement included in this project
/// Sample Client EULA.rtf
/// 
/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
/// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
/// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
/// </summary>
namespace Auth10.WindowsAzureActiveDirectory.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Extended error detail.
    /// </summary>
    [DataContract(Namespace = "")]
    public class ErrorDetail
    {
        /// <summary>
        /// Initializes a new instance of the ErrorDetail class.
        /// </summary>
        /// <param name="item">Error item.</param>
        /// <param name="value">Error value.</param>
        public ErrorDetail(string item, string value)
        {
            this.Item = item;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the error item.
        /// </summary>
        [DataMember(Name = "item")]
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the error value.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }   
}
