using System.ComponentModel.DataAnnotations;
using ReportTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ReportTest.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }


public static class ServiceEndpoints
{
	public static void MapServiceEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Service").WithTags(nameof(Service));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Services.ToListAsync();
        })
        .WithName("GetAllServices")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Service>, NotFound>> (int serviceid, AppDbContext db) =>
        {
            return await db.Services.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ServiceId == serviceid)
                is Service model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetServiceById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int serviceid, Service service, AppDbContext db) =>
        {
            var affected = await db.Services
                .Where(model => model.ServiceId == serviceid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ServiceId, service.ServiceId)
                  .SetProperty(m => m.Name, service.Name)
                  .SetProperty(m => m.Description, service.Description)
                  .SetProperty(m => m.Price, service.Price)
                  .SetProperty(m => m.Duration, service.Duration)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateService")
        .WithOpenApi();

        group.MapPost("/", async (Service service, AppDbContext db) =>
        {
            db.Services.Add(service);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Service/{service.ServiceId}",service);
        })
        .WithName("CreateService")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int serviceid, AppDbContext db) =>
        {
            var affected = await db.Services
                .Where(model => model.ServiceId == serviceid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteService")
        .WithOpenApi();
    }
}}
