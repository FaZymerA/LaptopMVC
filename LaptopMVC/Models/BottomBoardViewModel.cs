using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class BottomBoardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Socket { get; set; }
        public string ProcessorSupp { get; set; }
        public string RAMMemorySupp { get; set; }
        public string Image { get; set; }
    }
}