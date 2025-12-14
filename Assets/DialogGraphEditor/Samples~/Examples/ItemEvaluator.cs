using DialogGraph.Runtime;
using UnityEngine;

namespace DialogGraph.Examples
{
    [CreateAssetMenu(fileName ="ItemEval", menuName = "Romi's Dialog Graph/Item Evaluator")]
    public class ItemEvaluator : Evaluator
    {
        public bool itemOwned;

        public override bool Evaluate()
        {
            return itemOwned;
        }
    }
}