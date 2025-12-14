using System;
using System.Collections.Generic;

namespace DialogGraph.Runtime
{
    [Serializable]
    public class RuntimeDialogNode : RuntimeNode
    {
        public string Speaker;
        public string Message;
        public List<OptionData> Options = new();
        public string NextNodeId;
    }

    [Serializable]
    public class OptionData
    {
        public string OptionText;
        public string ConnectedNodeId;
    }
}