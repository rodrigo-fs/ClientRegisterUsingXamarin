using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterNewClient.Infrastructure
{
    public interface IPCService
    {
        void Launch(string command);
    }
}
