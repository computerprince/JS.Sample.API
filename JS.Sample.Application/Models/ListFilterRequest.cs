using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.Application.Models
{
    public class ListFilterRequest
    {

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public bool IsAsc { get; set; }
        public string Filter { get; set; }

    }
}
