using System;
using DialogGraph.Runtime;
using UnityEngine;

namespace DialogGraph.Examples
{
    [CreateAssetMenu(fileName ="EventTriggerer", menuName = "Romi's Dialog Graph/Event Triggerer")]
    public class EventTriggerer : Evaluator
    {
        public event Action onTriggered;
        
        public override bool Evaluate()
        {
            onTriggered?.Invoke();
            return true;
        }
    }
}