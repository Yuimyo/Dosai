using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosai.CLT.Commands.Protocols
{
    public interface IProtocol
    {
        string Description { get; }
        void Run(Dictionary<string, string> kvs);
    }
}
