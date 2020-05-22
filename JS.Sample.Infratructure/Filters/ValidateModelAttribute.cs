using Microsoft.AspNetCore.Mvc.Filters;


namespace JS.Sample.Infratructure.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// This will execute on each action executing 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
