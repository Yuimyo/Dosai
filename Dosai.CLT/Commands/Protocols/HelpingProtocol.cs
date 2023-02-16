using Serilog;
using Dosai.CLT.Commands.Utils;
using Dosai.Common.Utils;
using Dosai.Common.Videos;
using System.Text;

namespace Dosai.CLT.Commands.Protocols
{
    internal class HelpingProtocol : IProtocol
    {
        public string Description => "Displays help about command line tools (currently only displays information about available commands).";

        private readonly CommandManager commandManager;
        public HelpingProtocol(CommandManager commandManager) 
        {
            this.commandManager = commandManager;
        }

        public void Run(Dictionary<string, string> kvs)
        {
            // TODO: 各プロトコル毎の必要引数に関する情報の表示を実装する。
            var message = new StringBuilder();
            foreach (var d in commandManager.GetProtocolDetailAll())
                message.AppendLine($" \"{d.Order}\": {d.Description}");
            Log.Information(message.ToString());
        }
    }
}