using Newtonsoft.Json;
using System.Globalization;

namespace ResponseCompression.Middleware
{
    public class ChecagemIndisponibilidade
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ChecagemIndisponibilidade(
            RequestDelegate next,
            IConfiguration _config)
        {
            _next = next;
            _configuration = _config;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!HorarioIndisponibilidade())
            {
                _next(httpContext); // Request Delegate passa a chamada para o próximo Middleware do pipline
            }
            else
            {
                httpContext.Response.StatusCode = 403;
                httpContext.Response.ContentType = "application/json";

                var status = new
                {
                    Codigo = 403,
                    Status = "Forbidden",
                    Mensagem = "Sistema indisponível das 00:00:00 hrs as 07:00:00 hrs"
                };

                httpContext.Response.WriteAsync(
                    JsonConvert.SerializeObject(status));
            }
        }

        private bool HorarioIndisponibilidade()
        {
            var horarioIndisponibilidadeInicio = _configuration.GetSection("HorarioIndisponibilidadeInicio").Value.Split(":");// 00:00:00
            var horarioIndisponibilidadeFim = _configuration.GetSection("HorarioIndisponibilidadeFim").Value.Split(":");// 07:00:00

            int anoAtual = DateTime.Now.Year;
            int mesAtual = DateTime.Now.Month;
            int diaAtual = DateTime.Now.Day;

            //Inicio =>
            int horaInicio = int.Parse(horarioIndisponibilidadeInicio[0]);
            int minutoInicio = int.Parse(horarioIndisponibilidadeInicio[1]);
            int segundoInicio = int.Parse(horarioIndisponibilidadeInicio[2]);

            //FIm =>
            int horaFim = int.Parse(horarioIndisponibilidadeFim[0]);
            int minutoFim = int.Parse(horarioIndisponibilidadeFim[1]);
            int segundoFim = int.Parse(horarioIndisponibilidadeFim[2]);

            var IndispInicio = new DateTime(anoAtual, mesAtual, diaAtual, horaInicio, minutoInicio, segundoInicio);
            var IndispFim = new DateTime(anoAtual, mesAtual, diaAtual, horaFim, minutoFim, segundoFim);

            var compareIncio = DateTime.Compare(DateTime.Now, IndispInicio); // >0 &&
            var compareFim = DateTime.Compare(DateTime.Now, IndispFim); // <1 &&

            return (compareIncio > 0 && compareFim < 1);
        }
    }
}
