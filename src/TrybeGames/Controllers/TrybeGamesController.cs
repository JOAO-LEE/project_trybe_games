using System.Globalization;
using System.Collections;


namespace TrybeGames;
public class TrybeGamesController
{
    public TrybeGamesDatabase database;

    public IConsole Console;

    public TrybeGamesController(TrybeGamesDatabase database, IConsole console)
    {
        this.database = database;
        this.Console = console;
    }

    public void RemovePlayerFromGame(Game game)
    {
        var playersPlayingGame = database.Players.Where(p => game.Players.Contains(p.Id)).ToList();
        var player = SelectPlayer(playersPlayingGame);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrado!");
            return;
        }
        game.RemovePlayer(player);
        Console.WriteLine("Pessoa jogadora removido com sucesso!");
    }

    public void AddPlayerToGame(Game gameToAdd)
    {
        var playersNotPlayingGame = database.Players.Where(p => !gameToAdd.Players.Contains(p.Id)).ToList();
        var player = SelectPlayer(playersNotPlayingGame);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        gameToAdd.AddPlayer(player);
        Console.WriteLine("Pessoa jogadora adicionada com sucesso!");
    }

    public void QueryGamesFromStudio()
    {
        var gameStudio = SelectGameStudio(database.GameStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Opção inválida! Tente novamente.");
            return;
        }
        try
        {
            var games = database.GetGamesDevelopedBy(gameStudio);
            Console.WriteLine("Jogos do estúdio de jogos " + gameStudio.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }
    }

    public void QueryGamesPlayedByPlayer()
    {
        var player = SelectPlayer(database.Players);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        try
        {
            var games = database.GetGamesPlayedBy(player);
            if (games.Count() == 0)
            {
                Console.WriteLine("Pessoa jogadora não jogou nenhum jogo!");
                return;
            }
            Console.WriteLine("Jogos jogados pela pessoa jogadora " + player.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }

    }

    public void QueryGamesBoughtByPlayer()
    {
        var player = SelectPlayer(database.Players);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        try
        {
            var games = database.GetGamesOwnedBy(player);
            Console.WriteLine("Jogos comprados pela pessoa jogadora " + player.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }
    }

    // 1. Crie a funcionalidde para adicionar uma nova pessoa jogadora ao banco de dados
    public void AddPlayer()
    {
        string? playerName = Console.ReadLine();
        int playerId = this.database.Players.Count() + 1;
        Player createdPlayer = new Player
        {
            Name = playerName,
            Id = playerId,
            FavoriteGameStudios = new List<int>(),
            GamesOwned = new List<int>(),
        };
        this.database.Players.Add(createdPlayer);
    }

    // 2. Crie a funcionalidade de adicionar um novo estúdio de jogos ao banco de dados
    public void AddGameStudio()
    {
        string? studioName = Console.ReadLine();
        int studioId = this.database.GameStudios.Count() + 1;
        DateTime dateOfCreation = new DateTime();
        GameStudio createdStudio = new GameStudio()
        {
            Name = studioName,
            Id = studioId,
            CreatedAt = dateOfCreation
        };
        this.database.GameStudios.Add(createdStudio);
    }

    // 3. Crie a funcionalidade de adicionar novo Jogo ao Banco de dados
    public void AddGame()
    {
        // Console.WriteLine("Digite abaixo o nome do jogo e pressione enter:");
        // string? gameName = Console.ReadLine();
        // Console.WriteLine("Digite abaixo o a data de lançamento do jogo e pressione enter (\"dd/mm/yy\"):");
        // string? gameRealeaseDate = Console.ReadLine();
        // bool dateWasConverted = DateTime.TryParseExact(gameRealeaseDate, "dd/mm/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime gameReleaseDateConverted);
        // if (!dateWasConverted)
        // {
        //     Console.WriteLine("O formato de data de lançamento é inválido! (\"dd/mm/yy\")");
        //     return;
        // }
        // Console.WriteLine("Digite abaixo o número do gênero do jogo e pressione enter:");
        // Console.WriteLine("0 - Action / 1 - Adventure / 2 - Puzzle / 3 - Strategy / 4 - Simulation / 5 - Sports / 6 - Other");
        // string? gameGenre = Console.ReadLine();
        // bool isInputANumberAndValid = int.TryParse(gameGenre, out int gameGenreNum) && Enum.IsDefined(typeof(GameType), gameGenreNum);
        // if (!isInputANumberAndValid)
        // {
        //     Console.WriteLine("O gênero do jogo é inválido! Escolha um dos abaixo");
        //     Console.WriteLine("0 - Action / 1 - Adventure / 2 - Puzzle / 3 - Strategy / 4 - Simulation / 5 - Sports / 6 - Other");
        //     return;
        // }
        // int gameId = this.database.Games.Count() + 1;
        // Game createdGame = new Game()
        // {
        //     Name = gameName,
        //     Id = gameId,
        //     ReleaseDate = gameReleaseDateConverted,
        //     GameType = (GameType)gameGenreNum,
        // };
        // this.database.Games.Add(createdGame);
    }

    public void ChangeGameStudio(Game game)
    {
        var gameStudio = SelectGameStudio(database.GameStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Opção inválida! Tente novamente.");
            return;
        }
        game.DeveloperStudio = gameStudio.Id;
        Console.WriteLine("Estúdio de jogos alterado com sucesso!");
    }

    public void AddGameStudioToFavorites(Player player)
    {
        var notFavoriteStudios = database.GameStudios.Where(s => !player.FavoriteGameStudios.Contains(s.Id)).ToList();
        var gameStudio = SelectGameStudio(notFavoriteStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Nenhum estúdio de jogos encontrado!");
            return;
        }
        player.AddGameStudioToFavorites(gameStudio);
    }

    public void BuyGame(Player player)
    {
        var gamesNotOwned = database.Games.Where(g => !player.GamesOwned.Contains(g.Id)).ToList();
        var game = SelectGame(gamesNotOwned);
        if (game == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            return;
        }
        player.BuyGame(game);
        Console.WriteLine("Jogo comprado com sucesso!");
    }

    public Player SelectPlayer(List<Player> players)
    {
        Console.WriteLine("Selecione o jogador:");
        PrintPlayers(players);
        var playerId = int.Parse(Console.ReadLine() ?? "0");
        Player? result = players.FirstOrDefault(p => p.Id == playerId);
        return result!;
    }

    public Game SelectGame(List<Game> games)
    {
        Console.WriteLine("Selecione o jogo:");
        PrintGames(games);
        var gameId = int.Parse(Console.ReadLine() ?? "0");
        return games.FirstOrDefault(g => g.Id == gameId)!;
    }

    public GameStudio SelectGameStudio(List<GameStudio> gameStudios)
    {
        Console.WriteLine("Selecione o estúdio de jogos:");
        PrintGameStudios(gameStudios);
        var gameStudioId = int.Parse(Console.ReadLine() ?? "0");
        return gameStudios.FirstOrDefault(gs => gs.Id == gameStudioId)!;
    }

    public void PrintGames(List<Game> games)
    {
        foreach (var game in games)
        {
            Console.WriteLine(game.Id + " - " + game.Name);
        }
    }

    public void PrintGameStudios(List<GameStudio> gameStudios)
    {
        foreach (var gameStudio in gameStudios)
        {
            Console.WriteLine(gameStudio.Id + " - " + gameStudio.Name);
        }
    }

    public void PrintPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            Console.WriteLine(player.Id + " - " + player.Name);
        }
    }

    public void PrintGameTypes()
    {
        Console.WriteLine("1 - Ação");
        Console.WriteLine("2 - Aventura");
        Console.WriteLine("3 - Puzzle");
        Console.WriteLine("4 - Estratégia");
        Console.WriteLine("5 - Simulação");
        Console.WriteLine("6 - Esportes");
        Console.WriteLine("7 - Outro");
    }
}