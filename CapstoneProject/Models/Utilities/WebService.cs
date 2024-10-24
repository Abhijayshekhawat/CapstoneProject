using WS_LDAP_Search;
using Microsoft.Extensions.Configuration;  // Import for IConfiguration

namespace CapstoneProject.Models.Utilities
{
    public class WebService
    {
        private readonly string _webServiceUsername;
        private readonly string _webServicePassword;

        // Constructor Injection of IConfiguration
        public WebService(IConfiguration configuration)
        {
            _webServiceUsername = configuration["Connection_WS_Username"];
            _webServicePassword = configuration["Connection_WS_Password"];
        }

        // Synchronous method to get user info by AccessNet ID
        public TempleLDAPEntry GetUserInfoByAccessNet(string accessnetID)
        {
            try
            {
                var ldapProxy = new LDAP_SearchSoapClient(LDAP_SearchSoapClient.EndpointConfiguration.LDAP_SearchSoap);

                // Call the web service synchronously
                var response = ldapProxy.SearchAsync(_webServiceUsername, _webServicePassword, "uid", accessnetID).Result;
                var result = response.Body.SearchResult.FirstOrDefault();

                if (result != null && result.error == null)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine($"Error: {result?.error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }
    }
}