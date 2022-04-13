namespace BilliardClub.Data.TagHelpers
{
    public class HtmlTagSingle : IHtmlTag
    {
        private readonly string _name;

        public HtmlTagSingle(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public string Output(TagContext context)
        {
            return "<" + GetName() + " style=\"" + context.GetStyle() + "\"" + context.GetContent() + "class = \"" + context.GetClasses() + "\">";
        }
    }
}