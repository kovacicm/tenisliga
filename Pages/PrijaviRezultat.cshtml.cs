using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
//using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

public class PrijaviRezultatModel : PageModel
{
    private readonly AppDbContext _context;

    public PrijaviRezultatModel()
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
    public int SelectedGroup { get; set; }

    [BindProperty]
    public int SelectedPlayer1 { get; set; }

    [BindProperty]
    public int SelectedPlayer2 { get; set; }

    [BindProperty]
    public int Player1Set1 { get; set; }

    [BindProperty]
    public int Player2Set1 { get; set; }

    [BindProperty]
    public int Player1Set2 { get; set; }

    [BindProperty]
    public int Player2Set2 { get; set; }

    [BindProperty]
    public int Player1Set3 { get; set; }

    [BindProperty]
    public int Player2Set3 { get; set; }

    [BindProperty]
    public bool Friendly { get; set; }

    [BindProperty]
    public bool Predaja { get; set; }

    [BindProperty]
    public string Sifra { get; set; }

    [BindProperty]
    public DateTime MatchDate { get; set; }

    public List<Groups> Groups { get; set; }
    public List<Players> Players { get; set; }
    public Matches Match { get; set; }
    public List<Seasons> Season { get; set; }

    public async Task OnGetAsync()
    {

        // Retrieve groups from the database
        Groups = await _context.Groups.ToListAsync();

        Players = await _context.Players.ToListAsync();
            // You'll need a similar query to retrieve players based on the selected group.
            // Make sure to populate the Players property accordingly.
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        double trueProb, newElo1, newElo2, est1, est2;
        int sumGames, setLost;
        double K1, K2;
        int played;
        int numMatch1, numMatch2;
        setLost = 0;
        var valid = true;
        int group1, group2;
        int season;
        int currentSeason = 4;

        Match = new Matches();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        season = _context.Seasons.Max(u => u.Id);

        group1 = _context.Players
                           .Where(u => u.Id == SelectedPlayer1)
                           .Select(u => u.Group)
                           .SingleOrDefault();
        group2 = _context.Players
                           .Where(u => u.Id == SelectedPlayer2)
                           .Select(u => u.Group)
                           .SingleOrDefault();
        numMatch1 = _context.Matches
                            .Where(u => (u.WinnerId == SelectedPlayer1 || u.LoserId == SelectedPlayer1) && u.MatchStatus == 1)
                            .Count();
        numMatch2 = _context.Matches
                            .Where(u => (u.WinnerId == SelectedPlayer2 || u.LoserId == SelectedPlayer2) && u.MatchStatus == 1)
                            .Count();

        played = _context.Matches
                            .Where(u => ((u.WinnerId == SelectedPlayer1 && u.LoserId == SelectedPlayer2) || (u.WinnerId == SelectedPlayer2 && u.LoserId == SelectedPlayer1)) && u.MatchStatus == 1 && u.SeasonId == season)
                            .Count();

        if (played > 0)
        {
            valid = false;
        }

        if ((group1 != group2) && (Friendly != true))
        {
            valid = false;
        }

        if (Predaja == false) { 
            if (Player1Set1 > 7 || Player1Set2 > 7 || Player2Set1 > 7 || Player2Set2 > 7 || Player1Set3 > 10 || Player2Set3 > 10) {
                valid = false;
            }

            if (Player1Set3 < Player2Set3)
            {
                valid = false;
            }
        }
        if (group1 == group2)
        {
            Match.GroupId = group1;
        } else
        {
            Match.GroupId = 100;
        }


        if ((Sifra == "21310") && (valid == true))
        {
            Match.EloWinner = _context
                           .Players
                           .Where(u => u.Id == SelectedPlayer1)
                           .Select(u => u.Elo)
                           .SingleOrDefault();
            Match.EloLoser = _context
                           .Players
                           .Where(u => u.Id == SelectedPlayer2)
                           .Select(u => u.Elo)
                           .SingleOrDefault();
            Match.Set1winner = Player1Set1;
            Match.Set2winner = Player1Set2;
            Match.Set3winner = Player1Set3;

            Match.Set1loser = Player2Set1;
            Match.Set2loser = Player2Set2;
            Match.Set3loser = Player2Set3;

            Match.Date = MatchDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture); 

            Match.MatchStatus = 1;
            if (Friendly == true)
            {
                Match.MatchStatus = 3;
            }
            if (Predaja == true)
            {
                Match.MatchStatus = 4;
            }

            newElo1 = Match.EloWinner;
            newElo2 = Match.EloLoser;

            //elocalc
            if ((Predaja == false) && (Friendly == false))
            {
                est1 = 1.0 / (1 + Math.Pow(10.0, (newElo2 - newElo1) / 400.0));
                est2 = 1.0 / (1 + Math.Pow(10.0, (newElo1 - newElo2) / 400.0));
                sumGames = Player1Set1 + Player1Set2 + Player2Set1 + Player2Set2;
                trueProb = 0.5;

                K1 = 300;
                K2 = 300;
                K1 = K1 - numMatch1 * 2.5;
                K2 = K2 - numMatch2 * 2.5;
                if (K1 < 50)
                {
                    K1 = 50;
                }
                if (K2 < 50)
                {
                    K2 = 50;
                }

                if ((Player1Set3 > 0) || (Player2Set3 > 0))
                {
                    trueProb = 0.5;
                    setLost = 1;
                }
                else
                {
                    if (sumGames < 13)
                    {
                        trueProb = 1.0;
                    }
                    else if (sumGames < 14)
                    {
                        trueProb = 0.95;
                    }
                    else if (sumGames < 15)
                    {
                        trueProb = 0.9;
                    }
                    else if (sumGames < 17)
                    {
                        trueProb = 0.85;
                    }
                    else if (sumGames < 19)
                    {
                        trueProb = 0.8;
                    }
                    else if (sumGames < 21)
                    {
                        trueProb = 0.75;
                    }
                    else if (sumGames < 23)
                    {
                        trueProb = 0.70;
                    }
                    else if (sumGames < 25)
                    {
                        trueProb = 0.60;
                    }
                    else if (sumGames < 27)
                    {
                        trueProb = 0.55;
                    }
                }

                //drugi igrac nepouzdan, update samo drugog
                if((numMatch2<5) && (numMatch1>5)) {
                    newElo2 = newElo2 + K2 * (1 - trueProb - est2); 
                }
                //prvi igrac nepozdan, update samo prvog
                else if ((numMatch2 > 5) && (numMatch1 < 5))
                {
                    newElo1 = newElo1 + K1 * (trueProb - est1);
                }
                else
                {
                    newElo1 = newElo1 + K1 * (trueProb - est1);
                    newElo2 = newElo2 + K2 * (1 - trueProb - est2);
                }

                newElo1 = Math.Round(newElo1);
                newElo2 = Math.Round(newElo2);

                var result = _context.Players.SingleOrDefault(b => b.Id == SelectedPlayer1);
                if (result != null)
                {
                    result.Elo = newElo1;
                    _context.SaveChanges();
                }
                var result2 = _context.Players.SingleOrDefault(b => b.Id == SelectedPlayer2);
                if (result2 != null)
                {
                    result2.Elo = newElo2;
                    _context.SaveChanges();
                }

                var result3 = _context.Standings.SingleOrDefault(b => (b.PlayerId == SelectedPlayer1) && (b.SeasonId == currentSeason));
                if (result3 != null)
                {
                    result3.Points = result3.Points + 3;
                    result3.NumMatches = result3.NumMatches + 1;
                    result3.WonMatches = result3.WonMatches + 1;
                    result3.GemWon = result3.GemWon + Player1Set1 + Player1Set2;
                    result3.GemLost = result3.GemLost + Player2Set1 + Player2Set2;
                    result3.SetWon = result3.SetWon + 2;
                    result3.SetLost = result3.SetLost + setLost;
                    _context.SaveChanges();
                }

                var result4 = _context.Standings.SingleOrDefault(b => (b.PlayerId == SelectedPlayer2) && (b.SeasonId == currentSeason));
                if (result4 != null)
                {
                    result4.Points = result4.Points + 1;
                    result4.NumMatches = result4.NumMatches + 1;
                    result4.GemWon = result4.GemWon + Player2Set1 + Player2Set2;
                    result4.GemLost = result4.GemLost + Player1Set1 + Player1Set2;
                    result4.SetWon = result4.SetWon + setLost;
                    result4.SetLost = result4.SetLost + 2;
                    _context.SaveChanges();
                }

            }

            if (Predaja == true) {

                var result3 = _context.Standings.SingleOrDefault(b => (b.PlayerId == SelectedPlayer1) && (b.SeasonId == currentSeason));
                if (result3 != null)
                {
                    result3.Points = result3.Points + 3;
                    result3.NumMatches = result3.NumMatches + 1;
                    result3.WonMatches = result3.WonMatches + 1;
                    result3.SetWon = result3.SetWon + 2;
                    _context.SaveChanges();
                }

                var result4 = _context.Standings.SingleOrDefault(b => (b.PlayerId == SelectedPlayer2) && (b.SeasonId == currentSeason));
                if (result4 != null)
                {
                    result4.SetLost = result4.SetLost + 2;
                    _context.SaveChanges();
                }

            }
            Match.SeasonId = currentSeason;
            Match.HostId = SelectedPlayer1;
            Match.WinnerId = SelectedPlayer1;
            Match.LoserId = SelectedPlayer2;
            Match.WinnerName = _context
                           .Players
                           .Where(u => u.Id == SelectedPlayer1)
                           .Select(u => u.Name)
                           .SingleOrDefault();
            Match.LoserName = _context
                           .Players
                           .Where(u => u.Id == SelectedPlayer2)
                           .Select(u => u.Name)
                           .SingleOrDefault();
            _context.Matches.Add(Match);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Uspjeh"); // Redirect to a success page.
            
        } else
        {
            return RedirectToPage("./Neuspjeh"); // Redirect to a success page.
        }

        

        // Handle form submission here, including database updates.
        // You can use the values in SelectedGroup, SelectedPlayer1, SelectedPlayer2, Player1GamesWon, Player2GamesWon, and MatchDate.

        
    }


}
