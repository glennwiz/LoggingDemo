using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LoggingDemo
{
  
    
    
    
    
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        //Dictionary with random string messages
        private static readonly Dictionary<int, string> messages = new();
       
        List<int> ListOfNegativeMessages = new();
        

        public Worker(ILogger<Worker> logger)
        {
            messages.Add(1, "This is a test message");
            messages.Add(2, "Server is Down");
            messages.Add(3, "Server is Up");
            messages.Add(4, "Server is Restarting");
            messages.Add(5, "Script Failed");
            messages.Add(6, "Script Succeeded");
            messages.Add(7, "Script is Running");
            messages.Add(8, "Script is Stopped");
            messages.Add(9, "Script is Paused");
            messages.Add(10, "Script is Resumed");
            messages.Add(11, "Script is Starting");
            messages.Add(12, "Script is Stopping");
            messages.Add(13, "Users was deleted");
            
            ListOfNegativeMessages.Add(2);
            ListOfNegativeMessages.Add(5);
            ListOfNegativeMessages.Add(8);

            this.logger = logger;
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.File(@"C:\test\log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:8081")
                .CreateLogger();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //set random message
                var rnd = new Random();
                var value = rnd.Next(1, 14);
                
                var message = messages[value];
                
                if(ListOfNegativeMessages.Contains(value))
                {
                    Log.Error(message);
                }
                else
                {
                    if (value == 13)
                    {
                        Log.Information("Users was deleted {UsersDeleted}", rnd.Next(1, 100));
                        
                    }
                    Log.Information(message);
                }
                
                
                
                //console write
                // Console.ForegroundColor = (ConsoleColor)new Random().Next(1, 15);
                // Console.WriteLine($"{message} - {new Random().Next(0, 100)}");
                
                
                
                
                
                //serilog
                // Log.Information("Information");
                // Log.Debug("Debug");
                // Log.Fatal("Fatal");
                // Log.Verbose("Verbose");
                
                
                
                //default logger
                // _logger.LogDebug($"{DateTime.Now} - {new Random().Next(0, 100)}");
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //
                await Task.Delay(1000, stoppingToken);
                
                //spin up a SEQ server
                //docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 8081:80 datalust/seq:latest 
                
                
                
            }
        }
    }
}
