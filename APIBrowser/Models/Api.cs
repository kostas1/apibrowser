using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APIBrowser.Models
{
    public class Api
    {
        public string Key { get; set; }
        [Display(Name="Comma-separated parameters")]
        public string Parameters { get; set; }
    }
}