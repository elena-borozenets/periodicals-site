using System;
using System.Web.Mvc;
using NLog;

namespace Periodicals.Exceptions
{
    public class IndexOutOfRangePeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is IndexOutOfRangeException)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/IndexOutOfRangeError.html");
                logger.Error($"Some user get IndexOutOfRangeException");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

    public class ArgumentPeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ArgumentException)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/ArgumentError.html");
                logger.Error($"Some user get ArgumentException");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

    public class NullReferencePeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is NullReferenceException)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/NullReferenceError.html");
                logger.Error($"Some user get NullReferenceException");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

    public class InvalidOperationPeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is InvalidOperationException)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/InvalidOperationError.html");
                logger.Error($"Some user get InvalidOperationException");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

    public class ArgumentNullPeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ArgumentNullException)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/ArgumentNullError.html");
                _logger.Error($"Some user get ArgumentNullException");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

    public class ArgumentOutOfRangePeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ArgumentOutOfRangeException)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/ArgumentOutOfRangeError.html");
                logger.Error($"Some user get ArgumentOutOfRangeException");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

    public class PeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is Exception)
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/Error.html");
                logger.Error($"Some user get Exception");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }

}