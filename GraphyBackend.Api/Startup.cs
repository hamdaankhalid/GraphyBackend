using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using GraphyBackend.Api.Repositories;
using GraphyBackend.Api.Config;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using Azure.Storage.Queues;

namespace GraphyBackend.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
			BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
			
			services.AddSingleton<IMongoClient>(serviceProvider => 
			{
				var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
				return new MongoClient(settings.ConnectionString);
			});

			services.AddSingleton<QueueClient>(serviceProvider => 
			{
				var azureStorageQueueConnectionString = Environment.GetEnvironmentVariable("AzureStorageQueueConnectionString");
				var queue = new QueueClient(azureStorageQueueConnectionString, "item-uploaded-queue");
				queue.CreateIfNotExists();
				if (!queue.Exists()) {
					throw new Exception("queue not initialized");
				}
				return queue;
			});

			services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
            
			services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GraphyBackend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphyBackend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

