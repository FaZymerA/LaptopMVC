using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class CartViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> PC_ID { get; set; }
        public Nullable<int> VideoCard_ID { get; set; }
        public Nullable<int> Processor_ID { get; set; }
        public Nullable<int> Motherboard_ID { get; set; }
        public int Orders { get; set; }
        public int AcceptedOrder { get; set; }
        public Nullable<int> PC_Orders_Count { get; set; }
        public Nullable<int> VideoCard_Order_Count { get; set; }
        public Nullable<int> Processor_Order_Cout { get; set; }
        public Nullable<int> Motherboard_Order_Count { get; set; }

        public string PCName { get; set; }
        public string PCVideoCard { get; set; }
        public string PCProcessor { get; set; }

        public string VideoCardName1 { get; set; }
        public string VideoCardMaxDigital { get; set; }
        public string VideoCardMaxAVG { get; set; }

        public string ProcessorName { get; set; }
        public string ProcessorGHz { get; set; }
        public int ProcessorCore { get; set; }

        public string MotherboardName { get; set; }
        public string MotherboardSocet { get; set; }
        public string MotherboardProcessorSupp { get; set; }

        public List<Product> ListProducts { get; set; }
        public List<Processor> ListProcessor { get; set; }
        public List<VideoCard> ListVideoCard { get; set; }
        public List<Motherboard> ListMotherboard { get; set; }

    }
}