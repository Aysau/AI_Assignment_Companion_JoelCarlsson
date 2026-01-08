using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GiveHealthToPlayer", story: "Give health to player when close enough", category: "Action", id: "fdabf788f23b9ea5791b9f0ababc324a")]
public partial class GiveHealthToPlayerAction : Action
{
    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<float> healAmount;
    [SerializeReference]
    public BlackboardVariable<float> giveRange;
    protected override Status OnStart()
    {
        if(commandData.Value.playerTransform == null)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, commandData.Value.playerTransform.position);

        if(distance <= giveRange)
        {
            commandData.Value.playerHealth.Heal(healAmount);

            Debug.Log("[Ally] Gave health pickup to player");

            return Status.Success;
        }

        return Status.Failure;
    }

}

