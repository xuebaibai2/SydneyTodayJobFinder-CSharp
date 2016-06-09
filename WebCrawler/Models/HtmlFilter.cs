using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCrawler.Models
{
    public class HtmlFilter
    {
        [Required(ErrorMessage = "Keyword cannot be empty")]
        public string Keyword { get; set; }
        public string DivId { get; set; }
        [Required(ErrorMessage = "URL cannot be empty")]
        public string URL { get; set; }
    }
}