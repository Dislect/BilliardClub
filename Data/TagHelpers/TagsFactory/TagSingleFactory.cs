using System.Collections.Generic;
using BilliardClub.Data.TagHelpers.Interfaces;

namespace BilliardClub.Data.TagHelpers
{
    public class TagSingleFactory : ITagFactory
    {
        private readonly Dictionary<string, HtmlTagSingle> _tagsPool = new();

        public IHtmlTag GetHtmlTag(string name)
        {
            if (_tagsPool.ContainsKey(name))
            {
                return _tagsPool[name];
            }
            var newTag = new HtmlTagSingle(name);
            _tagsPool.Add(name, newTag);
            return newTag;
        }
    }
}