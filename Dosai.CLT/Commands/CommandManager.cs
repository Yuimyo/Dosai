using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Dosai.CLT.Commands.Protocols;
using Dosai.CLT.Exceptions;
using Dosai.Common.Videos;
using OpenQA.Selenium;

namespace Dosai.CLT.Commands
{
    public class CommandManager
    {
        private readonly Dictionary<string, IProtocol> protocols = new Dictionary<string, IProtocol>();
   
        public CommandManager() 
        {
        } 

        public void RegisterProtocol(string order, IProtocol protocol)
        {
            protocols[order] = protocol;
        }

        private bool exiting = false;
        public void ExitRunning() => exiting = true;
        public void Run()
        {
            Log.Information("Accepting input. Writing \"help\" will give you information about the kind of input it accepts.");
            while (!exiting)
            {
                try
                {
                    string? input = Console.ReadLine();
                    Log.Information($"Command entered: \"{input}\"");
                    string line = input ?? string.Empty;
                    var (order, kvs) = parse(line);
                    runProtocol(order, kvs);
                }
                catch(InvalidCommandException ex)
                {
                    Log.Warning("The invalid command has entered.");
                }
            }
            exiting = false;
        }

        private (string Order, Dictionary<string, string> KeyValues) parse(string line)
        {
            string[] values = splitCommandLine(line).ToArray();
            if (values.Length == 0)
                throw new InvalidCommandException();

            string order = values[0];
            var kvs = new Dictionary<string, string>();
            for (int i = 1; i < values.Length;)
            {
                // key-value.
                if (i + 1 >= values.Length)
                    throw new InvalidCommandException();
                var key = values[i] ?? string.Empty;
                var value = values[i + 1] ?? string.Empty;
                if (key == string.Empty)
                    throw new InvalidCommandException();
                kvs[key] = value;
                i += 2;
            }
            return (order, kvs);
        }

        private static IEnumerable<string> splitCommandLine(string commandLine)
        {
            bool inQuotes = false;

            return split(commandLine, c =>
            {
                if (c == '\"')
                    inQuotes = !inQuotes;

                return !inQuotes && c == ' ';
            })
                .Select(arg => arg.Trim().Replace("\"", ""))
                .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static IEnumerable<string> split(string str, Func<char, bool> controller)
        {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }


        private void runProtocol(string order, Dictionary<string, string> kvs)
        {
            if (!protocols.ContainsKey(order))
                throw new InvalidCommandException();
            protocols[order].Run(kvs);
        }

        public IEnumerable<ProtocolDetail> GetProtocolDetailAll()
        {
            foreach (var (order, protocol) in protocols)
                yield return new ProtocolDetail()
                {
                    Order = order,
                    Description = protocol.Description,
                };
        }
    }
}
