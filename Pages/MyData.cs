
    public class Players
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Elo { get; set; }
        public int Group { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

    }

    public class Matches
    {
        public int Id { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        public int Set1winner { get; set; }
        public int Set1loser { get; set; }
        public int Set2winner { get; set; }
        public int Set2loser { get; set; }
        public int Set3winner { get; set; }
        public int Set3loser { get; set; }
        public string? Date { get; set; }
        public int GroupId { get; set; }
        public int SeasonId { get; set; }
        public int HostId { get; set; }
        public int MatchStatus { get; set; }
        public double EloWinner { get; set; }
        public double EloLoser { get; set; }
        public string? WinnerName { get; set;}
        public string? LoserName { get; set; }

    }

public class Groups
{
    public int Id { get; set; }
    public string? GroupName { get; set; }

}

public class Seasons
{
    public int Id { get; set; }
    public string? SeasonName { get; set; }

}

public class Standings
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public int GroupId { get; set; }
    public int PlayerId { get; set; }
    public string? Name { get; set; }
    public int NumMatches { get; set; }
    public int WonMatches { get; set; }
    public int GemWon { get; set; }
    public int GemLost { get; set; }
    public int SetWon { get; set; }
    public int SetLost { get; set; }
    public int Points { get; set; }
    public int Position { get; set; }

}

public class PlayerGroupModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? GroupName { get; set; }
    public double Elo { get; set; }

}