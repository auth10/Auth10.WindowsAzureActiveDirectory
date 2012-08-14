//-------------------------------------------------------------------------------------------------
// <copyright file="TokenHeader.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>ACS token header data contract</summary>
//-------------------------------------------------------------------------------------------------

namespace Auth10.WindowsAzureActiveDirectory.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    

    /// <summary>
    /// ACS token header contract
    /// </summary>
    public class JsonWebTokenHeader
    {
        /// <summary>
        /// Initializes a new instance of the TokenHeader class.
        /// </summary>
        public JsonWebTokenHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TokenHeader class.
        /// </summary>
        /// <param name="algo">Signing algorithm.</param>
        /// <param name="hash">Certificate hash.</param>
        public JsonWebTokenHeader(string algo, string hash)
        {
            this.TokenType = "JWT";
            this.Algorithm = algo;
            this.CertificateHash = hash;
        }

        /// <summary>
        /// Gets or sets the token type (JWT).
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the ExpiresIn.
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the certificate hash.
        /// </summary>
        public string CertificateHash { get; set; }

        /// <summary>
        /// Encodes the tokens into JSON format.
        /// </summary>
        /// <returns>OtherClaims encoded in JSON</returns>
        public string EncodeToJson()
        {
            Dictionary<string, string> allClaims = new Dictionary<string, string>();

            allClaims.Add("typ", this.TokenType);
            allClaims.Add("alg", this.Algorithm);

            if (!String.IsNullOrEmpty(this.CertificateHash))
            {
                allClaims.Add("x5t", this.CertificateHash);
            }

            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            return jserializer.Serialize(allClaims);
        }
    }
}
