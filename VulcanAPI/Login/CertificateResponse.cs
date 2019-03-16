using HtmlAgilityPack;
using HtmlConvert;
using System;
using System.Collections.Generic;
using System.Text;

namespace VulcanAPI.Login
{
    public class Wresult
    {
        [HtmlProperty("samlattribute[AttributeName=UserInstance] samlattributevalue")]
        public List<string> Symbols { get; set; }
    }

    public class CertificateResponse : HtmlResponse
    {
        [HtmlProperty("form[name=hiddenform]", attribute: "action")]
        public string Action { get; set; }

        [HtmlProperty("input[name=wa]", attribute: "value")]
        public string Wa { get; set; }

        private string wresultRaw;

        [HtmlProperty("input[name=wresult]", attribute: "value")]
        public string WresultRaw
        {
            get { return wresultRaw; }
            set { wresultRaw = HtmlEntity.DeEntitize(value);
                Wresult = HtmlConvert.HtmlConvert.DeserializeObject<Wresult>(wresultRaw.Replace(":", "")); }
        }

        public Wresult Wresult { get; set; }

        [HtmlProperty("input[name=wctx]", attribute: "value")]
        public string Wctx { get; set; }

        public override bool IsValid()
        {
            if (Title == null || Title != "Working...")
                return false;
            if (Wa == null || Wresult == null || Wctx == null)
                return false;
            return true;
        }
    }
}
