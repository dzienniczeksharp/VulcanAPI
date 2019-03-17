using HtmlAgilityPack;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace VulcanAPI.Login
{
    public class LoginService : BaseService
    {
        private string FirstStepReturnUrl {
            get
            {
                var loginEndpoint = HtmlEntity.Entitize($"{Account.UrlGenerator.Schema}://uonetplus.{Account.UrlGenerator.Host}/{Account.UrlGenerator.Symbol}/LoginEndpoint.aspx");
                return $"/{Account.UrlGenerator.Symbol}/FS/LS?wa=wsignin1.0&wtrealm={loginEndpoint}&wctx={loginEndpoint}";
            }
        }

        public LoginService(VulcanAccount vulcanAccount) : base(vulcanAccount) { }

        public void Login(string email, string password) => Login(email, password, out _, out _);


        public void Login(string email, string password, out CertificateResponse certificate, out SendCertificateResponse sendCertificate)
        {
            certificate = SendCredentials(email, password);
            if (!certificate.IsValid())
                throw new Exception("Certificate is invalid");

            sendCertificate = SendCertificate(certificate);
        }

        public CertificateResponse SendCredentials(string email, string password)
        {
            var client = new RestClient(Account.UrlGenerator.Generate(UrlGenerator.Site.LOGIN));
            var request = new RestRequest("/Account/LogOn", Method.POST);
            request.AddQueryParameter("ReturnUrl", FirstStepReturnUrl);
            request.AddParameter("LoginName", email, ParameterType.GetOrPost);
            request.AddParameter("Password", password, ParameterType.GetOrPost);

            var result = client.Execute(request);
            if (result.IsSuccessful)
            {
                return HtmlConvert.HtmlConvert.DeserializeObject<CertificateResponse>(result.Content);
            }
            else
            {
                throw result.ErrorException ?? new Exception($"{result.StatusDescription} {result.ErrorMessage}");
            }
        }

        public string CurrentSymbol { get; set; }

        public SendCertificateResponse SendCertificate(CertificateResponse certificate)
        {
            Account.RestClient.BaseUrl = null;
            Account.RestClient.CookieContainer = new CookieContainer();

            var request = new RestRequest(certificate.Action, Method.POST);
            request.AddParameter("wa", certificate.Wa, ParameterType.GetOrPost);
            request.AddParameter("wresult", certificate.WresultRaw, ParameterType.GetOrPost);
            request.AddParameter("wctx", certificate.Wctx, ParameterType.GetOrPost);

            var result = Account.RestClient.Execute(request);
            if (result.IsSuccessful)
            {
                CurrentSymbol = Account.UrlGenerator.Symbol;
                return HtmlConvert.HtmlConvert.DeserializeObject<SendCertificateResponse>(result.Content);
            }
            else
            {
                throw result.ErrorException ?? new Exception($"{result.StatusDescription} {result.ErrorMessage}");
            }
        }
    }
}