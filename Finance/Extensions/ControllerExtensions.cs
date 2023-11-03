using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Finance.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult(this ControllerBase controller,
                                                   HttpStatusCode code,
                                                   object? value = null,
                                                   string message = "")
        {
            if (value is null)
                return controller.StatusCode((int)code, new ErrorResponse { Code = (int)code, Message = message });

            return controller.StatusCode((int)code, value);
        }
    }

    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
