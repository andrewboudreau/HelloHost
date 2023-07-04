using Microsoft.Extensions.Hosting;
using Serilog;
using System;

var host = Host.CreateDefaultBuilder()
    .UseSerilog() // Add Serilog
    .Build();

host.Start();

Console.WriteLine("Hello, Host.");
