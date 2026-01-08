using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "Attack a target", category: "Action", id: "9a1883efc6b14d03c6cafa79f09105a7")]
public partial class AttackTargetAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<AllyPerceptionSystem> perception;
    [SerializeReference]
    public BlackboardVariable<TargetType> targetType;


    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<float> attackRange;
    [SerializeReference]
    public BlackboardVariable<float> attackCooldown;
    [SerializeReference]
    public BlackboardVariable<float> lastAttackTime;
    [SerializeReference]
    public BlackboardVariable<int> damageAmount;
    protected override Status OnStart()
    {
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        GameObject target = GetTarget();

        if(target == null || !target.activeInHierarchy)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, target.transform.position);

        if(distance > attackRange)
        {
            return Status.Failure;
        }

        Vector3 direction = (target.transform.position - allyTransform.Value.position).normalized;
        direction.y = 0; //TODO Maybe fix?
        if(direction != Vector3.zero)
        {
            allyTransform.Value.rotation = Quaternion.LookRotation(direction);
        }

        if(Time.time - lastAttackTime.Value >= attackCooldown)
        {
            PerformAttack(target);
            lastAttackTime.Value = Time.time;
        }

        return Status.Running;
    }

    void PerformAttack(GameObject target)
    {
        //TODO implement attack logic
        Debug.Log($"[Ally] Attacking {target.name} for {damageAmount} damage");
    }
    private GameObject GetTarget()
    {
        switch (targetType.Value)
        {
            case TargetType.CommandTargetEnemy: return commandData.Value.targetEnemy;
            case TargetType.CommandTargetObject: return commandData.Value.targetObject;
            case TargetType.PlayerTransform: return commandData.Value.playerTransform.gameObject;
            case TargetType.NearestEnemy: return perception.Value.GetNearestEnemy();
            case TargetType.NearestHealthPickup: return perception.Value.GetNearestHealthPickup();
            default: return null;
        }
    }
}

