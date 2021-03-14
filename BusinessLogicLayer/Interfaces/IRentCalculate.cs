using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRentCalculate
    {
        Task<decimal> Calculate(int rentId, IRepository repository);
    }
}
