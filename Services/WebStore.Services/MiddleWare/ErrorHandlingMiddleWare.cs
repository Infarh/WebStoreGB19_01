using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebStore.Services.MiddleWare
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _Next;

        private static readonly log4net.ILog __Log = log4net.LogManager
            .GetLogger(typeof(ErrorHandlingMiddleWare));

        public ErrorHandlingMiddleWare(RequestDelegate Next) => _Next = Next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);
                throw;
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            __Log.Error(error.Message, error);
            return Task.CompletedTask;
        }
    }
}
