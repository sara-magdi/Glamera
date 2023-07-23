using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using ReportTest.Data;
using Microsoft.EntityFrameworkCore;

namespace ReportTest.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateOnly Birthdate { get; set; }

    }


public static class ClientEndpoints
{
	public static void MapClientEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Client").WithTags(nameof(Client));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Clients.ToListAsync();
        })
        .WithName("GetAllClients")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Client>, NotFound>> (int clientid, AppDbContext db) =>
        {
            return await db.Clients.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ClientId == clientid)
                is Client model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClientById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int clientid, Client client, AppDbContext db) =>
        {
            var affected = await db.Clients
                .Where(model => model.ClientId == clientid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ClientId, client.ClientId)
                  .SetProperty(m => m.FirstName, client.FirstName)
                  .SetProperty(m => m.LastName, client.LastName)
                  .SetProperty(m => m.Gender, client.Gender)
                  .SetProperty(m => m.Email, client.Email)
                  .SetProperty(m => m.Phone, client.Phone)
                  .SetProperty(m => m.Address, client.Address)
                  .SetProperty(m => m.City, client.City)
                  .SetProperty(m => m.Country, client.Country)
                  .SetProperty(m => m.Birthdate, client.Birthdate)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateClient")
        .WithOpenApi();

        group.MapPost("/", async (Client client, AppDbContext db) =>
        {
            db.Clients.Add(client);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Client/{client.ClientId}",client);
        })
        .WithName("CreateClient")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int clientid, AppDbContext db) =>
        {
            var affected = await db.Clients
                .Where(model => model.ClientId == clientid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteClient")
        .WithOpenApi();
    }
}

}
