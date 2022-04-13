using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.String;

namespace BilliardClub.Data.TagHelpers
{
    public class TagContext
    {
        private readonly List<string> _styles = new();
        private readonly List<string> _classes = new();
        private string _content = "";

        public void AddStyle(string style, string value)
        {
            _styles.Add(style + ":" + value + ";");
        }

        public void ClearStyle()
        {
            _styles.Clear();
        }

        public string GetStyle()
        {
            return Join("", _styles);
        }

        public void AddClass(string className)
        {
            _classes.Add(className);
        }

        public void ClearClasses()
        {
            _classes.Clear();
        }
         
        public string GetClasses()
        {
            return Join(" ", _classes);
        }

        public string GetContent()
        {
            return _content;
        }

        public void SetContent(string content)
        {
            _content = content;
        }

        public void ClearAll()
        {
            ClearStyle();
            ClearClasses();
            _content = "";
        }
    }
}
