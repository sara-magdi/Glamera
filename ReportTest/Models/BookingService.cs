using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using ReportTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ReportTest.Models
{
    public class BookingService
    {
        public int BookingServiceId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        [ValidateNever]
        public Booking Booking { get; set; }

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        [ValidateNever]
        public Service Service { get; set; }
        public decimal Price { get; set; }

    }


public static class BookingServiceEndpoints
{
	public static void MapBookingServiceEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/BookingService").WithTags(nameof(BookingService));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.BookingServices.ToListAsync();
        })
        .WithName("GetAllBookingServices")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<BookingService>, NotFound>> (int bookingserviceid, AppDbContext db) =>
        {
            return await db.BookingServices.AsNoTracking()
                .FirstOrDefaultAsync(model => model.BookingServiceId == bookingserviceid)
                is BookingService model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookingServiceById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int bookingserviceid, BookingService bookingService, AppDbContext db) =>
        {
            var affected = await db.BookingServices
                .Where(model => model.BookingServiceId == bookingserviceid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.BookingServiceId, bookingService.BookingServiceId)
                  .SetProperty(m => m.BookingId, bookingService.BookingId)
                  .SetProperty(m => m.ServiceId, bookingService.ServiceId)
                  .SetProperty(m => m.Price, bookingService.Price)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBookingService")
        .WithOpenApi();

        group.MapPost("/", async (BookingService bookingService, AppDbContext db) =>
        {
            db.BookingServices.Add(bookingService);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/BookingService/{bookingService.BookingServiceId}",bookingService);
        })
        .WithName("CreateBookingService")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int bookingserviceid, AppDbContext db) =>
        {
            var affected = await db.BookingServices
                .Where(model => model.BookingServiceId == bookingserviceid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBookingService")
        .WithOpenApi();
    }
}}
