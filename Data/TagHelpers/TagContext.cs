using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilliardClub.Data.TagHelpers
{
    public class TagContext : ITagContext
    {
        private List<string> _styles = new();
        private List<string> _classes = new();
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
            return String.Join("", _styles);
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
            return String.Join(" ", _classes);
        }

        public string GetContent()
        {
            return _content;
        }

        public void SetContent(string content)
        {
            _content = content;
        }

        public void Clear()
        {
            _classes.Clear();
            _content = "";
            _styles.Clear();
        }
    }
}
