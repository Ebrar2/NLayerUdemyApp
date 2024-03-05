using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //bunları implemente edince DbContext in SaveChange ve SaveChangeAsync fonk çağrıyor olucak
        Task CommitAsync();
        void Commit();
    }
}
