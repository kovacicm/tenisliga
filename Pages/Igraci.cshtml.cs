using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;

namespace tenisLigaOmis.Pages;

    public class IgraciModel : PageModel
    {

        private readonly AppDbContext? _context;
        
        public IgraciModel()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            _context = new AppDbContext(optionsBuilder.Options);
        }

        public List<Players>? Items { get; set; }
        public List<PlayerGroupModel>? PlayerGroupModel { get; set; }

    public async Task OnGetAsync()
        {
            Items = await _context.Players.ToListAsync();

            PlayerGroupModel =  (from a in _context.Players
                       join c in _context.Groups on a.Group equals c.Id
                       select new PlayerGroupModel
                       {
                           Id = a.Id,
                           Name = a.Name,
                           GroupName = c.GroupName,
                           Elo = a.Elo
                       }).OrderByDescending(x => x.Elo)
          .ToList();
    }        


    }




