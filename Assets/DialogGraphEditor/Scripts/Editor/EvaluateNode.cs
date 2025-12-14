using System;
using DialogGraph.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogGraph.Editor
{
    [Serializable]
    internal class EvaluateNode : BaseNode
    {
        public const string EVALUATOR_PORT_NAME = "EVALUATOR";
        public const string TRUE_OUT_PORT_NAME = "TRUE";
        public const string FALSE_OUT_PORT_NAME = "FALSE";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(DEFAULT_INPUT_PORT_NAME)
                    .WithDisplayName(string.Empty)
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();

            context.AddInputPort<Evaluator>(EVALUATOR_PORT_NAME)
                    .WithDisplayName("Evaluator")
                    .Build();
            
            context.AddOutputPort(TRUE_OUT_PORT_NAME)
                    .WithDisplayName("True")
                    .Build();
            
            context.AddOutputPort(FALSE_OUT_PORT_NAME)
                    .WithDisplayName("False")
                    .Build();
        }
    }
}