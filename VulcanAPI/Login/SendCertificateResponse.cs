using HtmlConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VulcanAPI.Login
{
    public class SendCertificateResponse : HtmlResponse
    {
        [HtmlProperty(".panel.linkownia.pracownik.klient a[href*=\"uonetplus-opiekun\"]", attribute: "href")]
        public List<string> OldStudentSchools { get; set; } = new List<string>();

        [HtmlProperty(".panel.linkownia.pracownik.klient a[href*=\"uonetplus-uczen\"]", attribute: "href")]
        public List<string> StudentSchools { get; set; } = new List<string>();

        public override bool IsValid()
        {
            return Title == "Uonet+" && OldStudentSchools.Count > 0 && StudentSchools.Count > 0;
        }
    }
}
