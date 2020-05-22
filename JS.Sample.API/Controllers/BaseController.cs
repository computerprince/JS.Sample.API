using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JS.Sample.API.Controllers
{
    public class BaseController<T> : Controller
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<T> _logger;

        public BaseController(IConfiguration configuration, ILogger<T> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


    }
}