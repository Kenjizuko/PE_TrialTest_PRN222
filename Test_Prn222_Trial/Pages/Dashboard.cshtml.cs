using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Test_Prn222_Trial.Models;
using Microsoft.AspNetCore.SignalR;

namespace Test_Prn222_Trial.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly Sp25PharmaceuticalDbContext _context;
        private readonly IHubContext<SignalrServer> _hubContext;

        public DashboardModel(Sp25PharmaceuticalDbContext context, IHubContext<SignalrServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public List<MedicineInformation> Medicines { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex;

            var query = _context.MedicineInformations
                .Include(m => m.Manufacturer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(m => m.ActiveIngredients.Contains(SearchQuery) ||
                                          m.ExpirationDate.Contains(SearchQuery) ||
                                          m.WarningsAndPrecautions.Contains(SearchQuery));
            }

            var totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / 3.0);

            Medicines = await query
                .OrderBy(m => m.MedicineId)
                .Skip((PageIndex - 1) * 3)
                .Take(3)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string medicineId)
        {
            var medicine = await _context.MedicineInformations.FindAsync(medicineId);
            if (medicine != null)
            {
                _context.MedicineInformations.Remove(medicine);

                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ItemDeleted", medicineId);
            }

            return RedirectToPage();
        }
    }
}
