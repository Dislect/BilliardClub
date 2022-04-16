using BilliardClub.Data.TagHelpers;
using BilliardClub.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BilliardClub.Data.DBModels.Repository;
using BilliardClub.Data.TagHelpers.Interfaces;

namespace BilliardClub.Data.HtmlTags
{
    public class PoolTableTagHelper : TagHelper
    {
        private Cart _cart;
        private StatusRepository _statusRepository;
        private ITagFactory factory;
        private StringBuilder listContent = new();
        Dictionary<string, string> cssStyle = new()
        {
            ["Русский бильярд"] = "rusb",
            ["Американский пул"] = "poolb"
        };
        public List<PoolTable> poolTables { get; set; }

        public PoolTableTagHelper(Cart cart, StatusRepository statusRepository)
        {
            _cart = cart;
            _statusRepository = statusRepository;
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
                divContext.AddStyle("background-image", $"url({poolTable.typeTable.picturePath})");
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
                    listContent.Append(div.Output(divContext));
                }
                else if (poolTable.statusTables.Any()
                    && poolTable.statusTables.Last().status.id == _statusRepository.GetStatusInCart().id)
                {
                    divContext.AddClass("inOtherCart");
                    listContent.Append(div.Output(divContext));
                }
                else if (poolTable.statusTables.Any() 
                    && poolTable.statusTables.Last().status.id == _statusRepository.GetStatusReserved().id)
                {
                    divContext.AddClass("reserved");
                    listContent.Append(div.Output(divContext));
                }
                else 
                {
                    listContent.Append(div.Output(divContext));
                }
            }

            output.Content.SetHtmlContent(listContent.ToString());
        }
    }
}
