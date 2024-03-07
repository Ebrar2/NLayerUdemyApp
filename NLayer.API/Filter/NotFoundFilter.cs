using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filter
{
    //filter eğer constructorında service geçiyorsa bunu program.cs e eklememiz gerekir
    //Dinamik yapı(NotFoundFilter<T> ) olduğundan sadece IAsyncActionFilter
    //Eğer bir filterın constructorında parametre geçiyorsa   [NotFoundFilter] şeklinde kullanılmaz
    // Onun yerine [ServicedFilter(typeof(NotFoundFilter<Product>)] şeklinde kullanılmalı

    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> service;
        //filtera takılmazsa next ile devam eder

        public NotFoundFilter(IService<T> _service)
        {
            service=_service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //contextden tüm request responselara erişebiliriz
            var idValue = context.ActionArguments.Values.FirstOrDefault();
            if (idValue == null)
            {
                await next.Invoke();
                return;
            }
            var id = (int)idValue;

            var anyEntity = await service.AnyAsync(x => x.Id == id);
            if (anyEntity)
            {
                await next.Invoke();
                return;
            }
            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id})  not found"));
        }
    }
}


