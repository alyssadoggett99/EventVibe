using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EventVibe.Models; 

public class CreateModel : PageModel
{
    private readonly EventVibeContext _context;

    [BindProperty]
    public Event Event { get; set; }

    public CreateModel(EventVibeContext context)
    {
        _context = context;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Events.Add(Event);
        _context.SaveChanges();

        return RedirectToPage("./Index");
    }
}
