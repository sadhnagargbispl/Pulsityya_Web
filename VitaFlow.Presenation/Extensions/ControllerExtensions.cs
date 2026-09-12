using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace VitaFlow.Presenation.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<string> RenderRazorViewToStringAsync(
        this Controller controller,
        string viewName,
        object model)
        {
            controller.ViewData.Model = model;

            var serviceProvider = controller.HttpContext.RequestServices;
            var razorViewEngine = serviceProvider.GetRequiredService<IRazorViewEngine>();
            var tempDataProvider = serviceProvider.GetRequiredService<ITempDataProvider>();
            var metadataProvider = serviceProvider.GetRequiredService<IModelMetadataProvider>();

            using var sw = new StringWriter();

            var viewResult = razorViewEngine.FindView(controller.ControllerContext, viewName, false);

            if (!viewResult.Success)
            {
                throw new FileNotFoundException($"View '{viewName}' not found.");
            }

            var viewDictionary = new ViewDataDictionary(metadataProvider, controller.ModelState)
            {
                Model = model
            };

            var tempData = new TempDataDictionary(controller.HttpContext, tempDataProvider);

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                viewDictionary,
                tempData,
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }
}
