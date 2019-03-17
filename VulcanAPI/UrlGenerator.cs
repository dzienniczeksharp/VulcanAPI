using System;
using System.Collections.Generic;
using System.Text;

namespace VulcanAPI
{
    public class UrlGenerator
    {
        public string Host { get; set; }
        public string Symbol { get; set; }
        public string SchoolId { get; set; }
        public bool SSL { get; set; }
        public string Schema => SSL ? "https" : "http";

        public UrlGenerator(string host, string symbol, string schoolId = null, bool ssl = true)
        {
            Host = host;
            Symbol = symbol;
            SSL = ssl;
            SchoolId = schoolId;
        }

        public enum Site
        {
            LOGIN,
            HOME,
            SNP,
            STUDENT,
            MESSAGES
        }

        public string Generate(Site type) => $"{Schema}://{GetSubDomain(type)}.{Host}/{Symbol}/{((type == Site.SNP || type == Site.STUDENT) ? SchoolId : "")}";

        private string GetSubDomain(Site type)
        {
            switch (type)
            {
                case Site.LOGIN:
                    return "cufs";
                case Site.HOME:
                    return "uonetplus";
                case Site.SNP:
                    return "uonetplus-opiekun";
                case Site.STUDENT:
                    return "uonetplus-uczen";
                case Site.MESSAGES:
                    return "uonetplus-uzytkownik";
            }

            return null;
        }
    }

}
