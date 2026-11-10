using LabBooking.Data;

using LabBooking.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LabBooking.Pages;

public class IndexModel : PageModel
{
    private readonly LabBookingContext _context;
    public IndexModel(LabBookingContext context)
    {
        _context = context; 
     }

    public IList<LabBooking.Models.Equipment> EquipmentList  
    { get; set;} = new List<LabBooking.Models.Equipment>(); 

    public IList<Category> Categories { get; set; } = new List<Category>(); 
      public string? SearchText { get; set;} 
     public int? CategoryId { get; set; } 


    public async Task OnGetAsync(string? searchText, int? categoryId)
     {
        SearchText = searchText;
        CategoryId = categoryId;

        Categories = await _context.Categories 
            .OrderBy(c => c.Name)
            .ToListAsync();

        IQueryable<Models.Equipment> equipmentQuery = _context.Equipment
    .Include(e => e.Category); 


        if (!string.IsNullOrWhiteSpace(SearchText))   
        {
            equipmentQuery = equipmentQuery.Where(e => 
                e.Name.Contains(SearchText));
        }

        if (CategoryId.HasValue)
        {
            equipmentQuery = equipmentQuery.Where(e => 
                e.CategoryId == CategoryId.Value); 
        }

        EquipmentList = await equipmentQuery 
            .OrderBy(e => e.Name) 
            .ToListAsync(); 
    }
}