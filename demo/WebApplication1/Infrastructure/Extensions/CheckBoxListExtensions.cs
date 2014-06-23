using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication1.Infrastructure.Enums;

namespace WebApplication1.Infrastructure.Extensions
{
    public static class CheckBoxListExtensions
    {
        #region -- CheckBoxList (Horizontal) --
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">SelectListItem.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo)
        {
            return htmlHelper.CheckBoxList(name, listInfo, (IDictionary<string, object>)null, 0);
        }

        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">SelectListItem.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            object htmlAttributes)
        {
            return htmlHelper.CheckBoxList(
                name, 
                listInfo, 
                (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes), 
                0);
        }

        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="number">每個Row的顯示個數.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int number)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必須給這些 CheckBoxList 一個 Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "必須要給List<SelectListItem> listInfo");
            }
            if (!listInfo.Any())
            {
                throw new ArgumentException("List<SelectListItem> listInfo 至少要有一組資料", "listInfo");
            }

            var sb = new StringBuilder();
            var lineNumber = 0;

            foreach (SelectListItem info in listInfo)
            {
                lineNumber++;

                TagBuilder builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                builder.MergeAttribute("id", string.Format("{0}_{1}", name, info.Value));
                sb.Append(builder.ToString(TagRenderMode.Normal));

                var labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", string.Format("{0}_{1}", name, info.Value));
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                if (number == 0 || (lineNumber % number == 0))
                {
                    sb.Append("<br />");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region -- CheckBoxListVertical --
        /// <summary>
        /// Checks the box list vertical.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListVertical(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int columnNumber = 1)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必須給這些 CheckBoxList 一個 Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "必須要給 List<CheckBoxListInfo> listInfo");
            }
            var selectListItems = listInfo as SelectListItem[] ?? listInfo.ToArray();
            if (!selectListItems.Any())
            {
                throw new ArgumentException("List<CheckBoxListInfo> listInfo 至少要有一組資料", "listInfo");
            }

            var dataCount = selectListItems.Count();

            // calculate number of rows
            var rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(columnNumber)));
            if (dataCount <= columnNumber || dataCount - columnNumber == 1)
            {
                rows = dataCount;
            }

            var wrapBuilder = new TagBuilder("div");
            wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");

            var wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
            var wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
            var wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

            var sb = new StringBuilder();
            sb.Append(wrapStart);

            var lineNumber = 0;

            foreach (var info in selectListItems)
            {
                var builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                builder.MergeAttribute("id", string.Format("{0}_{1}", name, info.Value));
                sb.Append(builder.ToString(TagRenderMode.Normal));

                var labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", string.Format("{0}_{1}", name, info.Value));
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                lineNumber++;

                if (lineNumber.Equals(rows))
                {
                    sb.Append(wrapBreak);
                    lineNumber = 0;
                }
                else
                {
                    sb.Append("<br/>");
                }
            }
            sb.Append(wrapClose);
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region -- CheckBoxList (Horizonal, Vertical) --
        /// <summary>
        /// Checks the box list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="position">The position.</param>
        /// <param name="number">Position.Horizontal則表示每個Row的顯示個數, Position.Vertical則表示要顯示幾個Column</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            Position position = Position.Horizontal,
            int number = 0)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必須給這些 CheckBoxList 一個 Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "必須要給List<SelectListItem> listInfo");
            }
            var selectListItems = listInfo as SelectListItem[] ?? listInfo.ToArray();
            if (!selectListItems.Any())
            {
                throw new ArgumentException("List<SelectListItem> listInfo 至少要有一組資料", "listInfo");
            }

            var sb = new StringBuilder();
            var lineNumber = 0;

            switch (position)
            {
                case Position.Horizontal:

                    foreach (var info in selectListItems)
                    {
                        lineNumber++;
                        sb.Append(CreateCheckBoxItem(info, name, htmlAttributes));

                        if (number == 0 || (lineNumber % number == 0))
                        {
                            sb.Append("<br />");
                        }
                    }
                    sb.Append("<br />");
                    break;

                case Position.Vertical:

                    var dataCount = selectListItems.Count();

                    // 計算最大顯示的列數(rows)
                    var rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(number)));
                    if (dataCount <= number || dataCount - number == 1)
                    {
                        rows = dataCount;
                    }

                    var wrapBuilder = new TagBuilder("div");
                    wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");

                    var wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
                    var wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
                    var wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

                    sb.Append(wrapStart);

                    foreach (var info in selectListItems)
                    {
                        lineNumber++;
                        sb.Append(CreateCheckBoxItem(info, name, htmlAttributes));

                        if (lineNumber.Equals(rows))
                        {
                            sb.Append(wrapBreak);
                            lineNumber = 0;
                        }
                        else
                        {
                            sb.Append("<br/>");
                        }
                    }
                    sb.Append(wrapClose);
                    break;
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Creates the check box item.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="name">The name.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        internal static string CreateCheckBoxItem(
            SelectListItem info, 
            string name, 
            IDictionary<string, object> htmlAttributes)
        {
            var sb = new StringBuilder();

            var labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", string.Format("{0}_{1}", name, info.Value));
            sb.Append(labelBuilder.ToString(TagRenderMode.StartTag));

            var builder = new TagBuilder("input");
            if (info.Selected)
            {
                builder.MergeAttribute("checked", "checked");
            }
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("value", info.Value);
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("id", string.Format("{0}_{1}", name, info.Value));
            sb.Append(builder.ToString(TagRenderMode.Normal));

            sb.AppendFormat(" {0}",info.Text);
            sb.Append("</label>");

            return sb.ToString();
        }
        #endregion
    }

}