using AutoMapper;
using JS.Sample.Application.Interfaces;
using JS.Sample.Application.Interfaces.Header;
using JS.Sample.Common.Models;
using JS.Sample.Percistance;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JS.Sample.CommandStack
{
    /// <summary>
    /// Handling for the create coupon commands from the controller
    /// </summary>
    public class CreateProductCommandHandler : BaseRequestCommandHandler, IRequestHandler<CreateProductCommand, CommonResponse>
    {

        /// <summary>
        /// Using DI to inject infrastructure persistence Repositories
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="couponService"></param>
        public CreateProductCommandHandler(ILogger<BaseCommandHandler> logger, IHeaderService headerService, IMapper mapper, IRequestManager requestManager, SampleDbContext context)
            : base(logger, headerService, mapper, requestManager, context)
        {

        }

        /// <summary>
        /// Handle the command initiated by the controller
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommonResponse> Handle(CreateProductCommand message, CancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInformation($"CreateProductCommandHandler : Create New Product");

                var Product = new Domain.Product(message.Name, message.Price, message.ManufactureDate, message.Location, message.IsAvailable);
                _context.Products.Add(Product);

                var result = await _context.SaveEntitiesAsync();
                if (result)
                {
                    await _requestManager.UpdateRequest(_headerService.GetRequestId(), Product.Id);
                    return CommonResponse.CreateSuccessResponse("Success", "Product has been successfully created", 200);
                }
                else
                {
                    return CommonResponse.CreateFailedResponse("Failed", 203);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateProductCommand));
                return CommonResponse.CreateFailedResponse("Failed", 500);
            }
        }



    }

    /// <summary>
    /// Use for Idempotency in Command process
    /// </summary>
    public class CreateProductIdentifiedCommandHandler : IdentifiedCommandHandler<CreateProductCommand, CommonResponse>
    {
        /// <summary>
        /// Purpose of this is to handle the duplicates
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="requestManager"></param>
        public CreateProductIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
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
