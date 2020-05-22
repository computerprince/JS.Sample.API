using AutoMapper;
using JS.Sample.Application.Interfaces;
using JS.Sample.Application.Interfaces.Header;
using JS.Sample.Percistance;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.CommandStack
{
    public class BaseRequestCommandHandler : BaseCommandHandler
    {
        protected readonly IHeaderService _headerService;
        protected readonly IMapper _mapper;
        protected readonly IRequestManager _requestManager;
        protected readonly SampleDbContext _context;
        public BaseRequestCommandHandler(ILogger<BaseCommandHandler> logger, IHeaderService headerService, IMapper mapper, IRequestManager requestManager, SampleDbContext context) : base(logger)
        {
            _headerService = headerService ?? throw new ArgumentNullException(nameof(headerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
