using LabBooking.Data;
using LabBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabBooking.Pages.Equipment;

public class CreateModel : PageModel
{
    private readonly LabBookingContext _context;

    public CreateModel(LabBookingContext context)
    {
        _context = context;
    }

    [BindProperty]
public Models.Equipment EquipmentItem { get; set; } = new();

    public IList<Category> Categories { get; set; } 
        = new List<Category>();
        public async Task OnGetAsync()
    {
        await LoadCategoriesAsync(); 
    }

    public async Task<IActionResult> OnPostAsync()
    {
        EquipmentItem.Name = EquipmentItem.Name?.Trim() ?? "";
        EquipmentItem.Inventorynumber =
            EquipmentItem.Inventorynumber?.Trim() ?? "";
        EquipmentItem.Description = 
            EquipmentItem.Description?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(EquipmentItem.Name)) 
        {
            ModelState.AddModelError(
                "EquipmentItem.Name",
                "Вкажіть назву пристрою.");
         }

        if (string.IsNullOrWhiteSpace(EquipmentItem.Inventorynumber))
        {
            ModelState.AddModelError(
                "EquipmentItem.Inventorynumber",
                "Вкажіть інвентарний номер.");
        }

        if (string.IsNullOrWhiteSpace(EquipmentItem.Description))
        {
            ModelState.AddModelError(
                "EquipmentItem.Description", 
                "Вкажіть опис пристрою.");
        }


        bool categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == EquipmentItem.CategoryId);
        if (!categoryExists)
        {
            ModelState.AddModelError(
                "EquipmentItem.CategoryId", 
                "Оберіть категорію.");
        }

        bool inventorynumberExists = await _context.Equipment 
            .AnyAsync(e =>
                e.Inventorynumber == EquipmentItem.Inventorynumber);

        if (inventorynumberExists)
        {
            ModelState.AddModelError(
                "EquipmentItem.Inventorynumber", 
                "Такий інвентарний номер вже існує!");
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();

            return Page();
        }


        _context.Equipment.Add(EquipmentItem);

        await _context.SaveChangesAsync();

        return RedirectToPage("/Equipment/Details",
            new { id = EquipmentItem.Id });

    }

    private async Task LoadCategoriesAsync()
    {
        Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}