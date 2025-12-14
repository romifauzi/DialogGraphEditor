
# Dialog Graph Editor

A Unity package that allows you to create branching narrative with visual nodes approach, also allows custom condition evaluation and event triggering via ScriptableObjects.
![Dialog Graph Editor Preview](https://github.com/romifauzi/DialogGraphEditor/raw/main/dialog_graph_screenshot.png)
## Installation

1. Open Unity and go to the **Package Manager** window.
2. Press the `+` button in the top left corner.
3. Choose **"Add package from Git URL..."**
4. Paste the following URL:`https://github.com/romifauzi/DialogGraphEditor.git?path=/Assets/DialogGraphEditor`
5. The package will be added to your project. You can also check the included samples for example scenes and usage.

## How to Use

1. **How to Create a Dialog Graph**
- In Project panel, right click, access `Create > Romi's Dialog Graph > Dialog Canvas Graph` to create a new dialog graph asset.
- Double click on the newly created dialog graph asset to open its editor.

2. **Runtime Usage**
- Import samples to see how to use the graph in runtime (see `DialogManager.cs` to see the bulk of code on how to use the dialog graph in your game). 
- The sample scene shows how to do alternate branching based on certain condition as well as custom event triggering.
- Refer to `ItemEvaluator.cs` on how to create your own custom condition evaluator to be used with the Dialog Graph.
- Refer to `EventTriggerer.cs` and `EventReceiver.cs` for triggering event from the Dialog graph and receiving event to a gameObject.
---

This add-on is heavily inspired from https://www.youtube.com/watch?v=Spa8au6cOmo by **Sasquatch B Studios**, with additional modification to do custom condition evaluation and event triggering.

Enjoy using **Dialog Graph Editor** to create branching narrative in your Unity projects and please report any bug, Thanks!