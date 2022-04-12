using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilliardClub.Data.TagHelpers
{
    public class TagFactory
    {
        private Dictionary<string, HtmlTag> _tagsPool = new();

        public HtmlTag GetHtmlTag(string name)
        {
            if (_tagsPool.ContainsKey(name))
            {
                return _tagsPool[name];
            }
            var newTag = new HtmlTag(name);
            _tagsPool.Add(name, newTag);
            return newTag;
        }
    }
}
