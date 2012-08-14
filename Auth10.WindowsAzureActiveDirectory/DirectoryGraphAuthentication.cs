using System;
using Auth10.WindowsAzureActiveDirectory.Authentication;

namespace Auth10.WindowsAzureActiveDirectory
{
    public class DirectoryGraphAuthentication
    {
        /// <summary>
        /// Connection URI for the AzureAD service, read from web.config file
        /// </summary>
        private Uri connectionUri;

        /// <summary>
        /// Tenant context id, read from web.config file
        /// </summary>
        private string tenantId;

        /// <summary>
        /// Data contract version for the AzureAD Service, read from web.config file
        /// </summary>
        private string dataContractVersion;

        /// <summary>
        /// The AppPrincipalId for the service principal
        /// </summary>
        private string spnAppPrincipalId;

        /// <summary>
        /// The symmetric key for the service principal
        /// </summary>
        private string spnSymmetricKey;

        /// <summary>
        /// This is the host for AzureAD Service
        /// </summary>
        private string azureADServiceHost;

        public DirectoryGraphAuthentication(string tenantId, string spnSymmetricKey, string spnAppPrincipalId, string dataContractVersion = "0.5")
        {
            this.azureADServiceHost = "directory.windows.net";
            this.connectionUri = new Uri(string.Format(@"https://{0}/{1}", azureADServiceHost, tenantId));
            this.tenantId = tenantId;
            this.spnSymmetricKey = spnSymmetricKey;
            this.dataContractVersion = dataContractVersion;
            this.spnAppPrincipalId = spnAppPrincipalId;
        }

        public OAuthAccessToken GetAccessToken()
        {
            string stsUrl = "https://accounts.accesscontrol.windows.net/tokens/OAuth/2";
            string AcsPrincipalId = "00000001-0000-0000-c000-000000000000";
            // Service Principal ID for the graphService principal - this is a Universal (reserved) id for all tenants
            string protectedResourcePrincipalId = "00000002-0000-0000-c000-000000000000";
            string protectedResourceHostName = "directory.windows.net";
            
            var webToken = new JsonWebToken(
                                            this.spnAppPrincipalId,
                                            tenantId.ToString(),
                                            (new Uri(stsUrl)).DnsSafeHost,
                                            AcsPrincipalId,
                                            DateTime.Now.ToUniversalTime(),
                                            60 * 60);

            string jwt = JsonWebTokenHelper.GenerateAssertion(webToken, this.spnSymmetricKey);

            string resource = String.Format("{0}/{1}@{2}", protectedResourcePrincipalId, protectedResourceHostName, tenantId);
            OAuthAccessToken accessToken = JsonWebTokenHelper.GetOAuthAccessTokenFromACS(stsUrl, jwt, resource);

            return accessToken;
        }
    }
}