
using Serilog;

namespace HotelListing.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //tell the pipline to use this policy means this cors which is made by us
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", 
                    b => b.AllowAnyHeader()
                          .AllowAnyOrigin()
                          .AllowAnyMethod());
            });

            //use serilog after adding nudget package from the extension.
            builder.Host.UseSerilog((ctx , lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
