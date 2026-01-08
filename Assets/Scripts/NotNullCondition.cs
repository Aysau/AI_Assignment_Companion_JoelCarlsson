using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "NotNull", story: "[Target] is not null", category: "Variable Conditions", id: "6703f8b9a577d8696802f9432517e059")]
public partial class NotNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {

        return !ReferenceEquals(Target?.Value, null);
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
