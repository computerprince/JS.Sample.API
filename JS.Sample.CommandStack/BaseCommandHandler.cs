using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;


namespace JS.Sample.CommandStack
{
    public class BaseCommandHandler
    {


        protected readonly ILogger<BaseCommandHandler> _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseCommandHandler(ILogger<BaseCommandHandler> logger, IHttpContextAccessor httpContextAccessorr)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessorr ?? throw new ArgumentNullException(nameof(httpContextAccessorr));

        }

        public BaseCommandHandler(ILogger<BaseCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
