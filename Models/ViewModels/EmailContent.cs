using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PackWalkLicenta.Models.ViewModels
{
    public class EmailContent
    {
        public int RezervareID { get; set; }
        public string Destinatar { get; set; }
        public string Subiect { get; set; }
        public string Mesaj { get; set; }
    }
}