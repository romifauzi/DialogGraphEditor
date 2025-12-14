using System;
using UnityEngine;

namespace DialogGraph.Runtime
{
    [Serializable]
    public class RuntimeEvaluatorNode : RuntimeNode
    {
        [SerializeReference] public Evaluator Evaluator;
        public string TrueNodeId;
        public string FalseNodeId;
    }
}