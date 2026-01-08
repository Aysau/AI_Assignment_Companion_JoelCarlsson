using System;
using Unity.Behavior;

[BlackboardEnum]
public enum CurrentCommand
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
