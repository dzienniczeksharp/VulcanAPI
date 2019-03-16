using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;
using VulcanAPI.Diary;
using VulcanAPI.Login;

namespace VulcanAPI
{
    public class LoginConfig
    {
        public LoginConfig(string host, string baseSymbol, string email, string password, bool isTestLog = false)
        {
            Host = host;
            BaseSymbol = baseSymbol;
            Email = email;
            Password = password;
            IsTestLog = isTestLog;
        }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("symbol")]
        public string BaseSymbol { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("isTestLog")]
        public bool IsTestLog { get; set; }
    }

    public class VulcanAccount
    {
        public LoginConfig Login { get; }
        public UrlGenerator UrlGenerator { get; set; }
        public List<RestResponseCookie> Cookies { get; set; }

        public LoginService LoginService { get; }
        public DiaryService DiaryService { get; }

        public VulcanAccount(LoginConfig login)
        {
            this.Login = login;
            UrlGenerator = new UrlGenerator(Login.Host, Login.BaseSymbol);

            LoginService = new LoginService(this);
            DiaryService = new DiaryService(this);
        }

        public List<Student.Student> Students { get; } = new List<Student.Student>();

        public void InitializeStudents()
        {
            LoginService.Login(Login.Email, Login.Password, out var certificate, out var sendCertificate);

            var lastSymbol = Login.BaseSymbol;
            foreach (var schoolSymbol in certificate.Wresult.Symbols.Select(x => x.Trim()).Where(x => new Regex("[a-zA-Z]*").IsMatch(x)))
            {
                UrlGenerator.Symbol = schoolSymbol;
                certificate.Action = certificate.Action.Replace(lastSymbol, schoolSymbol);
                lastSymbol = schoolSymbol;

                try
                {
                    sendCertificate = LoginService.SendCertificate(certificate);
                }
                catch (Exception)
                {
                    continue;
                }

                if (!sendCertificate.IsValid())
                    continue;

                UrlGenerator.SchoolId = sendCertificate.StudentSchools[0].Split('/')[4];

                var diares = DiaryService.GetDiares();
                if (diares.Success)
                {
                    foreach (var diary in diares.Data)
                    {
                        if (!Students.Any(x => x.StudentId == diary.StudentId))
                            Students.Add(new Student.Student(diary.StudentId, UrlGenerator.SchoolId, UrlGenerator.Symbol, diary.StudentName, diary.StudentLastName));
                    }
                }
            }
        }
    }
}