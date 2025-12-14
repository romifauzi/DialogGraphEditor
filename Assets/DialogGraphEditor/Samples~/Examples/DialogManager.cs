using System.Collections.Generic;
using DialogGraph.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogGraph.Examples
{
    public class DialogManager : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private RuntimeDialogGraph debugRuntimeGraph;

        [Header("UI Components")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TMPro.TMP_Text speakerNameText;
        [SerializeField] private TMPro.TMP_Text messageText;
        [SerializeField] private Transform buttonOptionsContainer;
        [SerializeField] private ButtonOption buttonOptionPrefab;
        
        private Dictionary<string, RuntimeNode> _nodeLookup = new();
        private RuntimeNode _currentNode;

        protected virtual void Start()
        {
            if (debugRuntimeGraph != null) StartDialog(debugRuntimeGraph);
        }

        [ContextMenu("Show Debug Dialog")]
        private void ShowDebugDialog()
        {
            StartDialog(debugRuntimeGraph);
        }

        public void StartDialog(RuntimeDialogGraph graph)
        {
            _nodeLookup.Clear();
            foreach (var node in graph.DialogNodes)
            {
                _nodeLookup[node.NodeId] = node;
            }

            foreach (var node in graph.EvaluatorNodes)
            {
                _nodeLookup[node.NodeId] = node;
            }

            if (!string.IsNullOrEmpty(graph.EntryNodeId))
            {
                ShowNode(graph.EntryNodeId);
                return;
            }

            EndDialog();
        }

        private void ShowNode(string nodeId)
        {
            if (!_nodeLookup.ContainsKey(nodeId))
            {
                EndDialog();
                return;
            }

            ClearOptions();

            var node = _nodeLookup[nodeId];

            if (node is RuntimeDialogNode dialogNode)
            {
                ShowDialog(dialogNode);
            }
            else if (node is RuntimeEvaluatorNode evalNode)
            {
                Evaluate(evalNode);
            }
        }

        private async void ShowDialog(RuntimeDialogNode node)
        {
            ShowSpeaker(string.Empty);
            _ = ShowMessage(string.Empty);

            await ShowPanel(true);
            
            _currentNode = node;
            ShowSpeaker(node.Speaker);
            
            await ShowMessage(node.Message);

            if (node.Options.Count > 0)
                ShowOptions(node.Options);
        }

        private void ShowSpeaker(string speaker)
        {
            var show = !string.IsNullOrEmpty(speaker);
            speakerNameText.transform.parent.gameObject.SetActive(show);
            if (!show) return;
            speakerNameText.SetText(speaker);
        }

        private void ShowOptions(List<OptionData> options)
        {
            ClearOptions();
            foreach (var option in options)
            {
                var button = Instantiate(buttonOptionPrefab, buttonOptionsContainer);
                button.SetupResponse(()=> ShowNode(option.ConnectedNodeId), option.OptionText);
            }
        }

        private void Evaluate(RuntimeEvaluatorNode node)
        {
            _currentNode = node;

            var eval = node.Evaluator.Evaluate();

            if (eval)
                ShowNode(node.TrueNodeId);
            else
                ShowNode(node.FalseNodeId);
        }

        private void ClearOptions()
        {
            foreach (Transform button in buttonOptionsContainer)
            {
                Destroy(button.gameObject);
            }
        }

        private void EndDialog()
        {
            ClearOptions();
            _currentNode = null;
            _ = ShowPanel(false);
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && 
                panel.activeInHierarchy &&
                _currentNode != null && 
                _currentNode is RuntimeDialogNode dialogNode && 
                buttonOptionsContainer.childCount == 0)
            {
                ShowNextNode(dialogNode.NextNodeId);
            }
        }

        protected virtual async Awaitable ShowPanel(bool enable)
        {
            panel.SetActive(enable);
        }

        protected virtual async Awaitable ShowMessage(string message)
        {
            messageText.SetText(message);
        }

        protected virtual void ShowNextNode(string nextNodeId)
        {
            ShowNode(nextNodeId);
        }
    }
}