using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckTargetDead", story: "Check if target enemy is dead and clear command if so", category: "Action", id: "a4f5151ed419f43c8f7b92858a16934e")]
public partial class CheckTargetDeadAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    protected override Status OnStart()
    {
        if(commandData.Value.targetEnemy == null || !commandData.Value.targetEnemy.activeInHierarchy)
        {
            commandData.Value.ClearCommand();
            Debug.Log("[Ally] Target eliminated - command complete");
            return Status.Success;
        }
        else
        {
            return Status.Failure; 
        }
    }

}

