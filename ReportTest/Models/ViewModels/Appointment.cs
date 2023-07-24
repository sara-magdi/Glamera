namespace ReportTest.Models.ViewModels
{
    public class Appointment
    {
        public int TotalNumber { get; set; }
        public Service Service { get; set; }
        public Branch Branch { get; set; }
        public Booking Booking { get; set; }

    }
}
