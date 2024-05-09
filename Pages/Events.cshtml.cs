using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventVibe.Models; 
using System.Linq;
using System.Threading.Tasks;

public class EventsModel : PageModel
{
    private readonly EventVibeContext _context;
    public int PageSize = 10;
    public PaginatedList<Event> Events { get; set; }

    [BindProperty(SupportsGet = true)]
    public string CurrentFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string CurrentSort { get; set; }

    public int CurrentPage { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public EventsModel(EventVibeContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync(int? pageNumber, string searchString, string sortOrder)
    {
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = CurrentFilter;
        }

        CurrentFilter = searchString;
        CurrentSort = sortOrder;

        IQueryable<Event> eventQuery = from e in _context.Events select e;

        if (!string.IsNullOrEmpty(searchString))
        {
            eventQuery = eventQuery.Where(e => e.EventName.Contains(searchString));
        }

        switch (sortOrder)
        {
            case "name_desc":
                eventQuery = eventQuery.OrderByDescending(e => e.EventName);
                break;
            case "name_asc":
                eventQuery = eventQuery.OrderBy(e => e.EventName);
                break;
            default:
                eventQuery = eventQuery.OrderBy(e => e.EventName); // Default sort
                break;
        }

        CurrentPage = pageNumber ?? 1;
        int totalItemCount = await eventQuery.CountAsync();
        Events = await PaginatedList<Event>.CreateAsync(eventQuery.AsNoTracking(), CurrentPage, PageSize);
    }
}

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        this.AddRange(items);
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
