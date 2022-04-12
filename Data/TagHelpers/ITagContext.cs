using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilliardClub.Data.TagHelpers
{
    public interface ITagContext
    {
        void AddStyle(string style, string value);
        void ClearStyle();
        string GetStyle();
        void SetContent(string content);
        string GetContent();
    }
}
