/// <copyright file="Constants.cs" company="Microsoft">
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
    using System.Linq;
    using System.Web;

    /// <summary>
    /// This class contains the constant fields used in this sample,
    /// primarily all the error codes amongst others
    /// </summary>
    public static class Constants
    {
        #region Resource Id

        /// <summary>
        /// Message Id for unauthorized request.
        /// </summary>
        public const string MessageIdUnauthorized = "Authentication_Unauthorized";

        /// <summary>
        /// Message id for expired tokens.
        /// </summary>
        public const string MessageIdExpired = "Authentication_ExpiredToken";

        /// <summary>
        /// Message id for unknown authentication failures.
        /// </summary>
        public const string MessageIdUnknown = "Authentication_Unknown";

        /// <summary>
        /// Message id for unsupported token type.
        /// </summary>
        public const string MessageIdUnsupportedToken = "Authentication_UnsupportedTokenType";

        /// <summary>
        /// Message id for the data contract missing error message
        /// </summary>
        public const string MessageIdContractVersionHeaderMissing = "Headers_DataContractVersionMissing";

        /// <summary>
        /// Message id for an invalid data contract version.
        /// </summary>
        public const string MessageIdInvalidDataContractVersion = "Headers_InvalidDataContractVersion";

        /// <summary>
        /// Message id for the data contract missing error message
        /// </summary>
        public const string MessageIdHeaderNotSupported = "Headers_HeaderNotSupported";

        /// <summary>
        /// When the company object could not be read.
        /// </summary>
        public const string MessageIdObjectNotFound = "Directory_ObjectNotFound";

        /// <summary>
        /// The most generic message id, when the fault is due to a server error.
        /// </summary>
        public const string MessageIdInternalServerError = "Service_InternalServerError";

        /// <summary>
        /// The replica session key provided in the request is invalid.
        /// </summary>
        public const string MessageIdInvalidReplicaSessionKey = "Request_InvalidReplicaSessionKey";

        /// <summary>
        /// The replica session key provided in the request is invalid.
        /// </summary>
        public const string MessageIdBadRequest = "Request_BadRequest";

        /// <summary>
        /// The replica session key provided in the request is unavailable.
        /// </summary>
        public const string MessageIdReplicaUnavailable = "Directory_ReplicaUnavailable";

        /// <summary>
        /// User's data is not in the current datacenter.
        /// </summary>
        public const string MessageIdBindingRedirection = "Directory_BindingRedirection";

        /// <summary>
        /// Calling principal's information could not be read from the directory.
        /// </summary>
        public const string MessageIdAuthorizationIdentityNotFound = "Authorization_IdentityNotFound";

        /// <summary>
        /// Calling principal is disabled.
        /// </summary>
        public const string MessageIdAuthorizationIdentityDisabled = "Authorization_IdentityDisabled";

        /// <summary>
        /// Request is denied due to insufficient privileges.
        /// </summary>
        public const string MessageIdAuthorizationRequestDenied = "Authorization_RequestDenied";

        /// <summary>
        /// Encountered an internal error when trying to populate the nearest datacenter endpoints.
        /// </summary>
        public const string MessageIdBindingRedirectionInternalServerError
            = "Directory_BindingRedirectionInternalServerError";

        /// <summary>
        /// The request is throttled temporarily
        /// </summary>
        public const string MessageIdThrottledTemporarily = "Request_ThrottledTemporarily";

        /// <summary>
        /// The request is throttled permanently
        /// </summary>
        public const string MessageIdThrottledPermanently = "Request_ThrottledPermanently";

        /// <summary>
        /// The request query was rejected because it was either invalid or unsupported.
        /// </summary>
        public const string MessageIdUnsupportedQuery = "Request_UnsupportedQuery";

        /// <summary>
        /// Request is denied due to insufficient privileges.
        /// </summary>
        public const string MessageIdInvalidRequestUrl = "Request_InvalidRequestUrl";

        /// <summary>
        /// Request failed because a target object could not be found.
        /// </summary>
        public const string MessageIdResourceNotFound = "Request_ResourceNotFound";

        /// <summary>
        /// Request failed due to the presence of objects with duplicate values in key-valued properties.
        /// </summary>
        public const string MessageIdMultipleObjectsWithSameKeyValue = "Request_MultipleObjectsWithSameKeyValue";

        /// <summary>
        /// The requested media type is not supported - the $format parameter value is not supported.
        /// </summary>
        public const string MessageIdMediaTypeNotSupported = "Request_MediaTypeNotSupported";

        #endregion

        /// <summary>
        /// Maximus retry attempt for a retryable exception from AzureAD Service
        /// </summary>
        public const int MaxRetryAttempts = 3;

        /// <summary>
        /// Content type header
        /// </summary>
        public const string HeaderNameAuthorization = "Authorization";

        /// <summary>
        /// Http Header name of data contract version
        /// </summary>
        public const string HeaderNameDataContractVersion = "x-ms-dirapi-data-contract-version";
    }
}