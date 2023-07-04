using Microsoft.Extensions.Hosting;
using System;

var host = Host.CreateDefaultBuilder().Build();

host.Start();

Console.WriteLine("Hello, Host.");
