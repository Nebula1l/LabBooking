using LabBooking.Data;
using LabBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabBooking.Pages.Equipment;

public class DetailsModel : PageModel
{
    private readonly LabBookingContext _context;

    public DetailsModel(LabBookingContext context)
    {
        _context = context; 
    }

public LabBooking.Models.Equipment? EquipmentItem { get; set; } 
 
    public IList<Booking> ActiveBookings { get; set; }
        = new List<Booking>();

    public async Task<IActionResult> OnGetAsync(int? id) 
    {
        if (id == null)
        {
            return NotFound();
        }

        EquipmentItem = await _context.Equipment 
            .Include(e => e.Category) 
            .Include(e => e.Bookings)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (EquipmentItem == null)
        {
            return NotFound();

         }

        ActiveBookings = EquipmentItem.Bookings
            .Where(b => !b.IsCancelled && b.EndTime > 
            DateTime.Now)
            .OrderBy(b => b.StartTime)
            .ToList(); 

        return Page(); 
  } 
}