using Business;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            //Set Orchestration Configuration Values from appsettings.json file
            OrchestrationConfiguration.BotStockCodeToken = Configuration["BotStockCodeToken"];
            OrchestrationConfiguration.BotStockEndPoint = Configuration["BotStockEndpoint"];
            OrchestrationConfiguration.StockCommandToken = Configuration["StockCommandToken"];
            OrchestrationConfiguration.MessageManagerQueueLimit = Convert.ToInt32(Configuration["MessageManagerQueueLimit"]);
            OrchestrationConfiguration.RetrieveHistoryChatOnStartup = Convert.ToBoolean(Configuration["RetrieveHistoryChatOnStart"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AppDbConnectionString")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            

            services.AddControllersWithViews();
            services.AddRazorPages();
            //Add SignalR for Messaging
            services.AddSignalR();
            //Add bot services
            services.AddSingleton<IStockBot, StockBot>();
            //Add ChatMessage Business and Data Services
            services.AddScoped<IChatMessageBusiness, ChatMessageBusiness>();
            services.AddScoped<IChatMessageData, ChatMessageData>();
            //Add message manager service
            services.AddSingleton<IMessageManager, MessageManager>();
            services.AddHttpClient();

            
            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                //Configure SignalR Message HUB
                endpoints.MapHub<MessageController>("/Home/Index");
            });

        }
    }
}
