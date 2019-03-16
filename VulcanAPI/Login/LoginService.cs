using HtmlAgilityPack;
using RestSharp;
using System;
using System.Linq;

namespace VulcanAPI.Login
{
    public class LoginService : BaseService
    {
        private string FirstStepReturnUrl { get; }

        public LoginService(VulcanAccount vulcanAccount) : base(vulcanAccount)
        {
            var loginEndpoint = HtmlEntity.Entitize($"{vulcanAccount.UrlGenerator.Schema}://uonetplus.{vulcanAccount.UrlGenerator.Host}/{vulcanAccount.UrlGenerator.Symbol}/LoginEndpoint.aspx");
            FirstStepReturnUrl = $"/{vulcanAccount.UrlGenerator.Symbol}/FS/LS?wa=wsignin1.0&wtrealm={loginEndpoint}&wctx={loginEndpoint}";
        }

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

        public SendCertificateResponse SendCertificate(CertificateResponse certificate)
        {
            var client = new RestClient();
            var request = new RestRequest(certificate.Action, Method.POST);
            request.AddParameter("wa", certificate.Wa, ParameterType.GetOrPost);
            request.AddParameter("wresult", certificate.WresultRaw, ParameterType.GetOrPost);
            request.AddParameter("wctx", certificate.Wctx, ParameterType.GetOrPost);

            var result = client.Execute(request);
            if (result.IsSuccessful)
            {
                Account.Cookies = result.Cookies.ToList();
                return HtmlConvert.HtmlConvert.DeserializeObject<SendCertificateResponse>(result.Content);
            }
            else
            {
                throw result.ErrorException ?? new Exception($"{result.StatusDescription} {result.ErrorMessage}");
            }
        }
    }
}