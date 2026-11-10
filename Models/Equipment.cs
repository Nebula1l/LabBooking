namespace LabBooking.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Inventorynumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int CategoryId { get; set; }
     public Category? Category { get; set; }
     public bool IsOperational { get; set; } = true;
     public List<Booking> Bookings { get; set; } = new();
}