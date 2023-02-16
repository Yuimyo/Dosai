using System;
using System.Text;
using Serilog;
using Serilog.Events;
using Dosai.Common.Utils;
using Dosai.Common.Videos;
using Dosai.CLT.Exceptions;
using Dosai.CLT.Commands;
using Dosai.CLT.Commands.Protocols;

namespace Dosai.CLT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File("logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                using (var videoController = new VideoController())
                {
                    var commandManager = new CommandManager();
                    commandManager.RegisterProtocol("insert", new InsertingProtocol(videoController));
                    commandManager.RegisterProtocol("change", new ChangingProtocol(videoController));
                    commandManager.RegisterProtocol("erase", new ErasingProtocol(videoController));
                    commandManager.RegisterProtocol("list", new ListingProtocol(videoController));
                    commandManager.RegisterProtocol("play", new PlayingProtocol(videoController));
                    commandManager.RegisterProtocol("pause", new PausingProtocol(videoController));
                    commandManager.RegisterProtocol("back", new BackingProtocol(videoController));
                    commandManager.RegisterProtocol("help", new HelpingProtocol(commandManager));
                    commandManager.RegisterProtocol("exit", new ExitingProtocol(commandManager));
                    commandManager.Run();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}