using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateCommandChanged", story: "Updates command changed", category: "Action", id: "d7ea61e7b97e3b0627e7f09279e21b73")]
public partial class UpdateCommandChangedAction : Action
{
    [SerializeReference]
    public BlackboardVariable<bool> commandDataChanged;
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    AllyCommand lastCommand;
    protected override Status OnUpdate()
    {
        if(commandData.Value.currentCommand != lastCommand)
        {
            commandDataChanged.Value = true;
        }
        else
        {
            commandDataChanged.Value = false;
        }
        lastCommand = commandData.Value.currentCommand;
        return Status.Running;
    }


}

