using System.ComponentModel.DataAnnotations;
using ReportTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ReportTest.Models
{
    public class Branch
    {
        [Key]
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }


public static class BranchEndpoints
{
	public static void MapBranchEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Branch").WithTags(nameof(Branch));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Branches.ToListAsync();
        })
        .WithName("GetAllBranches")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Branch>, NotFound>> (int branchid, AppDbContext db) =>
        {
            return await db.Branches.AsNoTracking()
                .FirstOrDefaultAsync(model => model.BranchId == branchid)
                is Branch model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBranchById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int branchid, Branch branch, AppDbContext db) =>
        {
            var affected = await db.Branches
                .Where(model => model.BranchId == branchid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.BranchId, branch.BranchId)
                  .SetProperty(m => m.Name, branch.Name)
                  .SetProperty(m => m.Address, branch.Address)
                  .SetProperty(m => m.City, branch.City)
                  .SetProperty(m => m.Country, branch.Country)
                  .SetProperty(m => m.Phone, branch.Phone)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBranch")
        .WithOpenApi();

        group.MapPost("/", async (Branch branch, AppDbContext db) =>
        {
            db.Branches.Add(branch);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Branch/{branch.BranchId}",branch);
        })
        .WithName("CreateBranch")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int branchid, AppDbContext db) =>
        {
            var affected = await db.Branches
                .Where(model => model.BranchId == branchid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBranch")
        .WithOpenApi();
    }
}}
