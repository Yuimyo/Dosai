using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosai.CLT.Commands.Protocols
{
    public class ExitingProtocol : IProtocol
    {
        public string Description => "Exit the command line tool.";

        private readonly CommandManager commandManager;
        public ExitingProtocol(CommandManager commandManager)
        {
            this.commandManager = commandManager;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            Log.Information("Exiting command-line tools...");
            commandManager.ExitRunning();
        }
    }
}
