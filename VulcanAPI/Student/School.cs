using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using HtmlConvert;

namespace VulcanAPI.Student
{
    public class School
    {
        public School(string schoolId, string schoolSymbol)
        {
            SchoolId = schoolId;
            SchoolSymbol = schoolSymbol;
        }

        public string SchoolId { get; set; }
        public string SchoolSymbol { get; set; }

        public int GetLuckyNumber(VulcanAccount account)
        {
            var client = new RestClient(new UrlGenerator(account.UrlGenerator.Host, SchoolSymbol).Generate(UrlGenerator.Site.HOME));
            var request = new RestRequest("Start.mvc/Index", Method.GET);
            foreach (var cookie in account.Cookies)
                request.AddCookie(cookie.Name, cookie.Value);

            var result = client.Execute(request);
            if (result.IsSuccessful)
            {
                return HtmlConvert.HtmlConvert.DeserializeObject<LuckyNumberResponse>(result.Content).LuckyNumer;
            }
            else
            {
                return -1;
            }
        }
    }

    public class LuckyNumberResponse : HtmlResponse
    {
        [HtmlProperty(".panel.szczesliweNumery .subDiv.pCont .subDiv.pCont .subDiv", regex: "(\\d+)")]
        public int LuckyNumer { get; set; } = -1;

        public override bool IsValid()
        {
            return true;
        }
    }
}
