using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace VulcanAPI.Student
{
    public class Student
    {
        public Student(long studentId, string schoolId, string schoolSymbol, string studentName, string studentLastName, Diary.Diary diary)
        {
            StudentId = studentId;
            School = new School(schoolId, schoolSymbol);
            StudentName = studentName;
            StudentLastName = studentLastName;
            Diary = diary;
        }

        public long StudentId { get; set; }
        public School School { get; set; }
        public Diary.Diary Diary { get; set; }

        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFullName => $"{StudentName} {StudentLastName}";

        public Timetable GetTimetable(VulcanAccount account, DateTime date)
        {
            account.SwitchToSymbol(School.SchoolSymbol);
            account.RestClient.BaseUrl = new Uri(new UrlGenerator(account.UrlGenerator.Host, School.SchoolSymbol, School.SchoolId).Generate(UrlGenerator.Site.STUDENT));
            var request = new RestRequest("PlanZajec.mvc/Get/", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { data = date.ToString("s") });

            request.AddCookie("idBiezacyDziennik", Diary.DiaryId.ToString());
            request.AddCookie("idBiezacyDziennikPrzedszkole", Diary.KindergartenDiaryId.ToString());
            request.AddCookie("idBiezacyUczen", StudentId.ToString());

            var result = account.RestClient.Execute(request);
            if (result.IsSuccessful)
            {
                var response = JsonConvert.DeserializeObject<ApiResponse<TimetableResponse>>(result.Content);
                if (response.Success)
                {
                    return new Timetable(response.Data);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}