using BilliardClub.Data.TagHelpers;
using BilliardClub.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilliardClub.Data.HtmlTags
{
    public class PoolTableTagHelper : TagHelper
    {
        private Cart _cart;
        Dictionary<string, string> cssStyle = new Dictionary<string, string>()
        {
            ["Русский бильярд"] = "rusb",
            ["Американский пул"] = "poolb"
        };
        public List<PoolTable> poolTables { get; set; }

        public PoolTableTagHelper(Cart cart)
        {
            _cart = cart;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            StringBuilder divStyle = new();
            StringBuilder pStyle = new();
            StringBuilder listContent = new();
            StringBuilder block = new();

            var factory = new TagFactory();
            var divContext = new TagContext();
            var pContext = new TagContext();
            var div = factory.GetHtmlTag("div");
            var p = factory.GetHtmlTag("p");
            var input = factory.GetHtmlTag("input");

            foreach (var poolTable in poolTables)
            {
                divContext.Clear();
                pContext.Clear();

                divContext.AddStyle("left", poolTable.tableX + "px");
                divContext.AddStyle("top", poolTable.tableY + "px");
                divContext.AddStyle("transform", $"rotate({poolTable.tableRotation.rotationAngle}deg)");
                divContext.AddStyle("position", "absolute");
                divContext.AddClass(cssStyle[poolTable.typeTable.name]);
                divContext.AddClass("trigger");

                pContext.AddStyle("transform", $"rotate(-{poolTable.tableRotation.rotationAngle}deg)");
                pContext.AddClass("numberTable");
                pContext.SetContent(poolTable.name);

                divContext.SetContent(p.Output(pContext) +
                    $"<input class=\"id\" type=\"hidden\" value=\"{poolTable.id}\">");

                if (_cart.CartPoolTables.Exists(x => x.PoolTable.id == poolTable.id))
                {
                    divContext.AddClass("inMyCart");
                    block.Append(div.Output(divContext));
                }
                else if (poolTable.statusTables.Any()
                    && poolTable.statusTables.Last().status.name == "В корзине")
                {
                    divContext.AddClass("inOtherCart");
                    block.Append(div.Output(divContext));
                }
                else if (poolTable.statusTables.Any() 
                    && poolTable.statusTables.Last().status.name == "Забронирован")
                {
                    divContext.AddClass("reserved");
                    block.Append(div.Output(divContext));
                }
                else 
                {
                    block.Append(div.Output(divContext));
                }

                listContent.Append(block);
            }

            output.Content.SetHtmlContent(listContent.ToString());
        }
    }
}
