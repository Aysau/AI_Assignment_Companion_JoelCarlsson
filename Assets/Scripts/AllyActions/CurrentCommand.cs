using System;
using Unity.Behavior;

[BlackboardEnum]
public enum CurrentCommand //Specically an enum for the blackboard and tree to make implementation easier
{
	None,
	Wait,
	AttackTarget,
	Interact,
	Flee,
	DefendSelf,
	DefendPlayer,
	AssistPlayer
}
