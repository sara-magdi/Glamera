using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ReportTest.Data;
using ReportTest.Models;
using ReportTest.Models.ViewModels;
using System.Linq;

namespace ReportTest.Models.ViewModels
{
    public class Revenue
    {
        public decimal Total { get; set; }
        public int ServiceId { get; set; }
        public int TransactionId { get; set; }
        public Branch Branch { get; set; }
        public Transaction Transaction { get; set; }   
        public Service Service { get; set; }   
        
    }
}
public static class RevenueEndpoints
{
    public static void MapRevenueEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Revenue").WithTags(nameof(BookingService));

        group.MapGet("/{id}", async Task<Results<Ok<BookingService>, NotFound>> (int BookingServiceId, AppDbContext db) =>
        {
            return await db.BookingServices.AsNoTracking()
            .Include(e => e.Service)
            .Include(e => e.Booking)
            .Include(e => e.Booking.Transactions)
                .FirstOrDefaultAsync(model => model.BookingServiceId == BookingServiceId)
                
                 
                is BookingService model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
       .WithName("GetRevenueById")
       .WithOpenApi();

    }
}