using UnityEngine;

namespace DialogGraph.Runtime
{
    public abstract class Evaluator : ScriptableObject
    {
        public abstract bool Evaluate();
    }
}