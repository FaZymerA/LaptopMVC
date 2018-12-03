using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class InfoViewModel
    {
        public List<Product> ListProducts { get; set; }
        public List<Processor> ListProcessor { get; set; }
        public List<VideoCard> ListVideoCard { get; set; }
        public List<Motherboard> ListMotherboard { get; set; }

    }
}