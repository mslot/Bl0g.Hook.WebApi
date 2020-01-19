using Bl0g.Hook.WebApi.Middleware.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bl0g.Middleware
{
    public class HMACRequestValidator
    {
        private readonly RequestDelegate _next;
        private readonly HMACRequestValidatorOptions _options;

        public HMACRequestValidator(RequestDelegate next,HMACRequestValidatorOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes(_options.SecretKey);
            string content = String.Empty;

            using (var reader = new StreamReader(httpContext.Request.Body, leaveOpen: true))
            {
                content = await reader.ReadToEndAsync();
            }

            httpContext.Request.Body.Position = 0;
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);

            using (var hmsha1 = new HMACSHA1(secretKeyBytes))
            {
                byte[] hash = hmsha1.ComputeHash(contentBytes);
                string hex = BitConverter.ToString(hash).Replace("-", string.Empty);
                hex = "sha1=" + hex.ToLowerInvariant();
                httpContext.Request.Headers.TryGetValue(_options.SignatureHeaderName, out StringValues signature);

                if (!signature.Equals(hex))
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }

            }

            await _next(httpContext);
        }
    }
    public static class HMACRequestValidatorExtensions
    {
        public static IApplicationBuilder UseHMACRequestValidator(
            this IApplicationBuilder builder, HMACRequestValidatorOptions options)
        {
            return builder.UseMiddleware<HMACRequestValidator>(options);
        }
    }
}
