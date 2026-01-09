using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsCloseToTarget", story: "Checks if close enough to attack", category: "Conditions", id: "4b84ff90fdeb907c01d8bd31e9b8b4cb")]
public partial class IsCloseToTargetCondition : Condition
{

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
