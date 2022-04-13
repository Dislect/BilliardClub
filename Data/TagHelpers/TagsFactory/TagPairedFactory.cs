using System.Collections.Generic;
using BilliardClub.Data.TagHelpers.Interfaces;

namespace BilliardClub.Data.TagHelpers
{
    public class TagPairedFactory : ITagFactory
    {
        private readonly Dictionary<string, HtmlTagPaired> _tagsPool = new();

        public IHtmlTag GetHtmlTag(string name)
        {
            if (_tagsPool.ContainsKey(name))
            {
                return _tagsPool[name];
            }
            var newTag = new HtmlTagPaired(name);
            _tagsPool.Add(name, newTag);
            return newTag;
        }
    }
}
