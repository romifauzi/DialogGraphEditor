using System;

namespace DialogGraph.Runtime
{
    [Serializable]
    public class RuntimeEvaluatorNode : RuntimeNode
    {
        public Evaluator Evaluator;
        public string TrueNodeId;
        public string FalseNodeId;
    }
}