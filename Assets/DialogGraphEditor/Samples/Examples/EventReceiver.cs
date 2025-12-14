using UnityEngine;
using UnityEngine.Events;

namespace DialogGraph.Examples
{
    public class EventReceiver : MonoBehaviour
    {
        [SerializeField] private EventTriggerer triggerer;

        [SerializeField] private UnityEvent onEventReceived;
        
        // Start is called before the first frame update
        void Start()
        {
            triggerer.onTriggered += ReceivedEvent;
        }

        private void OnDestroy()
        {
            triggerer.onTriggered -= ReceivedEvent;
        }

        private void ReceivedEvent()
        {
            Debug.Log($"Event received from {triggerer.name}");
            onEventReceived?.Invoke();
        }
    }
}