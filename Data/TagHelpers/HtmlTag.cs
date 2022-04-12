using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilliardClub.Data.TagHelpers
{
    public class HtmlTag
    {
        private string _name;

        public HtmlTag(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }


        public string Output(TagContext context)
        {
            string res = "<" + GetName() + " style=\"" + context.GetStyle() + "\"" + "class = \"" + context.GetClasses() + "\">";
            res += context.GetContent();
            res += "</" + GetName() + ">";
            return res;
        }
    }
}
