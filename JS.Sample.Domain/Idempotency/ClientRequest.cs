using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.Domain
{
    public class ClientRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long ContentId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}
