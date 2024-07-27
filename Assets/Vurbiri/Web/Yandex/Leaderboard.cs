public class Leaderboard
{ 
    public int UserRank { get; set; }
    public LeaderboardRecord[] Table { get; }

    public Leaderboard(int userRank, LeaderboardRecord[] table)
    {
        UserRank = userRank;
        Table = table;
    }
}

public class LeaderboardRecord : PlayerRecord
{
    public string Name { get; }
    public string AvatarURL { get; }

    public LeaderboardRecord(int rank, long score, string name, string avatarURL)  : base(rank, score)
    {
        Name = name;
        AvatarURL = avatarURL;
    }
}

public class PlayerRecord
{
    public int Rank { get; }
    public long Score { get; }

    public PlayerRecord(int rank, long score)
    {
        Rank = rank;
        Score = score;
    }
}
