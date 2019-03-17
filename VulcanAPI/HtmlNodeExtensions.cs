using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VulcanAPI
{
    public static class HtmlNodeExtensions
    {
        public static string[] GetClasses(this HtmlNode node)
        {
            return node.GetAttributeValue("css", "").Split(' ');
        }

        public static bool HasClass(this HtmlNode node, string className)
        {
            return node.GetClasses().Contains(className);
        }
    }
}
