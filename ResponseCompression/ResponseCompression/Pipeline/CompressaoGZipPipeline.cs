namespace ResponseCompression.Pipeline
{
    public class CompressaoGZipPipeline
    {
        public void Configure(IApplicationBuilder app)
            => app.UseResponseCompression();
    }
}
