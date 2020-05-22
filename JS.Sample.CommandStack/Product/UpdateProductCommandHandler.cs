
using AutoMapper;
using JS.Sample.Application.Interfaces;
using JS.Sample.Application.Interfaces.Header;
using JS.Sample.Common.Models;
using JS.Sample.Domain;
using JS.Sample.Percistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace JS.Sample.CommandStack
{
    /// <summary>
    /// Handling for the create coupon commands from the controller
    /// </summary>
    public class UpdateProductCommandHandler : BaseRequestCommandHandler, IRequestHandler<UpdateProductCommand, CommonResponse>
    {

        /// <summary>
        /// Using DI to inject infrastructure persistence Repositories
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="couponService"></param>
        public UpdateProductCommandHandler(ILogger<BaseCommandHandler> logger, IHeaderService headerService, IMapper mapper, IRequestManager requestManager, SampleDbContext context)
            : base(logger, headerService, mapper, requestManager, context)
        {

        }

        /// <summary>
        /// Handle the command initiated by the controller
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommonResponse> Handle(UpdateProductCommand message, CancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInformation($"UpdateProductCommandHandler : Update Product");

                Expression<Func<Product, bool>> property = p => p.Id == message.Id;

                var existing = await _context.Products.FirstOrDefaultAsync(property);
                if (existing == null)
                {
                    return CommonResponse.CreateFailedResponse("No Record Found", 404);
                }

                existing.Update(message.Name, message.Price, message.ManufactureDate, message.Location, message.IsAvailable);

                _context.Update(existing);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return CommonResponse.CreateSuccessResponse("Success", "Product has been successfully Updated", 200);
                }
                else
                {
                    return CommonResponse.CreateFailedResponse("Failed", 203);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateProductCommand));
                return CommonResponse.CreateFailedResponse("Failed", 500);
            }
        }



    }

    /// <summary>
    /// Use for Idempotency in Command process
    /// </summary>
    public class UpdateProductIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateProductCommand, CommonResponse>
    {
        /// <summary>
        /// Purpose of this is to handle the duplicates
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="requestManager"></param>
        public UpdateProductIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        /// <summary>
        ///  Ignore duplicate requests for creating coupon.
        /// </summary>
        /// <returns></returns>
        protected override CommonResponse CreateResultForDuplicateRequest(CommonResponse response)
        {
            //Todo: Return Already Existing Object
            return response;
        }
    }
}
