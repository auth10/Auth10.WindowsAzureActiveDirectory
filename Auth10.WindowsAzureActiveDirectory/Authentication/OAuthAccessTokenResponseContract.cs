//-------------------------------------------------------------------------------------------------
// <copyright file="OAuthAccessTokenResponseContract.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>OAuth access token response data contract.</summary>
//-------------------------------------------------------------------------------------------------

namespace Auth10.WindowsAzureActiveDirectory.Authentication
{
    using System.Runtime.Serialization;

    /// <summary>
    /// OAuth access token response
    /// </summary>
    [DataContract]
    public class OAuthAccessTokenResponseContract
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the ExpiresIn.
        /// </summary>
        [DataMember(Name = "expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the expires on.
        /// </summary>
        [DataMember(Name = "expires_on")]
        public long ExpiresOn { get; set; }

        /// <summary>
        /// Gets or sets the not before.
        /// </summary>
        [DataMember(Name = "not_before")]
        public long NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }
}
