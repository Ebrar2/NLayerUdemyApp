using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;

namespace NLayer.API.Filter
{
    //Filter:Controllera request geldiğinde metot çalışmadan önce veya çalıştıktan sonra return dönmeden önce döndükten sonra
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)     //hata varmı? Validate kütüphanesi contextden gelen modelstate ile entegre
            {
                var errors = context.ModelState.Values.SelectMany(X => X.Errors).Select(x => x.ErrorMessage).ToList(); //selectmany listedeki tek propertyi almayı sağlar
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors));

            }

        }
    }
}
