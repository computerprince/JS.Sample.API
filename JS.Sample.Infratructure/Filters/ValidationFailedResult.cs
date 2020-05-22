using JS.Sample.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace JS.Sample.Infratructure.Filters
{
    /// <summary>
    /// custom IActionResult to return validation errors
    /// </summary>
    public class ValidationFailedResult : ObjectResult
    {
        /// <summary>
        /// Set the status code and create validation error model
        /// </summary>
        /// <param name="modelState"></param>
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new CommonResponse(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
