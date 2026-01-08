using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FleeFromDanger", story: "Calculates positon away from nearest enemy, and moves to it.", category: "Action", id: "68438be64de3489f27104bfaed406170")]
public partial class FleeFromDangerAction : Action
{

    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<NavMeshAgent> navAgent;
    [SerializeReference]
    public BlackboardVariable<AllyPerceptionSystem> perception;
    [SerializeReference]
    public BlackboardVariable<float> fleeDistance;
    [SerializeReference]
    public BlackboardVariable<Vector3> fleeTarget;
    [SerializeReference]
    public BlackboardVariable<float> recalculateTimer;

    protected override Status OnStart()
    {
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        GameObject nearestEnemy = perception.Value.GetNearestEnemy();

        if(nearestEnemy == null)
        {
            navAgent.Value.isStopped = true;
            return Status.Success;
        }

        recalculateTimer.Value += Time.deltaTime;
        if(recalculateTimer.Value >= 0.5f)
        {
            Vector3 fleeDirection = (allyTransform.Value.position - nearestEnemy.transform.position).normalized;
            fleeTarget.Value = allyTransform.Value.position + fleeDirection * fleeDistance.Value;
            recalculateTimer.Value = 0f;
        }

        navAgent.Value.isStopped = false;
        navAgent.Value.SetDestination(fleeTarget);
        return Status.Running;
    }
}

