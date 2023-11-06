using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModel.Services
{
    public interface ISqlConnectionWrapper : IDisposable
    {
        void Open();
        void Close();
        // Add other methods or properties if needed
    }
}
