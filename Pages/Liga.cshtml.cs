using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenisLigaOmis.Pages;

    public class LigaModel : PageModel
    {
        private readonly AppDbContext _context;

        public LigaModel()
        {
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            _context = new AppDbContext(optionsBuilder.Options);
        }

        [BindProperty]
        public string SelectedOption { get; set; }

        [BindProperty]
        public string SelectedOptionSeason { get; set; }

        public List<Groups> DataOptions { get; set; }

        public List<Seasons> SeasonOptions { get; set; }

        public List<Standings> DisplayedData { get; set; }
        public List<Matches> Results { get; set; }


    public async Task<IActionResult> OnGetAsync()
        {

        DataOptions = _context.Groups.Select(d => new Groups
        {
            Id = d.Id,
            GroupName = d.GroupName
        }).ToList();

        SeasonOptions = _context.Seasons.Select(d => new Seasons
        {
            Id = d.Id,
            SeasonName = d.SeasonName
        }).ToList();

        return Page();
        }

    public IActionResult OnPost()
    {

        DataOptions = _context.Groups.Select(d => new Groups
        {
            Id = d.Id,
            GroupName = d.GroupName
        }).ToList();

        SeasonOptions = _context.Seasons.Select(d => new Seasons
        {
            Id = d.Id,
            SeasonName = d.SeasonName
        }).ToList();

        // Perform a database query based on the selected option
        if ((SelectedOption != null) && (SelectedOptionSeason != null)) { 
        int selectedId = int.Parse(SelectedOption);
        int selectedIdSeason = int.Parse(SelectedOptionSeason);
        DisplayedData = _context.Standings
            .Where(d => (d.GroupId == selectedId) && (d.SeasonId == selectedIdSeason))
            .OrderByDescending(d => d.Points)
            .ThenByDescending(d => d.NumMatches)
            .ThenByDescending(d => (double)d.SetWon/((double)d.SetWon+(double)d.SetLost))
            .ThenByDescending(d => (double)d.GemWon / ((double)d.GemWon + (double)d.GemLost))
            .ToList();
        Results = _context.Matches
            .Where(d => (d.GroupId == selectedId) && (d.SeasonId == selectedIdSeason))
            .ToList();
        }

        return Page();
    }


}

