namespace ResponseCompression.FilterMiddleware
{
    public class CompressaoGZipPipeline
    {
        public void Configure(IApplicationBuilder app)
            => app.UseResponseCompression();
    }
}
