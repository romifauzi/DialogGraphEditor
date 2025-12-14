using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace DialogGraph.Editor
{
    [Serializable]
    [Graph(AssetExtension)]
    internal class DialogGraphView : Graph 
    {
        internal const string AssetExtension = "dialogGraph";

        [MenuItem("Assets/Create/Romi's Dialog Graph/Dialog Canvas Graph")]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogGraphView>("DialogNodesGraph");
        }

        public override void OnGraphChanged(GraphLogger graphLogger)
        {
            base.OnGraphChanged(graphLogger);
        }
    }
}