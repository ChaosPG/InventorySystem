using System;
using InventorySystem.Areas.Identity.Data;
using InventorySystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(InventorySystem.Areas.Identity.IdentityHostingStartup))]
namespace InventorySystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<InventorySystemContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("InventorySystemContextConnection")));

                services.AddDefaultIdentity<InventorySystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<InventorySystemContext>();
            });
        }
    }
}