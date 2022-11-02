using BulkyBook.Web.Models;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace BulkyBook.Web.Extensions;

public static class ControllerExtensions
{
    public static IActionResult Error<TController>(this TController controller)
        where TController : Controller
    {
        controller.Response.Headers.CacheControl = new[] { "max-age=0", "no-cache", "no-store" };

        var viewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? controller.HttpContext.TraceIdentifier
        };
        return controller.View("Error", viewModel);
    }
}