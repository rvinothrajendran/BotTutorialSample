// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio MyLuisSample v4.6.2

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LuisSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
