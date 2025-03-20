using RazorLight;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Shared.Helpers
{
    public static class HtmlRenderHelper
    {
        private static readonly RazorLightEngine _razorEngine;

        static HtmlRenderHelper()
        {
            // Initialize RazorLightEngine once (thread-safe)
            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(AppContext.BaseDirectory)
                .UseMemoryCachingProvider()
                .Build();
        }

        /// <summary>
        /// Renders a Razor template into HTML from a path relative to the running assembly's directory, where the template is located.
        /// Uses culture-based naming conventions (e.g., File.es.cshtml) and falls back to default File.cshtml if no culture is provided.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to pass to the Razor template.</typeparam>
        /// <param name="templateRelativePath">The relative path to the Razor template file.</param>
        /// <param name="model">The model to be passed to the template for rendering.</param>
        /// <param name="culture">Optional culture code.</param>
        /// <returns>A string containing the rendered HTML.</returns>
        /// <exception cref="FileNotFoundException">Thrown if no template file is found.</exception>
        public static async Task<string> RenderHtmlFromFileTemplateAsync<TModel>(string templateRelativePath, TModel model, string? culture)
        {
            if (string.IsNullOrWhiteSpace(templateRelativePath))
            {
                throw new ArgumentException("Template path cannot be null or empty.", nameof(templateRelativePath));
            }

            string localizedFilePath = culture != null && culture != "en"
                ? Path.Combine(
                    Path.GetDirectoryName(templateRelativePath) ?? string.Empty,
                    $"{Path.GetFileNameWithoutExtension(templateRelativePath)}.{culture}{Path.GetExtension(templateRelativePath)}"
                  )
                : templateRelativePath;

            if (!File.Exists(Path.Combine(AppContext.BaseDirectory, localizedFilePath)))
            {
                throw new FileNotFoundException($"Template not found at path: {localizedFilePath}");
            }

            return await _razorEngine.CompileRenderAsync(localizedFilePath, model);
        }       
    }
}