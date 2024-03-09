using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IServiceWithDto<Entity,Dto> where Entity : BaseEntity where Dto : class   //Sadece dto almamasının sebebi generic repositorylerde entityi belirtmemiz gerekir
    {
        Task<CustomResponseDto<Dto>> GetByIdAsync(int id);
        Task<CustomResponseDto<IEnumerable<Dto>>> GetAllAsync();
        //productRepository.Where(x=>x.id>5).OrderBy().ToListAsync();
        Task<CustomResponseDto<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression);  //burada IQueryable dönmüyoruz çünkü IQueryable üzerindne koşullar controllerda yazılıyordu
        Task<CustomResponseDto<bool>> AnyAsync(Expression<Func<Entity, bool>> expression);

        Task<CustomResponseDto<Dto>> AddAsync(Dto dto);
        Task<CustomResponseDto<IEnumerable<Dto>>> AddRangeAsync(IEnumerable<Dto> dtos);
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(Dto dto);
        Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id);
        Task<CustomResponseDto<NoContentDto>> RemoveRange(IEnumerable<int> ids);
    }
}
