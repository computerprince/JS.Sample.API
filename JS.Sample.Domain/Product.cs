using JS.Sample.Common.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.Domain
{
    public class Product : Entity
    {
        public Product()
        {

        }
        public Product(string name, decimal price, DateTime manufactureDate, string location, bool isAvailable)
        {
            Name = name;
            Price = price;
            ManufactureDate = manufactureDate;
            Location = location;
            IsAvailable = isAvailable;
        }

        public void Update(string name, decimal price, DateTime manufactureDate, string location, bool isAvailable)
        {
            Name = name;
            Price = price;
            ManufactureDate = manufactureDate;
            Location = location;
            IsAvailable = isAvailable;
        }
        public virtual void Available()
        {
            IsAvailable = true;
        }
        public virtual void NotAvailable()
        {
            IsAvailable = true;
        }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }

    }
}
