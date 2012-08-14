# Windows Azure Active Directory Graph

## Usage

// authenticate
var auth = new DirectoryGraphAuthentication(tenantId, symmetricKey, appPrincipalId);
var token = auth.GetAccessToken(); // you can cache this until token.ExpiresOn

// use it
graph = new DirectoryGraph(ConfigurationManager.AppSettings["TenantContextId"], token.AccessToken);
var user = graph.GetUser(new Guid("123...."));

// get all users (default page size 20 and can be changed from ctor)
string nextPageUrl;
graph.GetUsers(out nextPageUrl);            
graph.GetUsers(nextPageUrl, out nextPageUrl);            

graph.GetAdministrators();

graph.GetUserSecurityGroups(user.ObjectId);

graph.GetBlockedUsers();

graph.GetLinks(id, "DirectReports");

graph.GetTenantSkus();