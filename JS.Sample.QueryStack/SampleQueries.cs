using AutoMapper;
using JS.Sample.Application.Interfaces.Header;
using JS.Sample.Application.Interfaces.Queries;
using JS.Sample.Application.Models;
using JS.Sample.Application.ViewModel.Product;
using JS.Sample.Common.Extension;
using JS.Sample.Common.Models;
using JS.Sample.Domain;
using JS.Sample.Percistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JS.Sample.QueryStack
{

    public class SampleQueries : ISampleQueries
    {
        private readonly ILogger<SampleQueries> _logger;
        private readonly IMapper _mapper;
        protected readonly IHeaderService _headerService;
        protected readonly SampleDbContext _context;
        public SampleQueries(ILogger<SampleQueries> logger, IHeaderService headerService, IMapper mapper, SampleDbContext context)

        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
            _headerService = headerService;
            _context = context;
        }

        public async Task<CommonResponse> GetProductDetail(long Id)
        {
            try
            {
                Expression<Func<Product, bool>> property = p => p.Id == Id;
                var existing = await _context.Products.FirstOrDefaultAsync(property);
                if (existing == null)
                    return CommonResponse.CreateFailedResponse("No Record Found", 404);
                var model = _mapper.Map<Product, ProductViewModel>(existing);
                return CommonResponse.CreateSuccessResponse("Success", model, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CodingTest.QueryStack - Error occured at GetProductDetail");
                return CommonResponse.CreateFailedResponse(ex.Message, 500, "ProductViewModel");
            }
        }

        public async Task<CommonResponse> GetList(ListFilterRequest request)
        {
            try
            {
                if (request.PageSize <= 0) request.PageSize = 10;
                if (request.PageNumber <= 0) request.PageNumber = 1;



                var Sqlrequest = new DataRequest<Product>();
                var SqlrequestAnd = new DataRequest<Product>();
                Sqlrequest.Query = request.Filter;
                Sqlrequest.Where = x => x.Id != 0;

                if (!String.IsNullOrEmpty(Sqlrequest.Query))
                {
                    if (Sqlrequest.NumberOnly())
                    {
                        SqlrequestAnd.Where = x => x.Id == Sqlrequest.Query.ToLong();
                        Sqlrequest.Where = ListExtension.PredicateBuilder(Sqlrequest.Where, SqlrequestAnd.Where);
                    }
                    else if (Sqlrequest.lettersonly())
                    {
                        SqlrequestAnd.Where = x => x.Name.Contains(Sqlrequest.Query);
                        Sqlrequest.Where = ListExtension.PredicateBuilder(Sqlrequest.Where, SqlrequestAnd.Where);
                    }
                    else
                    {
                        SqlrequestAnd.Where = x => x.Id == Sqlrequest.Query.ToLong() || x.Name.Contains(Sqlrequest.Query);
                        Sqlrequest.Where = ListExtension.PredicateBuilder(Sqlrequest.Where, SqlrequestAnd.Where);
                    }

                }

                if (request.StartDate != null && request.EndDate != null)
                {
                    Sqlrequest.Where = x => x.ManufactureDate >= request.StartDate && x.ManufactureDate <= request.EndDate;
                    SqlrequestAnd.Where = ListExtension.PredicateBuilder(Sqlrequest.Where, SqlrequestAnd.Where);
                }

                var product = _context.Products.Where(Sqlrequest.Where);

                #region sort preparations

                if (request.IsAsc)
                {
                    switch (request.SortBy)
                    {
                        default:
                        case "0": Sqlrequest.OrderBy = r => r.Id; break;
                        case "1": Sqlrequest.OrderBy = r => r.Name; break;
                        case "2": Sqlrequest.OrderBy = r => r.Price; break;
                        case "3": Sqlrequest.OrderBy = r => r.ManufactureDate; break;
                        case "4": Sqlrequest.OrderBy = r => r.Location; break;
                        case "5": Sqlrequest.OrderBy = r => r.IsAvailable; break;

                    };

                }
                else
                {
                    switch (request.SortBy)
                    {
                        default:
                        case "0": Sqlrequest.OrderByDesc = r => r.Id; break;
                        case "1": Sqlrequest.OrderByDesc = r => r.Name; break;
                        case "2": Sqlrequest.OrderByDesc = r => r.Price; break;
                        case "3": Sqlrequest.OrderByDesc = r => r.ManufactureDate; break;
                        case "4": Sqlrequest.OrderByDesc = r => r.Location; break;
                        case "5": Sqlrequest.OrderByDesc = r => r.IsAvailable; break;

                    };


                }
                #endregion

                //Order By
                if (request.IsAsc && Sqlrequest.OrderBy != null)
                {
                    product = product.OrderBy(Sqlrequest.OrderBy);
                }

                if (!request.IsAsc && Sqlrequest.OrderByDesc != null)
                {
                    product = product.OrderByDescending(Sqlrequest.OrderByDesc);
                }

                var total = await product.CountAsync();
                if (total == 0)
                {
                    return CommonResponse.CreateSuccessResponse("No Data Available", null, 200, "");
                }
                var execute = await product.AsNoTracking().Skip(ListExtension.getPageNumber(request.PageNumber, request.PageSize))
                    .Take(request.PageSize).ToListAsync();

                var response = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(execute);


                return CommonResponse.CreatePaginationResponse(
                        "Success",
                        total,
                        response,
                        request.PageSize,
                        request.PageNumber);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching the records.");
                throw;
            }
        }
    }
}
