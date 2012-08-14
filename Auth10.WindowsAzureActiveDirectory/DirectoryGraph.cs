/// <copyright file="AzureADApiInterface.cs" company="Microsoft">
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
namespace Auth10.WindowsAzureActiveDirectory
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Net;
    using Auth10.WindowsAzureActiveDirectory.Infrastructure;

    /// <summary>
    /// This is the only class that interacts with AzureAD API Service. 
    /// All the calls to AzureAD API go thru this class
    /// </summary>
    public class DirectoryGraph
    {        
        /// <summary>
        /// Proxy instance for the AzureAD service
        /// </summary>
        private DirectoryDataServiceProxy dataService;

        /// <summary>
        /// Data contract version for the AzureAD Service, read from web.config file
        /// </summary>
        private readonly string dataContractVersion;

        private readonly string accessToken;

        private readonly Uri connectionUri;

        private readonly int defaultPageSize;

        public DirectoryGraph(string tenantId, string accessToken, string dataContractVersion = "0.5", int defaultPageSize = 20)
        {
            this.dataContractVersion = dataContractVersion;
            this.defaultPageSize = defaultPageSize;

            this.connectionUri = new Uri(string.Format(@"https://directory.windows.net/{0}", tenantId));
            this.dataService = new DirectoryDataServiceProxy(connectionUri);
            this.dataService.IgnoreResourceNotFoundException = true;

            this.accessToken = accessToken;

            this.dataService.SendingRequest += delegate(object sender1, SendingRequestEventArgs args)
            {
                ((HttpWebRequest)args.Request).Headers.Add(Constants.HeaderNameAuthorization, "Bearer " + this.accessToken);
                ((HttpWebRequest)args.Request).Headers.Add(Constants.HeaderNameDataContractVersion, dataContractVersion);
            };
        }

        /// <summary>
        /// Get all the users
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetUsers(out string nextPageUrl)
        {
            string continuation = null;
            List<User> results = null;
            InvokeOperationWithRetry(() =>
            {
                var users = dataService.Users.AddQueryOption("$top", this.defaultPageSize);
                var userQuery = users.Execute();
                results = userQuery.ToList();

                // continuation token for next page. This is null if there is no next page
                var token = ((QueryOperationResponse)userQuery).GetContinuation();
                if (token != null)
                {
                    continuation = token.NextLinkUri.ToString();
                }
            });

            nextPageUrl = continuation;
            return results;
        }

        /// <summary>
        /// Get subscribed skus for tenant
        /// </summary>
        /// <returns>Subscribed skus for tenant</returns>
        public List<SubscribedSku> GetTenantSkus()
        {
            List<SubscribedSku> subscribedSkus = new List<SubscribedSku>();

            InvokeOperationWithRetry(() =>
            {
                // Get tenant SKUs
                var skus = dataService.SubscribedSkus.Execute();
                subscribedSkus = skus.ToList();
            });

            return subscribedSkus;
        }

        /// <summary>
        /// Get the next page of results.
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetUsersNextPage(string linkUrl, out string nextPageUrl)
        {
            string continuation = null;
            List<User> results = null;
            InvokeOperationWithRetry(() =>
            {
                var page = new Uri(string.Format("{0}&$top={1}", linkUrl, this.defaultPageSize));

                var userQuery = dataService.Execute<User>(page);

                // Get the return users list
                results = userQuery.ToList();

                // Continuation token for next page. This is null if there is no next page
                // If it is not null, it contains the URI to the next page
                var token = ((QueryOperationResponse)userQuery).GetContinuation();

                if (token != null)
                {
                    continuation = token.NextLinkUri.ToString();
                }
            });

            nextPageUrl = continuation;
            return results;
        }

        /// <summary>
        /// Gets a user object with given object id
        /// </summary>
        /// <param name="objectId">Object id for the user</param>
        /// <returns>The user object</returns>
        public User GetUser(Guid objectId)
        {
            // Get a user with specified object id
            string detailsUserQuery = string.Format("{0}('{1}')", "Users", objectId);
            var user = dataService.CreateQuery<User>(detailsUserQuery).ToList<User>()[0];
            return user;
        }

        /// <summary>
        /// Get links for the user with given object id
        /// </summary>
        /// <param name="objectId">Object id for the user</param>
        /// <param name="linkName">link to get</param>
        /// <returns>Links for the user</returns>
        public List<ReferencedObject> GetLinks(Guid objectId, string linkName)
        {
            // Get links for the user with given object Id
            string query = string.Format("{0}/{1}('{2}')/{3}", connectionUri, "Users", objectId, linkName);
            var linkQuery = dataService.Execute<ReferencedObject>(new Uri(query));
            return linkQuery.ToList();
        }

        /// <summary>
        /// Execute a dataservice query
        /// </summary>
        /// <param name="value">Request query</param>
        /// <returns>List of users.</returns>
        public List<User> ExecuteQuery(string value, out string nextPageUrl, string field = "Display Name")
        {
            List<User> results = null;
            string next = null;
            InvokeOperationWithRetry(() =>
            {
                // create the filtered query
                var users = dataService.Users.AddQueryOption("$filter", field + " eq '" + value + "'")
                                             .AddQueryOption("$top", this.defaultPageSize);
                
                var userQuery = users.Execute();
                results = userQuery.ToList();
                var token = ((QueryOperationResponse)userQuery).GetContinuation();
                if (token != null)
                {
                    next = token.NextLinkUri.ToString();
                }
            });

            nextPageUrl = next;
            return results;
        }

        /// <summary>
        /// Get all the company administrator users.
        /// </summary>
        /// <returns>List of administrators</returns>
        public List<User> GetAdministrators()
        {
            List<User> results = null;
            InvokeOperationWithRetry(() =>
            {
                // get all the roles in the tenant
                var roleQuery = dataService.Roles.Execute();
                List<Role> AzureADReturned = roleQuery.ToList();

                // find the company admin role
                Role adminRole =
                    AzureADReturned.Where<Role>(role => role.DisplayName == "Company Administrator").First<Role>();

                // Get the link 'Members' for the role
                // Not that it doesnt return the actual user object,
                // but it returns 'ReferencedObject' which has the 
                // user's objectid.
                string roleMemberQuery = string.Format("{0}('{1}')/Members", "Roles", adminRole.ObjectId);
                List<ReferencedObject> AzureADRoleMembers = dataService.CreateQuery<ReferencedObject>(roleMemberQuery).ToList<ReferencedObject>();

                // For all the referenced objects, get the real user object and add it to the list
                List<User> adminUsers = new List<User>();
                foreach (var AzureADRolemember in AzureADRoleMembers)
                {
                    adminUsers.Add(GetUser(AzureADRolemember.ObjectId.GetValueOrDefault()));
                }

                // return list of company administrators
                results = adminUsers;
            });

            return results;
        }

        public List<Group> GetUserSecurityGroups(Guid userObjectId)
        {
            var results = new List<Group>();
            InvokeOperationWithRetry(() =>
            {

                var groups = this.GetLinks(userObjectId, "MemberOf");

                foreach (var group in groups)
                {
                    results.Add(new Group 
                    {   
                        DisplayName = group.DisplayName, 
                        Mail = group.Mail, 
                        ObjectId = group.ObjectId, 
                        ObjectType = group.ObjectType, 
                        ObjectReference = group.ObjectReference
                    });
                }
            });

            return results;
        }

        public List<Group> GetUserSecurityGroups(string email)
        {
            string next;
            List<User> users = ExecuteQuery(email, out next, "Mail");
            if (users.Count == 1)
            {
                return this.GetUserSecurityGroups(users[0].ObjectId.Value);
            }

            return null;
        }

        /// <summary>
        /// Gets the list of users whose account is blocked
        /// </summary>
        /// <returns>List of users whose account is blocked</returns>
        public List<User> GetBlockedUsers()
        {
            List<User> results = null;
            // Get sign-in blocked users
            InvokeOperationWithRetry(() =>
            {
                var userQueryUri = new Uri(string.Format("{0}/Users?$filter=AccountEnabled eq false", connectionUri.ToString()));
                var userQuery = dataService.Execute<User>(userQueryUri);
                results = userQuery.ToList();
            });

            return results;
        }

        /// <summary>
        /// Delegate for invoking networking operations with retry.
        /// </summary>
        /// <param name="operation">the operation to invoke with retry</param>
        private void InvokeOperationWithRetry(Action operation)
        {
            int retryremaining = Constants.MaxRetryAttempts;
            while (retryremaining > 0)
            {
                try
                {
                    operation();

                    // The operation is successful, so retryremaining=0
                    retryremaining = 0;
                }
                catch (Exception ex)
                {
                    // Operation not successful

                    // De-serialize error message to check the error code from AzureAD Service
                    ErrorResponseEx parsedException = DataServiceExceptionUtil.DeserializeErrorMessage(ex);
                    if (parsedException == null)
                    {
                        // Could not parse the exception so it wasn't in the format of DataServiceException
                        throw ex;
                    }
                    else
                    {
                        // Look at the error code to determine if we want to retry on this exception 
                        switch (parsedException.Code)
                        {
                            // These are the errors we dont want to rety on
                            // Please look at the definistions for details about each of these
                            case Constants.MessageIdAuthorizationIdentityDisabled:
                            case Constants.MessageIdAuthorizationIdentityNotFound:
                            case Constants.MessageIdAuthorizationRequestDenied:
                            case Constants.MessageIdBadRequest:
                            case Constants.MessageIdBindingRedirectionInternalServerError:
                            case Constants.MessageIdContractVersionHeaderMissing:
                            case Constants.MessageIdHeaderNotSupported:
                            case Constants.MessageIdInternalServerError:
                            case Constants.MessageIdInvalidDataContractVersion:
                            case Constants.MessageIdInvalidReplicaSessionKey:
                            case Constants.MessageIdInvalidRequestUrl:
                            case Constants.MessageIdMediaTypeNotSupported:
                            case Constants.MessageIdMultipleObjectsWithSameKeyValue:
                            case Constants.MessageIdObjectNotFound:
                            case Constants.MessageIdResourceNotFound:
                            case Constants.MessageIdThrottledPermanently:
                            case Constants.MessageIdThrottledTemporarily:
                            case Constants.MessageIdUnauthorized:
                            case Constants.MessageIdUnknown:
                            case Constants.MessageIdUnsupportedQuery:
                            case Constants.MessageIdUnsupportedToken:
                                {
                                    retryremaining = 0;

                                    // We just create a new expection with the msg
                                    // and throw it so that the 'OnException' handler handles it
                                    throw new InvalidOperationException(parsedException.Message, ex);
                                }

                            // This exception means that the user's data is not in current DataCenter, 
                            // special handling is required for this exception
                            case Constants.MessageIdBindingRedirection:
                                {
                                    HandleBindingRedirectionException(parsedException, operation);
                                    retryremaining = 0;
                                    break;
                                }

                            // This means that the replica we were trying to go to was unavailable, 
                            // retry will possibly go to another replica and may work
                            case Constants.MessageIdReplicaUnavailable:
                                {
                                    retryremaining--;
                                    break;
                                }

                            // This means that the token has expired. 
                            case Constants.MessageIdExpired:
                                {
                                 
                                    // authHeader = GetAuthorizationHeader();
                                    retryremaining--;
                                    break;
                                }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to handle binding redirection exception. This exception means that the 
        /// user's data is located in another data center. This exception's details returns
        /// several urls that may work in this case. At least one url is guaranteed to work
        /// So we need to get all the URLs and try them
        /// </summary>
        /// <param name="parsedException">The binding redirection exception we received</param>
        /// <param name="operation">The operation to try</param>
        private void HandleBindingRedirectionException(ErrorResponseEx parsedException, Action operation)
        {
            List<string> urls = new List<string>();

            // Go thru the error details name\value pair
            foreach (ErrorDetail ed in parsedException.Values)
            {
                // if the name is something like url1, url2 add it to the list of URLs
                if (ed.Item.StartsWith("url"))
                {
                    urls.Add(ed.Value);
                }
            }

            // Now try each URL
            foreach (string newUrl in urls)
            {
                // We permanantly change the dataservice to point to the new URL
                // as none of the operations will work on the current url
                dataService = new DirectoryDataServiceProxy(new Uri(newUrl));

                try
                {
                    // try the operation
                    operation();

                    // if the operation is successful, break out of the loop
                    // all the subsequent operations will go to the new URL
                    break;
                }
                catch (Exception)
                {
                    // nothing can be done, try next URL
                }
            }
        }
    }
}
