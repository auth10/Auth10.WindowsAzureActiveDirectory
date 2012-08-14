# Windows Azure Active Directory Graph API

* This library simplifies access to Windows Azure Active Directory from .NET applications. 
* It is derived from original samples included in the Windows Azure Active Directory Preview. The original samples are more of proof of concepts and they were based on a static API (`AzureADGraph.GetUsers`) that wasn't thread safe. The paging implementation was akwards as well. We are cleaning that up. Also removed the dependency with AAL until it doesn't requires the c++ runtime dependency.  
* It includes integration tests with a dev tenant
* It provides a NuGet

## Usage

```
Install-Package Auth10.WindowsAzureActiveDirectory
```

```cs
// authenticate
var auth = new DirectoryGraphAuthentication(tenantId, symmetricKey, appPrincipalId);
var token = auth.GetAccessToken(); // you can cache this until token.ExpiresOn

// use it
graph = new DirectoryGraph(tenantId, token.AccessToken);
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
```

## How to Get TenantId, SymmetricKey and AppPrincipalId

### Creating the symmetric key and app principal id

```powershell
Import-Module MSOnline
Import-Module MSOnlineExtended

Connect-MsolService 

$symmetricKey = "FStnXT1QON84B............5onYtzJ91Gg/JH/Jxiw"
$appPrincipalId = "2829c758-2bef-....-a685-717089474509"

$sp = New-MsolServicePrincipal -ServicePrincipalNames @("yourappname/some.host.com") -AppPrincipalId $appPrincipalId -DisplayName "yourappname" -Type Symmetric -Value $symmetricKey -Usage Verify -StartDate "1/1/2012" -EndDate "11/11/2014" 
```

# assign permissions to that principal to query the graph (Service Support Administrator == read, Company Administrator == readwrite)
Add-MsolRoleMember -RoleMemberType ServicePrincipal -RoleName "Service Support Administrator" -RoleMemberObjectId $sp.ObjectId

### Getting your tenantId

```powershell
(get-msolcompanyinformation).objectId
```
