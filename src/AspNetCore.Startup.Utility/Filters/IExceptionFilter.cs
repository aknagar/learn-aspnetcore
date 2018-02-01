using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Filters
{
    public interface IExceptionFilter: IFilterMetadata
    {
        void OnException(ExceptionContext context);
    }
}
