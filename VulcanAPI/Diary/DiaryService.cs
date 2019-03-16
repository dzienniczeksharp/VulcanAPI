using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VulcanAPI.Diary
{
    public class DiaryService : BaseService
    {
        public DiaryService(VulcanAccount vulcanAccount) : base(vulcanAccount) { }

        public ApiResponse<List<Diary>> GetDiares()
        {
            var client = new RestClient(Account.UrlGenerator.Generate(UrlGenerator.Site.STUDENT));
            var request = new RestRequest("UczenDziennik.mvc/Get", Method.GET);
            foreach (var cookie in Account.Cookies)
                request.AddCookie(cookie.Name, cookie.Value);

            var result = client.Execute(request);
            if (result.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<Diary>>>(client.Execute(request).Content);
            }
            else
            {
                return ApiResponse<List<Diary>>.FailedReponse;
            }
        }
    }
}
