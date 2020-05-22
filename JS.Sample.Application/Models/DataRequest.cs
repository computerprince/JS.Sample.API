using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace JS.Sample.Application.Models
{
    public class DataRequest<T>
    {

        public string Query { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Expression<Func<T, bool>> Where { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public bool NumberOnly()
        {
            return Query == null ? false : Regex.IsMatch(Query, "^[0-9]+$", RegexOptions.Compiled);
        }

        public bool lettersonly()
        {
            return Query == null ? false : Regex.IsMatch(Query, @"^[a-zA-Z]+$");
        }

    }
}
