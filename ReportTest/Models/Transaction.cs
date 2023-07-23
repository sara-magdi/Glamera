using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using ReportTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ReportTest.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        [ValidateNever]
        public Booking Booking { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateOnly PaymentDate { get; set; }
    }


public static class TransactionEndpoints
{
	public static void MapTransactionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Transaction").WithTags(nameof(Transaction));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Transactions.ToListAsync();
        })
        .WithName("GetAllTransactions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Transaction>, NotFound>> (int transactionid, AppDbContext db) =>
        {
            return await db.Transactions.AsNoTracking()
                .FirstOrDefaultAsync(model => model.TransactionId == transactionid)
                is Transaction model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTransactionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int transactionid, Transaction transaction, AppDbContext db) =>
        {
            var affected = await db.Transactions
                .Where(model => model.TransactionId == transactionid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.TransactionId, transaction.TransactionId)
                  .SetProperty(m => m.BookingId, transaction.BookingId)
                  .SetProperty(m => m.Amount, transaction.Amount)
                  .SetProperty(m => m.PaymentMethod, transaction.PaymentMethod)
                  .SetProperty(m => m.PaymentDate, transaction.PaymentDate)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTransaction")
        .WithOpenApi();

        group.MapPost("/", async (Transaction transaction, AppDbContext db) =>
        {
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Transaction/{transaction.TransactionId}",transaction);
        })
        .WithName("CreateTransaction")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int transactionid, AppDbContext db) =>
        {
            var affected = await db.Transactions
                .Where(model => model.TransactionId == transactionid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTransaction")
        .WithOpenApi();
    }
}}
