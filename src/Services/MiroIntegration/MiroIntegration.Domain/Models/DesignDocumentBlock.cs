using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class DesignDocumentBlock
    {
        private DesignDocumentBlock(string guidelines, List<DesignDefinition> definitions)
        {
            Id = Guid.NewGuid();
            Guidelines = guidelines;
            Definitions = definitions.ToList();
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public string Guidelines { get; private set; } = string.Empty;
        public List<DesignDefinition> Definitions { get; private set; } = new();

        public static Result<DesignDocumentBlock> Create(
            string guidelines,
            List<DesignDefinition> definitions)
        {
            if (string.IsNullOrWhiteSpace(guidelines))
                return Result.Failure<DesignDocumentBlock>("Guidelines cannot be empty.");

            if (definitions is null)
                return Result.Failure<DesignDocumentBlock>("Definitions cannot be null.");

            return Result.Success(new DesignDocumentBlock(guidelines, definitions));
        }

        public Result AddDefinition(DesignDefinition definition)
        {
            if (definition is null) return Result.Failure("Definition cannot be null.");
            Definitions.Add(definition);
            return Result.Success();
        }
    }

    public class DesignDefinition
    {
        private DesignDefinition(string key, string value)
        {
            Id = Guid.NewGuid();
            Key = key;
            Value = value;
        }

        public Guid Id { get; private set; }
        public Guid DesignDocumentBlockId { get; private set; }

        public string Key { get; private set; } = string.Empty;
        public string Value { get; private set; } = string.Empty;

        public static Result<DesignDefinition> Create(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return Result.Failure<DesignDefinition>("Key cannot be empty.");

            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<DesignDefinition>("Value cannot be empty.");

            return Result.Success(new DesignDefinition(key, value));
        }
    }
}