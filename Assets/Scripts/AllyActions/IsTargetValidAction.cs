using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsTargetValid", story: "Check if command target is still valid", category: "Action", id: "8bae25741901ce98e66edd63834f2c6c")]
public partial class IsTargetValidAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;

    [SerializeReference]
    public BlackboardVariable<bool> checkEnemy;
    protected override Status OnStart()
    {
        bool validTarget = checkEnemy ? commandData.Value.IsTargetEnemyValid() : commandData.Value.IsTargetObjectValid();
        if (validTarget) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

