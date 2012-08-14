//-------------------------------------------------------------------------------------------------
// <copyright file="JsonWebToken.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>Implements the data contract for the Json Web Security Token.</summary>
//-------------------------------------------------------------------------------------------------

namespace Auth10.WindowsAzureActiveDirectory.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Definition of the JsonWebToken data contract.
    /// </summary>
    public class JsonWebToken
    {
        /// <summary>
        /// Start of the DateTime
        /// </summary>
        private static DateTime unixEpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Initializes a new instance of the JsonWebToken class.
        /// </summary>
        public JsonWebToken()
        {
            this.OtherClaims = new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Initializes a new instance of the JsonWebToken class using the claims.
        /// </summary>
        /// <param name="issuer">Token issuer.</param>
        /// <param name="audience">Token audience.</param>
        /// <param name="nbfTime">Not valid before.</param>
        /// <param name="expirationTime">Not valid after.</param>
        public JsonWebToken(
            string issuer, 
            string audience,
            DateTime nbfTime, 
            DateTime expirationTime) : this()
        {
            this.Issuer = issuer;
            this.Audience = audience;
            this.NotBeforeDateTime = nbfTime;
            this.ExpirationDateTime = expirationTime;
        }

        /// <summary>
        /// Initializes a new instance of the JsonWebToken class, initializes claims from raw values
        /// </summary>
        /// <param name="issuerPrincipalId">Service principal id of the issuer.</param>
        /// <param name="tenantRealm">Realm or context id of the tenant.</param>
        /// <param name="audienceHostName">Audience host name.</param>
        /// <param name="audiencePrincipalId">Principal id of the protected resource.</param>
        /// <param name="nbfTime">DateTime of the NotBefore claim.</param>
        /// <param name="validityInSeconds">Validity of the token in seconds.</param>
        public JsonWebToken(
            string issuerPrincipalId,
            string tenantRealm,
            string audienceHostName,
            string audiencePrincipalId,
            DateTime nbfTime,
            long validityInSeconds) : this()
        {
            this.Issuer = JsonWebTokenHelper.GetFormattedPrincipal(issuerPrincipalId, string.Empty, tenantRealm);
            this.Audience = JsonWebTokenHelper.GetFormattedPrincipal(audiencePrincipalId, audienceHostName, tenantRealm);
            this.NotBeforeDateTime = nbfTime;
            this.ExpirationDateTime = DateTime.Now.ToUniversalTime().AddSeconds(validityInSeconds);
        }

        /// <summary>
        /// Gets or sets the Issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the Audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the identity provider.
        /// </summary>
        public string IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets the Name identifier.
        /// </summary>
        public string NameIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the token is trusted for delegation.
        /// </summary>
        public bool? TrustedForDelegation { get; set; }

        /// <summary>
        /// Gets or sets the NotBefore value as DateTime.
        /// </summary>
        public DateTime NotBeforeDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Expiration value as DateTime.
        /// </summary>
        public DateTime ExpirationDateTime { get; set; }

        /// <summary>
        /// Gets the extra claims that are not represented by special properties.
        /// </summary>
        public Dictionary<string, string> OtherClaims { get; private set; }

        /// <summary>
        /// Encodes the tokens into JSON format.
        /// </summary>
        /// <returns>OtherClaims encoded in JSON</returns>
        public string EncodeToJson()
        {
            Dictionary<string, string> allClaims = new Dictionary<string, string>();

            allClaims.Add("aud", this.Audience);
            allClaims.Add("iss", this.Issuer);

            if (!string.IsNullOrEmpty(this.IdentityProvider))
            {
                allClaims.Add("identityprovider", this.IdentityProvider);
            }

            if (!string.IsNullOrEmpty(this.NameIdentifier))
            {
                allClaims.Add("nameid", this.NameIdentifier);
            }

            if (this.TrustedForDelegation.HasValue)
            {
                allClaims.Add("trustedfordelegation", this.TrustedForDelegation.Value.ToString());
            }

            long totalSeconds = (long) this.NotBeforeDateTime.Subtract(JsonWebToken.unixEpochDateTime).TotalSeconds;
            allClaims.Add("nbf", totalSeconds.ToString());

            totalSeconds = (long) this.ExpirationDateTime.Subtract(JsonWebToken.unixEpochDateTime).TotalSeconds;
            allClaims.Add("exp", totalSeconds.ToString());

            foreach (string claimType in this.OtherClaims.Keys)
            {
                allClaims.Add(claimType, this.OtherClaims[claimType]);
            }

            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            return jserializer.Serialize(allClaims);
        }
    }
}
