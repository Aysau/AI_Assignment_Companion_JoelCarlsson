using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsSelfUnderAttack", story: "Check if self is under attack", category: "Action", id: "7dd354a8b7c8db25e7a4c2e90d3f455c")]
public partial class IsSelfUnderAttackAction : Action
{

    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;

    protected override Status OnStart()
    {
        bool underAttack = commandData.Value.isSelfUnderAttack;
        if (underAttack) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

