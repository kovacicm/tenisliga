using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;

namespace tenisLigaOmis.Pages;
    public class PrijavaModel : PageModel
    {
        private readonly AppDbContext _context;

        public PrijavaModel()
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
        public Players Igrac { get; set; }

        public IActionResult OnGet()
        {
            Igrac = new Players();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Igrac.Elo = 1500;
            Igrac.Group = 101;
            _context.Players.Add(Igrac);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index"); // Redirect to a page showing the list of products.
        }
    }
