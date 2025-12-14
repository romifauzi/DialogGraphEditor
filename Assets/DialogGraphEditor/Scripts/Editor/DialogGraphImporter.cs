using System;
using System.Collections.Generic;
using System.Linq;
using DialogGraph.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace DialogGraph.Editor
{
    [ScriptedImporter(1, DialogGraphView.AssetExtension)]
    public class DialogGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var editorGraph = GraphDatabase.LoadGraphForImporter<DialogGraphView>(ctx.assetPath);
            var runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogGraph>();
            var nodeIdMap = new Dictionary<INode, string>();

            foreach (var node in editorGraph.GetNodes())
            {
                nodeIdMap[node] = Guid.NewGuid().ToString();
            }

            var startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNode != null)
            {
                var entryPort = startNode.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
                if (entryPort != null)
                {
                    //populate entry node id
                    runtimeGraph.EntryNodeId = nodeIdMap[entryPort.GetNode()];
                }
            }

            foreach (var iNode in editorGraph.GetNodes())
            {
                if (iNode is StartNode) continue;

                if (iNode is DialogNode dialogNode)
                {
                    var runtimeDialogNode = new RuntimeDialogNode { NodeId = nodeIdMap[iNode] };
                    ProcessDialogNode(dialogNode, runtimeDialogNode, nodeIdMap);
                    runtimeGraph.DialogNodes.Add(runtimeDialogNode);
                }
                else if (iNode is EvaluateNode evaluateNode)
                {
                    var runtimeEvalNode = new RuntimeEvaluatorNode { NodeId = nodeIdMap[iNode] };
                    ProcessEvaluatorNode(evaluateNode, runtimeEvalNode, nodeIdMap);
                    runtimeGraph.EvaluatorNodes.Add(runtimeEvalNode);
                }

            }

            ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
            ctx.SetMainObject(runtimeGraph);
        }

        private void ProcessDialogNode(DialogNode node, RuntimeDialogNode runtimeNode, Dictionary<INode, string> nodeIdMap)
        {
            runtimeNode.Speaker = GetPortValue<string>(node.GetInputPortByName(DialogNode.SPEAKER_INPUT_PORT_NAME));
            runtimeNode.Message = GetPortValue<string>(node.GetInputPortByName(DialogNode.MESSAGE_INPUT_PORT_NAME));

            var optionsOutputPorts = node.GetOutputPorts().Where(x => x.name.StartsWith("Choice ", StringComparison.CurrentCultureIgnoreCase));

            if (optionsOutputPorts.Count() > 0)
            {
                foreach (var output in optionsOutputPorts)
                {
                    var optionPort = node.GetInputPortByName(output.name);

                    var optionData = new OptionData
                    {
                        OptionText = GetPortValue<string>(optionPort),
                        ConnectedNodeId = output.firstConnectedPort != null ?
                            nodeIdMap[output.firstConnectedPort.GetNode()] : null
                    };

                    runtimeNode.Options.Add(optionData);
                }
            }
            else
            {
                var nextNodePort = node.GetOutputPortByName(BaseNode.DEFAULT_OUTPUT_PORT_NAME);
                if (nextNodePort != null)
                {
                    var nextNode = nextNodePort.firstConnectedPort?.GetNode(); 
                    if (nextNode != null && nodeIdMap.TryGetValue(nextNode, out var nextNodeId))
                        runtimeNode.NextNodeId = nextNodeId;
                }
            }
        }

        private void ProcessEvaluatorNode(EvaluateNode node, RuntimeEvaluatorNode runtimeEvalNode, Dictionary<INode, string> nodeIdMap)
        {
            runtimeEvalNode.Evaluator = GetPortValue<Evaluator>(node.GetInputPortByName(EvaluateNode.EVALUATOR_PORT_NAME));

            var trueConnectedNode = node.GetOutputPortByName(EvaluateNode.TRUE_OUT_PORT_NAME).firstConnectedPort.GetNode();
            if (trueConnectedNode != null)
            {
                runtimeEvalNode.TrueNodeId = nodeIdMap[trueConnectedNode];
            }

            var falseConnectedNode = node.GetOutputPortByName(EvaluateNode.FALSE_OUT_PORT_NAME).firstConnectedPort.GetNode();
            if (falseConnectedNode != null)
            {
                runtimeEvalNode.FalseNodeId = nodeIdMap[falseConnectedNode];
            }
        }

        private T GetPortValue<T>(IPort port)
        {
            if (port == null) return default;

            if (port.isConnected)
            {
                if (port.firstConnectedPort.GetNode() is IVariableNode variableNode)
                {
                    variableNode.variable.TryGetDefaultValue(out T value);
                    return value;
                }
            }

            port.TryGetValue(out T fallbackValue);
            return fallbackValue;
        }
    }
}
