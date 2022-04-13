using BilliardClub.Data.TagHelpers;
using BilliardClub.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BilliardClub.Data.TagHelpers.Interfaces;

namespace BilliardClub.Data.HtmlTags
{
    public class PoolTableTagHelper : TagHelper
    {
        private Cart _cart;
        private ITagFactory factory;
        private StringBuilder listContent = new();
        private StringBuilder block = new();
        Dictionary<string, string> cssStyle = new()
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

            factory = new TagPairedFactory();
            var div = factory.GetHtmlTag("div");
            var p = factory.GetHtmlTag("p");
            var divContext = new TagContext();
            var pContext = new TagContext();

            factory = new TagSingleFactory();
            var input = factory.GetHtmlTag("input");
            var inputContext = new TagContext();

            foreach (var poolTable in poolTables)
            {
                divContext.ClearAll();
                pContext.ClearAll();
                inputContext.ClearAll();

                divContext.AddStyle("left", poolTable.tableX + "px");
                divContext.AddStyle("top", poolTable.tableY + "px");
                divContext.AddStyle("transform", $"rotate({poolTable.tableRotation.rotationAngle}deg)");
                divContext.AddStyle("position", "absolute");
                divContext.AddClass(cssStyle[poolTable.typeTable.name]);
                divContext.AddClass("trigger");

                pContext.AddStyle("transform", $"rotate(-{poolTable.tableRotation.rotationAngle}deg)");
                pContext.AddClass("numberTable");
                pContext.SetContent(poolTable.name);

                inputContext.AddClass("id");
                inputContext.SetContent($"type=\"hidden\" value=\"{poolTable.id}\"");

                divContext.SetContent(p.Output(pContext) + input.Output(inputContext));

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
