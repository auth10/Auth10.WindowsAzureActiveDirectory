using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Net;

namespace Auth10.WindowsAzureActiveDirectory.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Scenarios
    {
        private DirectoryGraph graph;

        [TestInitialize]
        public void Init()
        {
            // set this so we can fiddle
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            var auth = new DirectoryGraphAuthentication(ConfigurationManager.AppSettings["TenantId"], ConfigurationManager.AppSettings["SymmetricKey"], ConfigurationManager.AppSettings["AppPrincipalId"]);
            var accessToken = auth.GetAccessToken(); // you can cache this until token.ExpiresOn
            this.graph = new DirectoryGraph(ConfigurationManager.AppSettings["TenantId"], accessToken.AccessToken);
        }

        [TestMethod]
        public void GetUsers()
        {
            string next;
            var users = this.graph.GetUsers(out next);

            Assert.AreEqual(3, users.Count);
            Assert.AreEqual("John Foo", users[0].DisplayName);
            Assert.AreEqual("John Blocked", users[1].DisplayName);
            Assert.AreEqual("Matias Woloski", users[2].DisplayName);
        }

        [TestMethod]
        public void GetUser()
        {
            var user = this.graph.GetUser(new Guid("1409fc2a-6aaf-436b-87e3-ee1ce0098b8e"));

            Assert.IsNotNull(user);
            Assert.AreEqual("Matias Woloski", user.DisplayName);
            Assert.AreEqual("matias@auth10dev.onmicrosoft.com", user.Mail);
            Assert.AreEqual("Buenos Aires", user.City);

        }

        [TestMethod]
        public void GetBlockedUsers()
        {
            var users = this.graph.GetBlockedUsers();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("John Blocked", users[0].DisplayName);
        }

        [TestMethod]
        public void GetAdministrators()
        {
            var users = this.graph.GetAdministrators();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("Matias Woloski", users[0].DisplayName);
        }

        [TestMethod]
        public void GetUserSecurityGroups()
        {
            var groups = this.graph.GetUserSecurityGroups(new Guid("1409fc2a-6aaf-436b-87e3-ee1ce0098b8e"));

            Assert.IsNotNull(groups);
            Assert.AreEqual(2, groups.Count);
            Assert.AreEqual("Company Administrator", groups[0].DisplayName);
            Assert.AreEqual("Test Group", groups[1].DisplayName);
        }

        [TestMethod]
        public void GetUserSecurityGroupsByMail()
        {
            var groups = this.graph.GetUserSecurityGroups("matias@auth10dev.onmicrosoft.com");

            Assert.IsNotNull(groups);
            Assert.AreEqual(2, groups.Count);
            Assert.AreEqual("Company Administrator", groups[0].DisplayName);
            Assert.AreEqual("Test Group", groups[1].DisplayName);
        }

        [TestMethod]
        public void GetUsersWithPaging()
        {
            string next;
            this.graph.DefaultPageSize = 2; // make page size 2 so we get two pages
            var firstPage = this.graph.GetUsers(out next);
            var secondPage = this.graph.GetUsersNextPage(next, out next);

            Assert.IsNull(next, "should have got only two pages and after the second page, next should be null because there is no third page");
            Assert.IsNotNull(firstPage);
            Assert.IsNotNull(secondPage);

            Assert.AreEqual(2, firstPage.Count);
            Assert.AreEqual(1, secondPage.Count);
            
            Assert.AreEqual("John Foo", firstPage[0].DisplayName);
            Assert.AreEqual("John Blocked", firstPage[1].DisplayName);
            Assert.AreEqual("Matias Woloski", secondPage[0].DisplayName);
        }

        [TestMethod]
        public void GetTenantSkus()
        {
            var skus = this.graph.GetTenantSkus();

            Assert.AreEqual(1, skus.Count);
            Assert.AreEqual(1, skus[0].ConsumedUnits); //we have only 1 license available in dev mode
            Assert.AreEqual(1, skus[0].PrepaidUnits.Enabled);
            Assert.AreEqual(0, skus[0].PrepaidUnits.Suspended);
            Assert.AreEqual(0, skus[0].PrepaidUnits.Warning);
            Assert.AreEqual("DEVPACK_B_PILOT", skus[0].SkuPartNumber);
            Assert.AreEqual("OFFICE_PRO_PLUS_SUBSCRIPTION_B_PILOT", skus[0].ServicePlans[0].ServicePlanName);
            Assert.AreEqual("SHAREPOINTWAC_DEVELOPER_B_PILOT", skus[0].ServicePlans[1].ServicePlanName);
            Assert.AreEqual("LYNC_S_ENTERPRISE_B_PILOT", skus[0].ServicePlans[2].ServicePlanName);
            Assert.AreEqual("SHAREPOINT_S_DEVELOPER_B_PILOT", skus[0].ServicePlans[3].ServicePlanName);
            Assert.AreEqual("EXCHANGE_S_ENTERPRISE_B_PILOT", skus[0].ServicePlans[4].ServicePlanName);
        }

        [TestMethod]
        public void SearchUsers()
        {
            string next;
            var users = this.graph.SearchUsers("Matias Woloski", out next);

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("Matias Woloski", users[0].DisplayName);
        }

        [TestMethod]
        public void SearchUsersSpecifyingField()
        {
            string next;
            var users = this.graph.SearchUsers("Buenos Aires", out next, "City");

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual("Matias Woloski", users[0].DisplayName);
            Assert.AreEqual("John Foo", users[1].DisplayName);
        }
    }
}
