using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class MetaBlock
    {
        private MetaBlock(string gameName, string genre, string gameElements, int playerCount)
        {
            Id = Guid.NewGuid();
            GameName = gameName;
            Genre = genre;
            GameElements = gameElements;
            PlayerCount = playerCount;
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public string GameName { get; private set; } = string.Empty;
        public string Genre { get; private set; } = string.Empty;
        public string GameElements { get; private set; } = string.Empty;
        public int PlayerCount { get; private set; }

        public string? CoverImageUrl { get; private set; }

        public static Result<MetaBlock> Create(
            string gameName,
            string genre,
            string gameElements,
            int playerCount,
            string? coverImageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(gameName))
                return Result.Failure<MetaBlock>("GameName cannot be empty.");

            if (string.IsNullOrWhiteSpace(genre))
                return Result.Failure<MetaBlock>("Genre cannot be empty.");

            if (playerCount < 1)
                return Result.Failure<MetaBlock>("PlayerCount must be at least 1.");

            var meta = new MetaBlock(gameName, genre, gameElements, playerCount)
            {
                CoverImageUrl = coverImageUrl
            };

            return Result.Success(meta);
        }

        public Result Update(
            string gameName,
            string genre,
            string gameElements,
            int playerCount,
            string? coverImageUrl)
        {
            if (string.IsNullOrWhiteSpace(gameName))
                return Result.Failure("GameName cannot be empty.");

            if (playerCount < 1)
                return Result.Failure("PlayerCount must be at least 1.");

            GameName = gameName;
            Genre = genre;
            GameElements = gameElements;
            PlayerCount = playerCount;
            CoverImageUrl = coverImageUrl;

            return Result.Success();
        }
    }
}