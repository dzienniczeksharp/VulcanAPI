using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Fizzler.Systems.HtmlAgilityPack;

namespace VulcanAPI.Student
{
    public class TimetableResponse
    {
        [JsonProperty("Data")]
        public string MondayDate { get; set; }

        [JsonProperty("Header")]
        public List<Header> Headers { get; set; }

        [JsonProperty("Rows")]
        public List<string[]> Rows { get; set; }

        public partial class Header
        {
            [JsonProperty("Text")]
            public string Text { get; set; }
        }
    }

    public class Timetable
    {
        public Timetable(TimetableResponse response)
        {
            var index = 1;
            foreach (var header in response.Headers.Skip(1))
            {
                var splitted = header.Text.Split(new string[] { "<br />" }, StringSplitOptions.None);
                var day = new Day()
                {
                    Name = splitted[0],
                    Date = DateTimeOffset.ParseExact(splitted[1], "dd.MM.yyyy", CultureInfo.CurrentCulture)
                };

                foreach (var row in response.Rows)
                {
                    const string timeFormat = "H:m";
                    var splittedRow = row[0].Split(new string[] { "<br />" }, StringSplitOptions.None);

                    var lesson = new Lesson()
                    {
                        Number = int.Parse(splittedRow[0]),
                        TimeStart = DateTimeOffset.ParseExact(splittedRow[1], timeFormat, null),
                        TimeEnd = DateTimeOffset.ParseExact(splittedRow[2], timeFormat, null),
                    };

                    var html = new HtmlDocument();
                    html.LoadHtml(row[index]);
                    var divs = html.DocumentNode.QuerySelectorAll("div").ToArray();

                    const string LessonPlanned = "x-treelabel-ppl";
                    const string LessonRealized = "x-treelabel-rlz";
                    const string LessonChanges = "x-treelabel-zas";
                    const string LessonCancelled = "x-treelabel-inv";

                    if (divs.Length == 1)
                    {
                        var spans = divs[0].QuerySelectorAll("span").ToArray();
                        lesson.Subject = spans.ElementAtOrDefault(0)?.InnerHtml;
                        lesson.Teacher = spans.ElementAtOrDefault(1)?.InnerHtml;
                        lesson.Room = spans.ElementAtOrDefault(2)?.InnerHtml;
                        lesson.Reason = spans.ElementAtOrDefault(3)?.InnerHtml;

                        lesson.IsCancelled = spans[0].HasClass(LessonCancelled);
                        lesson.IsPlanned = spans[0].HasClass(LessonPlanned);
                    }
                    else if(divs.Length == 2)
                    {
                        
                    }

                    day.Lessons.Add(lesson);
                }

                Days.Add(day);
                index++;
            }
        }

        public List<Day> Days { get; set; } = new List<Day>();

        public partial class Day
        {
            public string Name { get; set; }
            public DateTimeOffset Date { get; set; }
            public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        }

        public partial class Lesson
        {
            public string Subject { get; set; }
            public string Room { get; set; }
            public string Teacher { get; set; }
            public string Reason { get; set; }
            public bool IsCancelled { get; set; }
            public bool IsPlanned { get; set; }
            public bool IsRealized { get; set; }

            public int Number { get; set; }
            public DateTimeOffset TimeStart { get; set; }
            public DateTimeOffset TimeEnd { get; set; }
        }
    }
}
