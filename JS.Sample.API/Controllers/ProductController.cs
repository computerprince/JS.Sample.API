using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using JS.Sample.CommandStack;
using JS.Sample.Application.Interfaces.Queries;
using JS.Sample.Infratructure.Filters;
using JS.Sample.Common.Models;
using JS.Sample.Application.Models;

namespace JS.Sample.API.Controllers
{

    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private string nameSpace = "JS.Sample.API.Product.Controllers";

        private readonly ISampleQueries _queries;
        private readonly IMediator _mediator;
        public ProductController(IConfiguration _configuration, ILogger<ProductController> _logger, ISampleQueries queries, IMediator mediator) :
                  base(_configuration, _logger)
        {
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }



        /// <summary>
        /// Add New a Product
        /// </summary>
        /// <param name="command">The Payload Request.</param>
        /// <param name="requestId">The request identifier.</param>
        /// <response code="200">Data has been successfully created</response>
        /// <response code="201">Name already taken</response>
        /// <response code="202">The Request Already Processed</response>  
        /// <response code="203">Data has failed to create</response>  
        /// <response code="400">Bad Request</response>  
        /// <response code="500">Internal Server Error</response>  
        /// <response code="401">Unauthorized Access</response>  
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody]CreateProductCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            _logger.LogInformation("{nameSpace} - Created a new Product", nameSpace);
            CommonResponse commandResult = null;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty && command != null)
            {
                var requestTravel = new IdentifiedCommand<CreateProductCommand, CommonResponse>(command, guid);

                commandResult = await _mediator.Send(requestTravel).ConfigureAwait(false);
            }
            return commandResult != null ? Ok(commandResult) : BadRequest() as IActionResult;
        }

        /// <summary>
        /// Update the Product
        /// </summary>
        /// <param name="command">The Payload Request.</param>
        /// <param name="requestId">The request identifier.</param>
        /// <response code="200">Successful operation</response>
        /// <response code="202">The Request Already Processed</response>   
        /// <response code="203">Data has failed to update</response>  
        /// <response code="404">Id does not exist</response> 
        /// <response code="400">Bad Request</response>  
        /// <response code="500">Internal Server Error</response>  
        /// <response code="401">Unauthorized Access</response>  
        [HttpPut("{Id}")]
        [ValidateModel]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            _logger.LogInformation("{nameSpace} - Update the product", nameSpace);

            CommonResponse commandResult = null;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty && command != null)
            {
                var requestTravel = new IdentifiedCommand<UpdateProductCommand, CommonResponse>(command, guid, command.Id);

                commandResult = await _mediator.Send(requestTravel).ConfigureAwait(false);
            }
            return commandResult != null ? Ok(commandResult) : BadRequest() as IActionResult;
        }

        /// <summary>
        ///  Update the Product  Status
        /// </summary>
        /// <param name="Id">The Payload Request.</param>
        /// <param name="status">The Payload Request.</param>
        /// <param name="requestId">The request identifier.</param>
        /// <response code="200">Successful operation</response>
        /// <response code="203">Data has failed to update</response>  
        /// <response code="404">Id does not exist</response> 
        /// <response code="400">Bad Request</response>  
        /// <response code="500">Internal Server Error</response>  
        /// <response code="401">Unauthorized Access</response>  
        [HttpPut("status/{Id}/{status}")]
        public async Task<IActionResult> changestatus(long Id, bool status, [FromHeader(Name = "x-requestid")] string requestId)
        {
            _logger.LogInformation("{nameSpace} - Update a product {id}", nameSpace, Id);

            CommonResponse commandResult = null;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var command = new StatusProductCommand();
                command.Id = Id;
                command.IsAvailable = status;
                commandResult = await _mediator.Send(command).ConfigureAwait(false);
            }

            return commandResult != null ? (IActionResult)Ok(commandResult) : (IActionResult)BadRequest();
        }



        /// <summary>
        ///  Delete the Product
        /// </summary>
        /// <param name="Id">The Payload Request.</param>
        /// <param name="requestId">The request identifier.</param>
        /// <response code="200">Successful operation</response>
        /// <response code="203">Data has failed to Delete</response>  
        /// <response code="404">Id does not exist</response> 
        /// <response code="400">Bad Request</response>  
        /// <response code="500">Internal Server Error</response>  
        /// <response code="401">Unauthorized Access</response>     
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id, [FromHeader(Name = "x-requestid")] string requestId)
        {
            _logger.LogInformation("{nameSpace} - Delete product {id}", nameSpace, Id);
            CommonResponse commandResult = null;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var command = new DeleteProductCommand();
                command.Id = Id;
                commandResult = await _mediator.Send(command).ConfigureAwait(false);
            }
            return commandResult != null ? (IActionResult)Ok(commandResult) : (IActionResult)BadRequest();
        }




        /// <summary>
        /// Get the Details of Product
        /// </summary>
        /// <param name="Id">Id of Ancillary Parent</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Id does not exist</response> 
        /// <response code="400">Bad Request</response>  
        /// <response code="500">Internal Server Error</response>  
        /// <response code="401">Unauthorized Access</response>  
        [HttpGet("{Id}")]
        public async Task<IActionResult> Detail(long Id)
        {
            var commandResult = await _queries.GetProductDetail(Id);
            return commandResult != null ? Ok(commandResult) : BadRequest() as IActionResult;

        }

        /// <summary>
        /// Get list of Products
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Bad Request</response>  
        /// <response code="500">Internal Server Error</response>  
        /// <response code="401">Unauthorized Access</response>  
        [HttpGet("List")]
        public async Task<IActionResult> List([FromQuery]ListFilterRequest request)
        {
            var commandResult = await _queries.GetList(request);
            return commandResult != null ? Ok(commandResult) : BadRequest() as IActionResult;

        }

    }
}