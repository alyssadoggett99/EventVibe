using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using EventVibe.Models;

public class DetailsModel : PageModel
{
    private readonly EventVibeContext _context;
    public Event Event { get; set; }

    public DetailsModel(EventVibeContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Event = await _context.Events.FirstOrDefaultAsync(m => m.EventId == id);

        if (Event == null)
        {
            return NotFound();
        }
        return Page();
    }
}
