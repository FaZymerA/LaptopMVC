using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class ProcessorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PoworGHz { get; set; }
        public int Core { get; set; }
        public string Image { get; set; }
    }
}