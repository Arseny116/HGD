using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class PlayerBlock
    {
        private PlayerBlock(PlayerDefinition definition)
        {
            Id = Guid.NewGuid();
            Definition = definition;
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public PlayerDefinition Definition { get; private set; }
        public List<PlayerProperty> Properties { get; private set; } = new();
        public List<PlayerReward> Rewards { get; private set; } = new();

        public string? ReferenceImageUrl { get; private set; }

        public static Result<PlayerBlock> Create(PlayerDefinition definition, string? referenceImageUrl = null)
        {
            if (definition is null)
                return Result.Failure<PlayerBlock>("Definition cannot be null.");

            var player = new PlayerBlock(definition) { ReferenceImageUrl = referenceImageUrl };
            return Result.Success(player);
        }

        public Result AddProperty(PlayerProperty property)
        {
            if (property is null) return Result.Failure("Property cannot be null.");
            Properties.Add(property);
            return Result.Success();
        }

        public Result AddReward(PlayerReward reward)
        {
            if (reward is null) return Result.Failure("Reward cannot be null.");
            Rewards.Add(reward);
            return Result.Success();
        }
    }

    public class PlayerDefinition
    {
        private PlayerDefinition(int health, List<string> weapons, List<string> actions)
        {
            Id = Guid.NewGuid();
            Health = health;
            Weapons = weapons.ToList();
            Actions = actions.ToList();
        }

        public Guid Id { get; private set; }
        public Guid PlayerBlockId { get; private set; }

        public int Health { get; private set; }
        public List<string> Weapons { get; private set; } = new();
        public List<string> Actions { get; private set; } = new();

        public static Result<PlayerDefinition> Create(int health, List<string> weapons, List<string> actions)
        {
            if (health <= 0)
                return Result.Failure<PlayerDefinition>("Health must be greater than 0.");

            if (weapons is null)
                return Result.Failure<PlayerDefinition>("Weapons cannot be null.");

            if (actions is null)
                return Result.Failure<PlayerDefinition>("Actions cannot be null.");

            return Result.Success(new PlayerDefinition(health, weapons, actions));
        }
    }

    public class PlayerProperty
    {
        private PlayerProperty(string name, string description, string? feedback)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Feedback = feedback;
        }

        public Guid Id { get; private set; }
        public Guid PlayerBlockId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string? Feedback { get; private set; }

        public static Result<PlayerProperty> Create(string name, string description, string? feedback = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<PlayerProperty>("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<PlayerProperty>("Description cannot be empty.");

            return Result.Success(new PlayerProperty(name, description, feedback));
        }
    }

    public class PlayerReward
    {
        private PlayerReward(string name, string effect, PlayerRewardType type, string? iconUrl)
        {
            Id = Guid.NewGuid();
            Name = name;
            Effect = effect;
            Type = type;
            IconUrl = iconUrl;
        }

        public Guid Id { get; private set; }
        public Guid PlayerBlockId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string Effect { get; private set; } = string.Empty;
        public PlayerRewardType Type { get; private set; }
        public string? IconUrl { get; private set; }

        public static Result<PlayerReward> Create(
            string name,
            string effect,
            PlayerRewardType type,
            string? iconUrl = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<PlayerReward>("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(effect))
                return Result.Failure<PlayerReward>("Effect cannot be empty.");

            return Result.Success(new PlayerReward(name, effect, type, iconUrl));
        }
    }

    public enum PlayerRewardType
    {
        PowerUp,
        PickUp,
        Bonus
    }
}