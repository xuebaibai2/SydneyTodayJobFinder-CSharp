using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        HtmlFilter hf = new HtmlFilter();
        RequestHtml requestHtml = new RequestHtml();
        //private const string sydneytoday = "http://www.sydneytoday.com";
        // GET: Home
        public ActionResult Index()
        {
            return View(hf);
        }

        public ActionResult GetContent(HtmlFilter hf, int? page)
        {
            Html html = new Html();
            if (hf.DivId == null)
            {
                hf.DivId = "list_middle";
            }
            if (page == 1 || page == 0 || page == null)
            {

                html.contentList = GetFilteredContent(hf.URL, hf.Keyword, hf.DivId);
            }
            else if (page > 0 && page > 1)
            {
                html.contentList = GetFilteredContent(hf.URL, hf.Keyword, hf.DivId);
                for (int i = 1; i <= page; i++)
                {
                    hf.URL = "http://www.sydneytoday.com/job_information?page=" + i;
                    html.contentList.AddRange(GetFilteredContent(hf.URL, hf.Keyword, hf.DivId));
                }
            }

            return View(html);
        }

        private void GetWebContext(string url, string divID)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            string scheme = web.ResponseUri.Scheme;
            string host = web.ResponseUri.Host;

            HtmlNode rateNode = doc.DocumentNode.SelectSingleNode("//div[@id='" + divID + "']");
            requestHtml.Host = scheme + "://" + host;
            requestHtml.Content = rateNode.InnerHtml;
        }

        private List<string> GetFilteredContent(string url, string keyword, string divID)
        {
            List<string> resultList = new List<string>();
            GetWebContext(url, divID);

            MatchCollection collection = Regex.Matches(requestHtml.Content, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
            var list = collection.Cast<Match>().Where(m => m.Value.Contains(keyword)).ToList();
            foreach (var title in list)
            {
                string temp = title.Value.Insert(9, requestHtml.Host);
                resultList.Add(temp);
            }
            return resultList;
        }

    }
}