using ResponseCompression.Middleware;

namespace ResponseCompression.FilterMiddleware
{
    public class ChecagemIndisponibilidadePipeline
    {
        public void Configure(IApplicationBuilder app)
            => app.UseMiddleware<ChecagemIndisponibilidade>();
    }
}
