using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.Application.ViewModel.Product
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }
    }
}
