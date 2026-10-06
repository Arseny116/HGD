using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class UiBlock
    {
        private UiBlock(string? layoutImageUrl, string? layoutDescription)
        {
            Id = Guid.NewGuid();
            LayoutImageUrl = layoutImageUrl;
            LayoutDescription = layoutDescription;
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public List<UiControl> Controls { get; private set; } = new();

        public string? LayoutImageUrl { get; private set; }
        public string? LayoutDescription { get; private set; }

        public static Result<UiBlock> Create(string? layoutImageUrl, string? layoutDescription)
        {
            if (string.IsNullOrWhiteSpace(layoutImageUrl) && string.IsNullOrWhiteSpace(layoutDescription))
                return Result.Failure<UiBlock>("Either LayoutImageUrl or LayoutDescription must be provided.");

            return Result.Success(new UiBlock(layoutImageUrl, layoutDescription));
        }

        public Result AddControl(UiControl control)
        {
            if (control is null) return Result.Failure("Control cannot be null.");
            Controls.Add(control);
            return Result.Success();
        }
    }

    public class UiControl
    {
        private UiControl(string name, string action, UiControlType type, string? iconUrl)
        {
            Id = Guid.NewGuid();
            Name = name;
            Action = action;
            Type = type;
            IconUrl = iconUrl;
        }

        public Guid Id { get; private set; }
        public Guid UiBlockId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string Action { get; private set; } = string.Empty;
        public UiControlType Type { get; private set; }
        public string? IconUrl { get; private set; }

        public static Result<UiControl> Create(
            string name,
            string action,
            UiControlType type,
            string? iconUrl = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<UiControl>("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(action))
                return Result.Failure<UiControl>("Action cannot be empty.");

            return Result.Success(new UiControl(name, action, type, iconUrl));
        }
    }

    public enum UiControlType
    {
        Button,
        Joystick,
        Swipe,
        Tap,
        Gesture
    }
}