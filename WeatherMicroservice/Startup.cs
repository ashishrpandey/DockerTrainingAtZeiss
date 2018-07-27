﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace WeatherMicroservice
{
    public static class Extensions
    {
        public static double? TryParse(this string input)
        {
            if (double.TryParse(input, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }

    public class Startup
    {

        // bool TryParse(string s, out double result);
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
		var latString = context.Request.Query["lat"].FirstOrDefault();
		var longString = context.Request.Query["long"].FirstOrDefault();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
		var latitude = latString.TryParse();
		var longitude = longString.TryParse();
                if (latitude.HasValue && longitude.HasValue)
                {
                    var forecast = new List<WeatherReport>();
                    for (var days = 1; days <= 5; days++)
                    {
                        forecast.Add(new WeatherReport(latitude.Value, longitude.Value, days));
                    }
                    var json = JsonConvert.SerializeObject(forecast, Formatting.Indented);
                    context.Response.ContentType = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(json);
                }
               
                await context.Response.WriteAsync($"Retrieving Weather for lat: {latitude}, long: {longitude}");
            });
        }
    }
}
