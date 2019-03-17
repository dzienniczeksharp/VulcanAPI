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
            Account.RestClient.BaseUrl = new Uri(Account.UrlGenerator.Generate(UrlGenerator.Site.STUDENT));
            var request = new RestRequest("UczenDziennik.mvc/Get", Method.GET);

            var result = Account.RestClient.Execute(request);
            if (result.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<Diary>>>(result.Content);
            }
            else
            {
                return ApiResponse<List<Diary>>.FailedReponse;
            }
        }
    }
}
