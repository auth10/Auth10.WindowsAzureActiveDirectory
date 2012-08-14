/// <copyright file="ErrorResponse.cs" company="Microsoft">
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
    /// Defines the error response contract.
    /// </summary>
    [DataContract(Name = "error", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the ErrorResponse class.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public ErrorResponse(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
