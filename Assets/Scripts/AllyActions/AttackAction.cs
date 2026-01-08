using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Agent] attacks [target]", category: "Action", id: "3106059fbb0ed7b7fa1ae859651bc7b4")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> damageAmount;
    [SerializeReference] public BlackboardVariable<float> attackCooldown;

    float currentAttackCooldown;

    Health targetHealth;

    protected override Status OnStart()
    {
        return Status.Running;
       
    }

    protected override Status OnUpdate()
    {
        if(currentAttackCooldown == 0)
        {
            targetHealth = Target.Value.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
                currentAttackCooldown = attackCooldown;
                Debug.Log($"[Guard] attacked {Target.Value.name} for {damageAmount.Value} damage");
            }
            else
            {
                Debug.LogError("No health script attached to " + Target.Name);
                return Status.Failure;
            }
        }
        else
        {
            currentAttackCooldown = Mathf.Max(currentAttackCooldown - Time.deltaTime, 0);

        }
        return Status.Success;
    }

}

