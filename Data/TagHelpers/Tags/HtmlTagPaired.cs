namespace BilliardClub.Data.TagHelpers
{
    public class HtmlTagPaired : IHtmlTag
    {
        private readonly string _name;

        public HtmlTagPaired(string name)
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
