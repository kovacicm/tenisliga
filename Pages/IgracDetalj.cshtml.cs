using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Mvc;

namespace tenisLigaOmis.Pages;

    public class IgracDetalj : PageModel
    {

        private readonly AppDbContext? _context;

        public List<Matches> Results { get; set; }
    public IgracDetalj()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            _context = new AppDbContext(optionsBuilder.Options);
        }

        public Players Player { get; set; }

        public IActionResult OnGet(int id)
        {
            Player = _context.Players.FirstOrDefault(p => p.Id == id);

            Results = _context.Matches
                            .Where(p => p.WinnerId == id || p.LoserId == id)
                            .ToList();

            if (Player == null)
            {
                return NotFound();
            }

            return Page();
    }        


    }




