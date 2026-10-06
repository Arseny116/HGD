using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class FlowchartBlock
    {
        private FlowchartBlock(List<FlowchartNode> nodes, List<FlowchartEdge> edges)
        {
            Id = Guid.NewGuid();
            Nodes = nodes.ToList();
            Edges = edges.ToList();
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public List<FlowchartNode> Nodes { get; private set; } = new();
        public List<FlowchartEdge> Edges { get; private set; } = new();

        public static Result<FlowchartBlock> Create(
            List<FlowchartNode> nodes,
            List<FlowchartEdge> edges)
        {
            if (nodes is null || nodes.Count == 0)
                return Result.Failure<FlowchartBlock>("At least one node is required.");

            if (edges is null)
                return Result.Failure<FlowchartBlock>("Edges cannot be null.");

            return Result.Success(new FlowchartBlock(nodes, edges));
        }

        public Result AddNode(FlowchartNode node)
        {
            if (node is null) return Result.Failure("Node cannot be null.");
            Nodes.Add(node);
            return Result.Success();
        }

        public Result AddEdge(FlowchartEdge edge)
        {
            if (edge is null) return Result.Failure("Edge cannot be null.");

            var fromExists = Nodes.Any(n => n.Id == edge.FromNodeId);
            var toExists = Nodes.Any(n => n.Id == edge.ToNodeId);

            if (!fromExists || !toExists)
                return Result.Failure("Edge references non-existing node.");

            Edges.Add(edge);
            return Result.Success();
        }
    }

    public class FlowchartNode
    {
        private FlowchartNode(string label, FlowchartNodeType type, double x, double y)
        {
            Id = Guid.NewGuid();
            Label = label;
            Type = type;
            X = x;
            Y = y;
        }

        public Guid Id { get; private set; }
        public Guid FlowchartBlockId { get; private set; }

        public string Label { get; private set; } = string.Empty;
        public FlowchartNodeType Type { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public string? Color { get; private set; }

        public static Result<FlowchartNode> Create(
            string label,
            FlowchartNodeType type,
            double x,
            double y,
            string? color = null)
        {
            if (string.IsNullOrWhiteSpace(label))
                return Result.Failure<FlowchartNode>("Label cannot be empty.");

            var node = new FlowchartNode(label, type, x, y) { Color = color };
            return Result.Success(node);
        }
    }

    public class FlowchartEdge
    {
        private FlowchartEdge(Guid fromNodeId, Guid toNodeId, string? label)
        {
            Id = Guid.NewGuid();
            FromNodeId = fromNodeId;
            ToNodeId = toNodeId;
            Label = label;
        }

        public Guid Id { get; private set; }
        public Guid FlowchartBlockId { get; private set; }

        public Guid FromNodeId { get; private set; }
        public Guid ToNodeId { get; private set; }
        public string? Label { get; private set; }

        public static Result<FlowchartEdge> Create(Guid fromNodeId, Guid toNodeId, string? label = null)
        {
            if (fromNodeId == Guid.Empty || toNodeId == Guid.Empty)
                return Result.Failure<FlowchartEdge>("Node ids cannot be empty.");

            if (fromNodeId == toNodeId)
                return Result.Failure<FlowchartEdge>("Self-loop edges are not allowed.");

            return Result.Success(new FlowchartEdge(fromNodeId, toNodeId, label));
        }
    }

    public enum FlowchartNodeType
    {
        Start,
        Process,
        Decision,
        End
    }
}