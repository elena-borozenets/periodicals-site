using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace Periodicals.Exceptions
{
    public class IndexOutOfRangePeriodicalsException : FilterAttribute, IExceptionFilter
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            /*logger.Trace("trace message");
            logger.Debug("debug message");
            logger.Info("info message");
            logger.Warn("warn message");
            logger.Error("error message");
            logger.Fatal("fatal message");*/

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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ArgumentException) //Catch our exc
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/ArgumentError.html"); // User get that page
                logger.Error($"Some user get ArgumentException");
                exceptionContext.ExceptionHandled = true; // exc handled, huray
            }
        }
    }

    public class NullReferencePeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is NullReferenceException) //Catch our exc
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/NullReferenceError.html"); // User get that page
                logger.Error($"Some user get NullReferenceException");
                exceptionContext.ExceptionHandled = true; // exc handled, huray
            }
        }
    }

    public class InvalidOperationPeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is InvalidOperationException) //Catch our exc
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/InvalidOperationError.html"); // User get that page
                logger.Error($"Some user get InvalidOperationException");
                exceptionContext.ExceptionHandled = true; // exc handled, huray
            }
        }
    }

    public class ArgumentNullPeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ArgumentNullException) //Catch our exc
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/ArgumentNullError.html"); // User get that page
                logger.Error($"Some user get ArgumentNullException");
                exceptionContext.ExceptionHandled = true; // exc handled, huray
            }
        }
    }

    public class ArgumentOutOfRangePeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is ArgumentOutOfRangeException) //Catch our exc
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/ArgumentOutOfRangeError.html"); // User get that page
                logger.Error($"Some user get ArgumentOutOfRangeException");
                exceptionContext.ExceptionHandled = true; // exc handled, huray
            }
        }
    }

    public class PeriodicalsException : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is Exception) //Catch our exc
            {
                exceptionContext.Result = new RedirectResult("~/Content/ErrorsInfo/Error.html"); // User get that page
                logger.Error($"Some user get Exception");
                exceptionContext.ExceptionHandled = true; // exc handled, huray
            }
        }
    }

}