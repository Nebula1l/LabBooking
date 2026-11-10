using LabBooking.Data;
using LabBooking.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabBooking.Pages.Bookings;

public class IndexModel : PageModel
{
     private readonly LabBookingContext _context;
     public IndexModel(LabBookingContext context)
    {
        _context = context;
    }

    public IList<Booking> BookingList { get; set; }
        = new List<Booking>();
 public IList<Models.Equipment> EquipmentList { get; set; }
     = new List<Models.Equipment>();


    public int? EquipmentId { get; set; }
    public DateTime? FilterDate { get; set; }
    public async Task OnGetAsync(int? equipmentId, DateTime? filterDate)
    {
        EquipmentId = equipmentId;
        FilterDate = filterDate;
       EquipmentList = await _context.Equipment
            .OrderBy(e => e.Name)
            .ToListAsync();
        IQueryable<Booking> bookingQuery = _context.Bookings
            .Include(b => b.Equipment);
        if (EquipmentId.HasValue)
        {
            bookingQuery = bookingQuery.Where(b =>
                b.EquipmentId == EquipmentId.Value);
        }

        if (FilterDate.HasValue)
        {
            DateTime dayStart = FilterDate.Value.Date;
            DateTime nextDay = dayStart.AddDays(1);
            bookingQuery = bookingQuery.Where(b =>
            b.StartTime < nextDay && b.EndTime > dayStart);
        }
        BookingList = await bookingQuery
         .OrderByDescending(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostCancelAsync(int id)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == id);
        if (booking != null && !booking.IsCancelled)
        {
            booking.IsCancelled = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();

    }
}