using Microsoft.AspNetCore.ResponseCompression;

namespace ResponseCompression.Extensions
{
    public static class ResponseCompressionExtension
    {
        public static void ConfigureResponseCompression(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options => 
                options.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });
        }
    }
}
