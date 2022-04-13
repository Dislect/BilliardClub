namespace BilliardClub.Data.TagHelpers
{
    public interface IHtmlTag
    {
        public string GetName();

        public string Output(TagContext context);
    }
}