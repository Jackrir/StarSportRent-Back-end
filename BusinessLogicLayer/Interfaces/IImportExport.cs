using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IImportExport
    {
        Task<byte[]> Export();
        Task<bool> Import(string fileName);
    }
}
