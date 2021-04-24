using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace amount_in_words.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new ObjectResult(new { Error = context.Exception.Message })
                {
                    StatusCode = 500,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
