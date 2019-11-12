using Faculty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Helpers
{
    public static class ListHelper
    {
        public static MvcHtmlString HomeCourseListHelper(this HtmlHelper html)
        {
            List<string> ls = new List<string>() { "z-a", "Самые длительные", "Самые короткие", "Самые популярные" };

            TagBuilder select = new TagBuilder("select");
            select.MergeAttribute("name", "sort");
            select.MergeAttribute("id", "sort");
            select.MergeAttribute("onclick", "FormSubmit()");

            foreach (string par in ls)
            {
                TagBuilder option = new TagBuilder("option");
                option.MergeAttribute("value", par);
                option.InnerHtml = par;
                select.InnerHtml += option.ToString();
            }

            return new MvcHtmlString(select.ToString());
        }

        public static MvcHtmlString ResultList(this HtmlHelper html, IEnumerable<JournalViewModel> courses)
        {
            TagBuilder ul = new TagBuilder("ul");
            foreach (JournalViewModel course in courses)
            {
                TagBuilder li = new TagBuilder("li");
                li.SetInnerText($"{course.CourseName} - {course.Mark}");
                ul.InnerHtml += li.ToString();
            }
            return new MvcHtmlString(ul.ToString());
        }

        public static MvcHtmlString TutorsCourseList(this HtmlHelper html, IEnumerable<string> courses)
        {
            TagBuilder ul = new TagBuilder("ul");
            foreach (string course in courses)
            {
                TagBuilder li = new TagBuilder("li");
                li.SetInnerText(course);
                ul.InnerHtml += li.ToString();
            }
            return new MvcHtmlString(ul.ToString());
        }
    }
}