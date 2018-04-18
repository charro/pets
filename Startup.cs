using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using pets.Models;
using pets.Controllers;

namespace pets
{
    public class Startup
    {       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PetDBContext>(opt => opt.UseInMemoryDatabase("Pets"));
            services.AddTransient<AnimalsController, AnimalsController>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
