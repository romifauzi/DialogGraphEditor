using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogGraph.Runtime
{
    [Serializable]
    public class RuntimeDialogGraph : ScriptableObject
    {
        public string EntryNodeId;
        public List<RuntimeDialogNode> DialogNodes = new();
        public List<RuntimeEvaluatorNode> EvaluatorNodes = new();

    }
}