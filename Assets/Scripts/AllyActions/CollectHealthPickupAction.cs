using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CollectHealthPickup", story: "Collect health pickup when close enough", category: "Action", id: "38aa5ba07077457b695a510a590706a2")]
public partial class CollectHealthPickupAction : Action
{
    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<float> collectRange;
    [SerializeReference]
    public BlackboardVariable<float> healAmount;
    protected override Status OnStart()
    {
        GameObject pickup = commandData.Value.targetPickup;

        if(pickup == null || !pickup.activeInHierarchy)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, pickup.transform.position);

        if(distance <= collectRange)
        {
            commandData.Value.currentHealth = Mathf.Min(commandData.Value.currentHealth + healAmount, commandData.Value.maxHealth);

            //TODO Implement actual pickup collection

            Debug.Log($"[Ally] Collected health pickup! Health: {commandData.Value.currentHealth}/{commandData.Value.maxHealth}");

            pickup.SetActive(false);

            return Status.Success;
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

