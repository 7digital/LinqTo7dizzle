using System.Configuration;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace LinqTo7Dizzle.OAuth
{
    public class ServiceDescriptions
    {
        private static readonly string _consumerKey = ConfigurationManager.AppSettings["consumerKey"];
        private static readonly string _baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private static readonly string _accountUrl = ConfigurationManager.AppSettings["accountUrl"];

        public static ServiceProviderDescription SignIn
        {
            get
            {
                return new ServiceProviderDescription
                {
                    RequestTokenEndpoint = new MessageReceivingEndpoint(_baseUrl + "oauth/requesttoken",
                        HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),

                    UserAuthorizationEndpoint = new MessageReceivingEndpoint(_accountUrl + _consumerKey + "/oauth/authorise",
                        HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),

                    AccessTokenEndpoint = new MessageReceivingEndpoint(_baseUrl + "oauth/accesstoken",
                        HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),

                    TamperProtectionElements = new ITamperProtectionChannelBindingElement[]
                    {
                        new HmacSha1SigningBindingElement()
                    },
                };
            }
        }
    }
}