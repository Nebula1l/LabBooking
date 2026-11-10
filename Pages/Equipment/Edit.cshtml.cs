using LabBooking.Data;
using LabBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabBooking.Pages.Equipment;


public class EditModel : PageModel
{
    private readonly LabBookingContext _context;

    public EditModel(LabBookingContext context)
    {
        _context = context;
    }

    [BindProperty]
public Models.Equipment EquipmentItem { get; set; } = new();

public IList<Category> Categories { get; set; }
        = new List<Category>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var equipment = await _context.Equipment
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipment == null)
        {
            return NotFound();
        }

        EquipmentItem = equipment;

        await LoadCategoriesAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        EquipmentItem.Name = EquipmentItem.Name?.Trim() ?? "";
        EquipmentItem.Inventorynumber =
            EquipmentItem.Inventorynumber?.Trim() ?? "";
        EquipmentItem.Description =
            EquipmentItem.Description?.Trim() ?? "";

       var equipmentFromDatabase = await _context.Equipment
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipmentFromDatabase == null)
        {
            return NotFound();
        }

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
                e.Inventorynumber == EquipmentItem.Inventorynumber && 
                e.Id != id);

        if (inventorynumberExists)
        {
        ModelState.AddModelError(
                "EquipmentItem.Inventorynumber",
                "Такий інвентарний номер уже існує.");
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return Page();
        }

        equipmentFromDatabase.Name = EquipmentItem.Name;
        equipmentFromDatabase.Inventorynumber = 

            EquipmentItem.Inventorynumber;
        equipmentFromDatabase.Description = 

            EquipmentItem.Description;
        equipmentFromDatabase.CategoryId = 
            EquipmentItem.CategoryId;

        equipmentFromDatabase.IsOperational = 
            EquipmentItem.IsOperational;

        await _context.SaveChangesAsync();
        return RedirectToPage("/Equipment/Details",
            new { id = id });
    }

    private async Task LoadCategoriesAsync()
    {
        Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}