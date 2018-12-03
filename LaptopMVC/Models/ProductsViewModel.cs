using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public int ProcessorID { get; set; }
        public int VideoCardID { get; set; }
        public int BottomBoradID { get; set; }
        public string SystemType { get; set; }
        public string Image { get; set; }

        public string ProcessorName { get; set; }
        public string VideoCardName { get; set; }
        public string BottomBoradName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual Motherboard Motherboard { get; set; }
        public virtual Processor Processor { get; set; }
        public virtual VideoCard VideoCard { get; set; }
    }
}