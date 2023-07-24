//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using ReportTest.Data;
//using ReportTest.Models.ViewModels;

//namespace ReportTest.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AppointmentController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        [HttpGet]
//        [HttpGet("{id}")]
//        public virtual async Task<Appointment> GetById(int id)
//        {


//            try
//            {
//                result = await _db.Bookings.Select(e=>e.Status).;
//            }

//            catch (Exception ex)
//            {

//            }

//            return result;

//        }
//    }
//}
