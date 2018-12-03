using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class VideoCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaxDigitalResolution { get; set; }
        public string MaxVGAResolution { get; set; }
        public string MemoryBit { get; set; }
        public string MemoryGBsec { get; set; }
        public string Image { get; set; }
    }
}