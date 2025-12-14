using Unity.GraphToolkit.Editor;

namespace DialogGraph.Editor
{
    internal abstract class BaseNode : Node
    {
        public const string DEFAULT_INPUT_PORT_NAME = "In";
        public const string DEFAULT_OUTPUT_PORT_NAME = "Out";
    }
}