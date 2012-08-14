using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

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
            var auth = new DirectoryGraphAuthentication(ConfigurationManager.AppSettings["TenantId"], ConfigurationManager.AppSettings["SymmetricKey"], ConfigurationManager.AppSettings["AppPrincipalId"]);
            var accessToken = auth.GetAccessToken(); // you can cache this until token.ExpiresOn
            this.graph = new DirectoryGraph(ConfigurationManager.AppSettings["TenantId"], accessToken.AccessToken);
        }

        [TestMethod]
        public void GetUsers()
        {
            string next;
            var users = this.graph.GetUsers(out next);

            Assert.AreEqual(2, users.Count);
        }

        [TestMethod]
        public void GetUser()
        {
            var user = this.graph.GetUser(new Guid("3c6960fd-cb81-407d-b1a9-10ef594a9d1f"));

            Assert.IsNotNull(user);
            Assert.AreEqual("Matias Woloski", user.DisplayName);
            Assert.AreEqual("matias@auth10test2.onmicrosoft.com", user.Mail);
            Assert.AreEqual("Buenos Aires", user.City);

        }

        [TestMethod]
        public void GetUserSecurityGroups()
        {
            var groups = this.graph.GetUserSecurityGroups(new Guid("3c6960fd-cb81-407d-b1a9-10ef594a9d1f"));

            Assert.IsNotNull(groups);
            Assert.AreEqual(1, groups.Count);
            Assert.AreEqual("Company Administrator", groups[0].DisplayName);
        }

        [TestMethod]
        public void GetUserSecurityGroupsByMail()
        {
            var groups = this.graph.GetUserSecurityGroups("matias@auth10test2.onmicrosoft.com");

            Assert.IsNotNull(groups);
            Assert.AreEqual(1, groups.Count);
            Assert.AreEqual("Company Administrator", groups[0].DisplayName);
        }
    }
}
