using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_17._05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new GameContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var games = new[]
                {
                    new Game { Name = "Game 1", Studio = "Studio A", Style = "Style1", ReleaseDate = new DateTime(2020, 1, 1), GameMode = "Single-player", CopiesSold = 1000000 },
                    new Game { Name = "Game 2", Studio = "Studio B", Style = "Style2", ReleaseDate = new DateTime(2021, 5, 20), GameMode = "Multi-player", CopiesSold = 500000 },
                    new Game { Name = "Game 3", Studio = "Studio C", Style = "Style3", ReleaseDate = new DateTime(2019, 8, 15), GameMode = "Co-op", CopiesSold = 750000 }
                };

                context.Games.AddRange(games);
                context.SaveChanges();

                // Task 1
                SearchGameByName(context, "Game 1");
                SearchGamesByStudio(context, "Studio B");
                SearchGameByStudioAndName(context, "Studio C", "Game 3");
                SearchGamesByStyle(context, "Style2");
                SearchGamesByYear(context, 2021);

                // Task 2
                DisplaySinglePlayerGames(context);
                DisplayMultiplayerGames(context);
                ShowGameWithMaxCopiesSold(context);
                ShowGameWithMinCopiesSold(context);
                DisplayTop3MostPopularGames(context);
                DisplayTop3MostUnpopularGames(context);

                // Task 3
                AddNewGame(context, new Game { Name = "Game 4", Studio = "Studio D", Style = "Style4", ReleaseDate = new DateTime(2022, 3, 10), GameMode = "Single-player", CopiesSold = 200000 });
                UpdateGame(context, "Game 1", "Studio A", new Game { Name = "Game 1 Updated", Studio = "Studio A", Style = "Style1", ReleaseDate = new DateTime(2020, 1, 1), GameMode = "Single-player", CopiesSold = 1200000 });
                DeleteGame(context, "Game 2", "Studio B");
            }
        }

        // Task 1 Methods
        static void SearchGameByName(GameContext context, string name)
        {
            var game = context.Games.FirstOrDefault(g => g.Name == name);
            if (game != null)
                Console.WriteLine($"Found Game: {game.Name}, Studio: {game.Studio}, Style: {game.Style}, Release Date: {game.ReleaseDate.ToShortDateString()}, Game Mode: {game.GameMode}, Copies Sold: {game.CopiesSold}");
            else
                Console.WriteLine("Game not found.");
        }

        static void SearchGamesByStudio(GameContext context, string studio)
        {
            var games = context.Games.Where(g => g.Studio == studio).ToList();
            if (games.Any())
                games.ForEach(g => Console.WriteLine($"Found Game: {g.Name}, Studio: {g.Studio}, Style: {g.Style}, Release Date: {g.ReleaseDate.ToShortDateString()}, Game Mode: {g.GameMode}, Copies Sold: {g.CopiesSold}"));
            else
                Console.WriteLine("No games found for the given studio.");
        }

        static void SearchGameByStudioAndName(GameContext context, string studio, string name)
        {
            var game = context.Games.FirstOrDefault(g => g.Studio == studio && g.Name == name);
            if (game != null)
                Console.WriteLine($"Found Game: {game.Name}, Studio: {game.Studio}, Style: {game.Style}, Release Date: {game.ReleaseDate.ToShortDateString()}, Game Mode: {game.GameMode}, Copies Sold: {game.CopiesSold}");
            else
                Console.WriteLine("Game not found.");
        }

        static void SearchGamesByStyle(GameContext context, string style)
        {
            var games = context.Games.Where(g => g.Style == style).ToList();
            if (games.Any())
                games.ForEach(g => Console.WriteLine($"Found Game: {g.Name}, Studio: {g.Studio}, Style: {g.Style}, Release Date: {g.ReleaseDate.ToShortDateString()}, Game Mode: {g.GameMode}, Copies Sold: {g.CopiesSold}"));
            else
                Console.WriteLine("No games found for the given style.");
        }

        static void SearchGamesByYear(GameContext context, int year)
        {
            var games = context.Games.Where(g => g.ReleaseDate.Year == year).ToList();
            if (games.Any())
                games.ForEach(g => Console.WriteLine($"Found Game: {g.Name}, Studio: {g.Studio}, Style: {g.Style}, Release Date: {g.ReleaseDate.ToShortDateString()}, Game Mode: {g.GameMode}, Copies Sold: {g.CopiesSold}"));
            else
                Console.WriteLine("No games found for the given year.");
        }

        // Task 2 Methods
        static void DisplaySinglePlayerGames(GameContext context)
        {
            var games = context.Games.Where(g => g.GameMode == "Single-player").ToList();
            if (games.Any())
                games.ForEach(g => Console.WriteLine($"Found Single-player Game: {g.Name}, Studio: {g.Studio}, Style: {g.Style}, Release Date: {g.ReleaseDate.ToShortDateString()}, Copies Sold: {g.CopiesSold}"));
            else
                Console.WriteLine("No Single-player games found.");
        }

        static void DisplayMultiplayerGames(GameContext context)
        {
            var games = context.Games.Where(g => g.GameMode == "Multi-player").ToList();
            if (games.Any())
                games.ForEach(g => Console.WriteLine($"Multiplayer Game: {g.Name}, Studio: {g.Studio}, Style: {g.Style}, Release Date: {g.ReleaseDate.ToShortDateString()}, Copies Sold: {g.CopiesSold}"));
            else
                Console.WriteLine("No multiplayer games found.");
        }

        static void ShowGameWithMaxCopiesSold(GameContext context)
        {
            var game = context.Games.OrderByDescending(g => g.CopiesSold).FirstOrDefault();
            if (game != null)
                Console.WriteLine($"Game with Max Copies Sold: {game.Name}, Studio: {game.Studio}, Style: {game.Style}, Release Date: {game.ReleaseDate.ToShortDateString()}, Copies Sold: {game.CopiesSold}");
            else
                Console.WriteLine("No games found.");
        }

        static void ShowGameWithMinCopiesSold(GameContext context)
        {
            var game = context.Games.OrderBy(g => g.CopiesSold).FirstOrDefault();
            if (game != null)
                Console.WriteLine($"Game with Min Copies Sold: {game.Name}, Studio: {game.Studio}, Style: {game.Style}, Release Date: {game.ReleaseDate.ToShortDateString()}, Copies Sold: {game.CopiesSold}");
            else
                Console.WriteLine("No games found.");
        }

        static void DisplayTop3MostPopularGames(GameContext context)
        {
            var games = context.Games.OrderByDescending(g => g.CopiesSold).Take(3).ToList();
            if (games.Any())
            {
                Console.WriteLine("Top 3 Most Popular Games:");
                games.ForEach(g => Console.WriteLine($"Name: {g.Name}, Studio: {g.Studio}, Copies Sold: {g.CopiesSold}"));
            }
            else
            {
                Console.WriteLine("No games found.");
            }
        }

        static void DisplayTop3MostUnpopularGames(GameContext context)
        {
            var games = context.Games.OrderBy(g => g.CopiesSold).Take(3).ToList();
            if (games.Any())
            {
                Console.WriteLine("Top 3 Most Unpopular Games:");
                games.ForEach(g => Console.WriteLine($"Name: {g.Name}, Studio: {g.Studio}, Copies Sold: {g.CopiesSold}"));
            }
            else
            {
                Console.WriteLine("No games found.");
            }
        }

        // Task 3 Methods
        static void AddNewGame(GameContext context, Game newGame)
        {
            var existingGame = context.Games.FirstOrDefault(g => g.Name == newGame.Name && g.Studio == newGame.Studio);
            if (existingGame == null)
            {
                context.Games.Add(newGame);
                context.SaveChanges();
                Console.WriteLine("New game added successfully.");
            }
            else
            {
                Console.WriteLine("Game already exists.");
            }
        }

        static void UpdateGame(GameContext context, string name, string studio, Game updatedGame)
        {
            var game = context.Games.FirstOrDefault(g => g.Name == name && g.Studio == studio);
            if (game != null)
            {
                game.Name = updatedGame.Name;
                game.Studio = updatedGame.Studio;
                game.Style = updatedGame.Style;
                game.ReleaseDate = updatedGame.ReleaseDate;
                game.GameMode = updatedGame.GameMode;
                game.CopiesSold = updatedGame.CopiesSold;
                context.SaveChanges();
                Console.WriteLine("Game updated successfully.");
            }
            else
            {
                Console.WriteLine("Game not found.");
            }
        }

        static void DeleteGame(GameContext context, string name, string studio)
        {
            var game = context.Games.FirstOrDefault(g => g.Name == name && g.Studio == studio);
            if (game != null)
            {
                Console.WriteLine($"Are you sure you want to delete the game: {game.Name} from {game.Studio}? (yes/no)");
                var confirmation = Console.ReadLine();
                if (confirmation?.ToLower() == "yes")
                {
                    context.Games.Remove(game);
                    context.SaveChanges();
                    Console.WriteLine("Game deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion canceled.");
                }
            }
            else
            {
                Console.WriteLine("Game not found.");
            }
        }
    }
}
