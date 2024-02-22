using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Mvc;

namespace tenisLigaOmis.Pages;

public class RezultatiPocetna : PageModel
{

    private readonly AppDbContext? _context;

    public List<Matches> Results { get; set; }
    public RezultatiPocetna()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var configuration = configurationBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

        _context = new AppDbContext(optionsBuilder.Options);
    }

    public IActionResult OnGet()
    {

        Results = _context.Matches
                        .OrderByDescending(item => item.Id)
                        .Take(10)
                        .ToList();

        return Page();

    }

    public async Task<IActionResult> OnPostAsync()
    {

        return RedirectToPage("./PrijaviRezultat");
    }

}


