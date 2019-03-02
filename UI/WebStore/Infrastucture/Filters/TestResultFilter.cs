using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastucture.Filters
{
    public class TestResultFilter : Attribute, IResultFilter
    {
        private ILogger _Logger;

        public TestResultFilter(ILogger logger) => _Logger = logger;

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _Logger.LogInformation($"Controller {context.Controller} invoked");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}