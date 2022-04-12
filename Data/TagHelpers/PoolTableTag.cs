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

            foreach (var poolTable in poolTables)
            {
                divStyle.Clear();
                pStyle.Clear();
                block.Clear();

                divStyle.Append( "left: " + poolTable.tableX + "px;" 
                    + "top: " + poolTable.tableY + "px;" 
                    + "transform: rotate(" + poolTable.tableRotation.rotationAngle + "deg);"
                    + "position: absolute;");

                pStyle.Append( "transform:\"rotate(-" + poolTable.tableRotation.rotationAngle + "deg);\"");

                if (_cart.CartPoolTables.Exists(x => x.PoolTable.id == poolTable.id))
                {
                    block.Append($"<div style =\"{divStyle}\" class =\"{cssStyle[poolTable.typeTable.name]} trigger inMyCart\">");
                }
                else if (poolTable.statusTables.Any()
                    && poolTable.statusTables.Last().status.name == "В корзине")
                {
                    block.Append($"<div style =\"{divStyle}\" class =\"{cssStyle[poolTable.typeTable.name]} trigger inOtherCart\">");
                }
                else if (poolTable.statusTables.Any() 
                    && poolTable.statusTables.Last().status.name == "Забронирован")
                {
                    block.Append($"<div style =\"{divStyle}\" class =\"{cssStyle[poolTable.typeTable.name]} trigger reserved\">");
                }
                else 
                {
                    block.Append($"<div style =\"{divStyle}\" class =\"{cssStyle[poolTable.typeTable.name]} trigger\">");
                }

                block.Append($"<p class=\"numberTable\" style={pStyle}> {poolTable.name} </p>");
                block.Append($"<input class=\"id\" type=\"hidden\" value=\"{poolTable.id}\">");
                block.Append("</div>");

                listContent.Append(block);
            }

            output.Content.SetHtmlContent(listContent.ToString());
        }
    }
}
