using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindNearestHealthPickup", story: "Find and store nearest health pickup", category: "Action", id: "c47fbe1ff283999e4a6f8b8cd8bddf45")]
public partial class FindNearestHealthPickupAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyPerceptionSystem> perception;
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    protected override Status OnStart()
    {
        GameObject pickup = perception.Value.GetNearestHealthPickup();

        if(pickup == null)
        {
            Debug.Log("[Ally] No health pickup found nearby");
            return Status.Failure;
        }

        commandData.Value.targetPickup = pickup;

        return Status.Success;
    }

}

