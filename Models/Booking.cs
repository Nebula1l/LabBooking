namespace LabBooking.Models;

public class Booking
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    public string StudentName { get; set; } = string.Empty;

    public string StudentGroup { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
     public bool IsCancelled { get; set; } = false;
}