using HtmlConvert;
using System;
using System.Collections.Generic;
using System.Text;

namespace VulcanAPI
{
    public abstract class HtmlResponse
    {
        [HtmlProperty("title")]
        public string Title { get; set; }

        public abstract bool IsValid();
    }
}
