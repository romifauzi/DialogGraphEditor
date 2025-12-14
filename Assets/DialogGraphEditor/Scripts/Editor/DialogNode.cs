using System;
using Unity.GraphToolkit.Editor;

namespace DialogGraph.Editor
{
    [Serializable]
    internal class DialogNode : BaseNode
    {
        private const string CHOICE_COUNT_OPTION_NAME = "ChoicesOptionCount";

        internal const string SPEAKER_INPUT_PORT_NAME = "Speaker";
        internal const string MESSAGE_INPUT_PORT_NAME = "Message";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(DEFAULT_INPUT_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddInputPort<string>(SPEAKER_INPUT_PORT_NAME)
                .WithDisplayName("Speaker")
                .WithDefaultValue(string.Empty)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();

            context.AddInputPort<string>(MESSAGE_INPUT_PORT_NAME)
                .WithDisplayName("Message")
                .WithDefaultValue(string.Empty)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();


            var choiceOptionCount = GetNodeOptionByName(CHOICE_COUNT_OPTION_NAME);

            if (choiceOptionCount.TryGetValue<int>(out var choiceCount))
            {
                if (choiceCount == 0)
                {
                    context.AddOutputPort(DEFAULT_OUTPUT_PORT_NAME)
                    .WithDisplayName($"out")
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();
                }

                for (int i = 0; i < choiceCount; i++)
                {
                    var name = $"Choice {i + 1:00}";

                    context.AddInputPort<string>(name)
                        .WithDisplayName(name)
                        .WithDefaultValue(string.Empty)
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    context.AddOutputPort(name)
                        .WithDisplayName($"{name} out")
                        .WithConnectorUI(PortConnectorUI.Arrowhead)
                        .Build();
                }
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(CHOICE_COUNT_OPTION_NAME)
            .WithDisplayName("Choice Count")
            .WithDefaultValue(0)
            .Delayed();
        }
    }
}