using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VulcanAPI.Diary
{
    public partial class Diary
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("IdUczen")]
        public long StudentId { get; set; }

        [JsonProperty("UczenImie")]
        public string StudentName { get; set; }

        [JsonProperty("UczenImie2")]
        public string StudentSecondName { get; set; }

        [JsonProperty("UczenNazwisko")]
        public string StudentLastName { get; set; }

        [JsonProperty("IsDziennik")]
        public bool IsDiary { get; set; }

        [JsonProperty("IdDziennik")]
        public long DiaryId { get; set; }

        [JsonProperty("IdPrzedszkoleDziennik")]
        public long KindergartenDiaryId { get; set; }

        // 1, 2 , 3
        [JsonProperty("Poziom")]
        public long ClassLevel { get; set; }

        // a, b, c
        [JsonProperty("Symbol")]
        public string ClassSymbol { get; set; }

        [JsonProperty("Nazwa", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Name { get; set; }

        [JsonProperty("DziennikRokSzkolny")]
        public long Year { get; set; }

        [JsonProperty("Okresy")]
        public Semester[] Semseters { get; set; }

        [JsonProperty("IdSioTyp")]
        public long IdSioTyp { get; set; }

        [JsonProperty("IsDorosli")]
        public bool IsAdults { get; set; }

        [JsonProperty("IsPolicealna")]
        public bool IsPostSecondary { get; set; }

        [JsonProperty("Is13")]
        public bool Is13 { get; set; }

        [JsonProperty("IsArtystyczna")]
        public bool IsArtistic { get; set; }

        [JsonProperty("IsArtystyczna13")]
        public bool IsArtistic13 { get; set; }

        [JsonProperty("IsSpecjalny")]
        public bool IsSpecial { get; set; }

        [JsonProperty("IsPrzedszkola")]
        public bool IsKinderGarten { get; set; }

        [JsonProperty("UczenPelnaNazwa")]
        public string StudentFullName { get; set; }
    }

    public partial class Semester
    {
        [JsonProperty("NumerOkresu")]
        public long Number { get; set; }

        [JsonProperty("Poziom")]
        public long Level { get; set; }

        [JsonProperty("DataOd")]
        public string Start { get; set; }

        [JsonProperty("DataDo")]
        public string End { get; set; }

        [JsonProperty("IdOddzial")]
        public long ClassId { get; set; }

        [JsonProperty("IdJednostkaSprawozdawcza")]
        public long UnitId { get; set; }

        [JsonProperty("IsLastOkres")]
        public bool IsLast { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }
    }
}
