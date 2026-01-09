using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetIsNull", story: "[Target] is Null", category: "Conditions", id: "0f38de49afa2719d7eaf1dab7b2d384b")]
public partial class TargetIsNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        if (Target.Type.IsValueType)
        {
            return false;
        }
        return Target.ObjectValue.Equals(null);
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
