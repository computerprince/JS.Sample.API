using JS.Sample.Common.Models;
using MediatR;
using System;

namespace JS.Sample.CommandStack
{
    public class UpdateProductCommand : IRequest<CommonResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }
    }
}
