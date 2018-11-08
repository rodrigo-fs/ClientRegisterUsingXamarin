using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegisterNewClient.Infrastructure
{
    public interface IShare
    {
        void Share(string filePath);
        void Open(string filePath);
        Task<string> Search();
    }
}
