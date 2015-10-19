using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceApp.Helpers
{   
    public static class DisableHtmlControlExtension
    {

        //Helper class used to disable HTML elements.
        //Disables an element if the boolean expression is valid
        //usage: @Html.ActionLink("Example", "Example").DisableIf(() => (200.00 > 100.00))
        public static MvcHtmlString DisableIf(this MvcHtmlString htmlString, Func<bool> expression)
        {
            if (expression.Invoke())
            {
                var html = htmlString.ToString();
                const string disabled = "\"disabled\"";
                html = html.Insert(html.IndexOf(">",
                  StringComparison.Ordinal), " disabled= " + disabled);
                return new MvcHtmlString(html);
            }
            return htmlString;
        }
    }
}