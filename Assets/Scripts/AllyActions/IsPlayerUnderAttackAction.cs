using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsPlayerUnderAttack", story: "Check if player is under attack", category: "Action", id: "c74214438a33632cdb05cccb3ed2baed")]
public partial class IsPlayerUnderAttackAction : Action
{

    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;

    protected override Status OnStart()
    {
        bool underAttack = commandData.Value.isPlayerUnderAttack;
        if (underAttack) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

