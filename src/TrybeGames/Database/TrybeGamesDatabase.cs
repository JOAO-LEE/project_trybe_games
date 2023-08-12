namespace TrybeGames;

public class TrybeGamesDatabase
{
    public List<Game> Games = new List<Game>();

    public List<GameStudio> GameStudios = new List<GameStudio>();

    public List<Player> Players = new List<Player>();

    public List<Game> GetGamesDevelopedBy(GameStudio gameStudio)
    {
        return (from developedGame in Games
                where developedGame.DeveloperStudio == gameStudio.Id
                select developedGame).ToList();
    }

    public List<Game> GetGamesPlayedBy(Player player)
    {
        return (from developedGame in Games
                where developedGame.Players.Contains(player.Id)
                select developedGame).ToList();
    }

    public List<Game> GetGamesOwnedBy(Player playerEntry)
    {
        return (from developedGame in Games
                where developedGame.Players.Contains(playerEntry.Id)
                select developedGame).ToList();
    }

    public List<GameWithStudio> GetGamesWithStudio()
    {
        return (from developedGame in Games
                join studio in GameStudios
                on developedGame.DeveloperStudio equals studio.Id
                select new GameWithStudio { GameName = developedGame.Name, StudioName = studio.Name, NumberOfPlayers = developedGame.Players.Count }).ToList();
    }

    public List<GameType> GetGameTypes()
    {
        //Reference from the 'Distinct' operator used below.
        // https://stackoverflow.com/questions/21807339/avoid-duplicate-in-linq
        return (from developedGame in Games
                select developedGame.GameType).Distinct().ToList();
    }

    public List<StudioGamesPlayers> GetStudiosWithGamesAndPlayers()
    {

        return (from studio in GameStudios
                join developedGames in Games
                on studio.Id equals developedGames.DeveloperStudio into gameAndStudio
                select new StudioGamesPlayers
                {
                    GameStudioName = studio.Name,
                    Games = (from games in gameAndStudio
                             select new GamePlayer
                             {
                                 GameName = games.Name,
                                 Players = (from player in Players
                                            select player).ToList()
                             }).ToList()
                }).ToList();
    }
}
