using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace VulcanAPI.Test
{
    public class BasicTests
    {
        private readonly ITestOutputHelper _output;
        public BasicTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public static IEnumerable<object[]> GetLogins()
        {
            yield return new object[] { JsonConvert.DeserializeObject<LoginConfig>(File.ReadAllText("config.json")) };
            yield return new object[] { new LoginConfig("fakelog.cf", "Default", "jan@fakelog.cf", "jan123", true) };
        }

        [Theory]
        [MemberData(nameof(GetLogins))]
        public void LoginTest(LoginConfig login)
        {
            var account = new VulcanAccount(login);
            account.LoginService.Login(login.Email, login.Password, out var certificate, out var sendCertificate);

            Assert.NotNull(certificate);
            Assert.True(certificate.IsValid());

            Assert.NotNull(sendCertificate);
            Assert.True(sendCertificate.IsValid());

            _output.WriteLine($"Successfuly logged with {login.Email}, {sendCertificate.StudentSchools.Count} students, {certificate.Wresult.Symbols.Count} symbols");
        }

        [Theory]
        [MemberData(nameof(GetLogins))]
        public void StudentsTest(LoginConfig login)
        {
            var account = new VulcanAccount(login);
            account.InitializeStudents();

            Assert.True(account.Students.Count > 0);
        }

        [Theory]
        [MemberData(nameof(GetLogins))]
        public void LuckyNumberTest(LoginConfig login)
        {
            var account = new VulcanAccount(login);
            account.InitializeStudents();

            foreach (var school in account.Students.Select(x=>x.School).DistinctBy(x=>x.SchoolId))
            {
                var luckyNumber = school.GetLuckyNumber(account);
                if(login.IsTestLog)
                    Assert.Equal(DateTime.Now.Hour <= 7 ? -1 : DateTime.Today.Day, luckyNumber);
                _output.WriteLine($"{school.SchoolSymbol} have lucky {luckyNumber} today");
            }
        }

        [Theory]
        [MemberData(nameof(GetLogins))]
        public void TimetableTest(LoginConfig login)
        {
            var account = new VulcanAccount(login);
            account.InitializeStudents();

            var timetable = account.Students[0].GetTimetable(account, DateTime.Today);

            Assert.NotNull(timetable);
            Assert.NotEmpty(timetable.Days);
            Assert.NotEmpty(timetable.Days[0].Lessons);
        }
    }
}
