using LabBooking.Data;
using LabBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace LabBooking.Pages.Bookings;

public class CreateModel : PageModel
{
    private readonly LabBookingContext _context;

    public CreateModel(LabBookingContext context)
    {
        _context = context;
    }

    [BindProperty] public Booking BookingItem { get; set; } = new();
     public IList<Models.Equipment> EquipmentList { get; set; }
        = new List<Models.Equipment>();

    public async Task OnGetAsync(int? equipmentId)
    {
        if (equipmentId.HasValue)
        {
            BookingItem.EquipmentId = equipmentId.Value;
        }
        await LoadEquipmentAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        BookingItem.StudentName =
            BookingItem.StudentName?.Trim() ?? "";

        BookingItem.StudentGroup =
            BookingItem.StudentGroup?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(BookingItem.StudentName))
        {
            ModelState.AddModelError("BookingItem.StudentName",
                "Вкажіть ім'я студента.");
        }

        if (string.IsNullOrWhiteSpace(BookingItem.StudentGroup)) 
        {
            ModelState.AddModelError("BookingItem.StudentGroup",
                "Вкажіть навчальну групу.");
        }
  
        if (BookingItem.EquipmentId <= 0) 
        {
            ModelState.AddModelError("BookingItem.EquipmentId",
                "Оберіть пристрій.");
        }

        if (BookingItem.EndTime <= BookingItem.StartTime) 
        {
            ModelState.AddModelError("BookingItem.EndTime",
                "Час завершення має бути пізнішим за час початку.");
        }

        if (BookingItem.StartTime < DateTime.Now)
        {
            ModelState.AddModelError( "BookingItem.StartTime",    
                "Бронювання не може починатися в минулому."); 
        }


        var equipment = await _context.Equipment
            .FirstOrDefaultAsync(e => e.Id == BookingItem.EquipmentId);

        if (equipment == null)
        {
            ModelState.AddModelError(
                "BookingItem.EquipmentId",
                "Обраний пристрій не знайдено.");
        }
        else if (!equipment.IsOperational)
        {
            ModelState.AddModelError("BookingItem.EquipmentId", 
            "Несправний пристрій не можна забронювати.");
        }

        if (equipment != null &&
            BookingItem.EndTime > BookingItem.StartTime)
        {
            bool hasOverlap = await _context.Bookings
           .AnyAsync(b =>
                  b.EquipmentId == BookingItem.EquipmentId &&
                !b.IsCancelled &&
                BookingItem.StartTime < b.EndTime &&
                BookingItem.EndTime > b.StartTime);
            if (hasOverlap)
            {
                ModelState.AddModelError("", "На цей час пристрій уже заброньований.");
            }
        }

        if (!ModelState.IsValid)
        {
            await LoadEquipmentAsync();

            return Page();
        }
        _context.Bookings.Add(BookingItem);
        await _context.SaveChangesAsync();
        return RedirectToPage("/Bookings/Index");
    }
    private async Task LoadEquipmentAsync()
    {
        EquipmentList = await _context.Equipment
            .Where(e => e.IsOperational)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }
}