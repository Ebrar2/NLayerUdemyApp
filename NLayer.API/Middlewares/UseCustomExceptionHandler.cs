using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NLayer.API.Middlewares
{

    //Extension metot yazabilmek için class ve metot static olmalı
    public static class UseCustomExceptionHandler
    {
        //
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
             
                //Run sonlandırıcı middlewaredır.Buraya geldikten sonra artık geri döner ileride başka middleware a gitmez
                //Exception varsa ileriye gitme
                   config.Run(async context=>
                   {
                       context.Response.ContentType = "application/json";
                       var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();  //uygulamada fırlatılan hata alınır.

                       var statusCode = exceptionFeature.Error switch
                       {
                           ClientSideException=>400,
                           NotFoundException=>404,
                           _=>500
                       };
                       context.Response.StatusCode = statusCode;
                       var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                       await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                       //normalde framework kendi sonucu json çevirir ama burada frame çalışmaz buyüzden sonucu json a çevirmemiz gerekir
                   })
            );
        }
    }
}
