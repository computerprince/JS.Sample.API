using JS.Sample.Application.Interfaces.Header;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;


namespace JS.Sample.Infratructure.Services
{
    public class HeaderService : IHeaderService
    {
        private readonly IHttpContextAccessor _context;

        public HeaderService(IHttpContextAccessor context)
        {
            _context = context;

        }
        public Guid GetRequestId()
        {
            Guid.TryParse(_context?.HttpContext?.Request?.Headers["x-requestid"], out Guid guid);
            return guid;
        }
    }
}
