using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SlimeState/Death")]
public class DeathState : ScriptableObject, IState<SlimeStateMachine>
{
	public void PerformFixedUpdate(SlimeStateMachine context)
	{
	}

	public void PerformUpdate(SlimeStateMachine context)
	{
		Object.Destroy(context.gameObject);
	}

	public IState<SlimeStateMachine> CheckTransition(SlimeStateMachine context)
	{
		return null;
	}
}
