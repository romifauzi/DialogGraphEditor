using System;
using Unity.GraphToolkit.Editor;

namespace DialogGraph.Editor
{
    [Serializable]
    internal class StartNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(DEFAULT_OUTPUT_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}