using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReportTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ReportTest.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("ClientId")]
        [ValidateNever]
        public Client Client { get; set; }
        public int ClientId { get; set; }


        [ForeignKey("BranchId")]
        [ValidateNever]
        public Branch Branch { get; set; }
        public int BranchId { get; set; }

        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public string Status { get; set; }
    }


public static class BookingEndpoints
{
	public static void MapBookingEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Booking").WithTags(nameof(Booking));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Bookings.ToListAsync();
        })
        .WithName("GetAllBookings")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Booking>, NotFound>> (int bookingid, AppDbContext db) =>
        {
            return await db.Bookings.AsNoTracking()
                .FirstOrDefaultAsync(model => model.BookingId == bookingid)
                is Booking model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookingById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int bookingid, Booking booking, AppDbContext db) =>
        {
            var affected = await db.Bookings
                .Where(model => model.BookingId == bookingid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.BookingId, booking.BookingId)
                  .SetProperty(m => m.ClientId, booking.ClientId)
                  .SetProperty(m => m.BranchId, booking.BranchId)
                  .SetProperty(m => m.BookingDate, booking.BookingDate)
                  .SetProperty(m => m.BookingTime, booking.BookingTime)
                  .SetProperty(m => m.Status, booking.Status)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBooking")
        .WithOpenApi();

        group.MapPost("/", async (Booking booking, AppDbContext db) =>
        {
            db.Bookings.Add(booking);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Booking/{booking.BookingId}",booking);
        })
        .WithName("CreateBooking")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int bookingid, AppDbContext db) =>
        {
            var affected = await db.Bookings
                .Where(model => model.BookingId == bookingid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBooking")
        .WithOpenApi();
    }
}}
