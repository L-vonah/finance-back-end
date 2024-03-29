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
                return controller.StatusCode((int)code, new ErrorResponse(code, message));

            return controller.StatusCode((int)code, value);
        }
    }

    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;

        public ErrorResponse(HttpStatusCode code, string message)
        {
            Code = (int)code;
            Message = string.IsNullOrEmpty(message) ? GetErrorMessage(code) : message;
        }

        private static string GetErrorMessage(HttpStatusCode code)
        {
            return code switch
            {
                HttpStatusCode.BadRequest => "The request is invalid.",
                HttpStatusCode.Unauthorized => "You are not authorized to access this resource.",
                HttpStatusCode.Forbidden => "You are not allowed to access this resource.",
                HttpStatusCode.NotFound => "The resource was not found.",
                HttpStatusCode.InternalServerError => "An error occurred while processing your request.",
                _ => "An error occurred."
            };
        }
    }
}
