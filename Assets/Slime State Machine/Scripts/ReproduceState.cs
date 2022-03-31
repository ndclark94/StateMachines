using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeState/Reproduce")]
public class ReproduceState : ScriptableObject, IState<SlimeStateMachine>
{
	public DeathState Death;

	public IState<SlimeStateMachine> CheckTransition(SlimeStateMachine context)
	{
		return Death;
	}

	public void PerformFixedUpdate(SlimeStateMachine context)
	{
	}

	public void PerformUpdate(SlimeStateMachine context)
	{ 
		context.transform.GetChild(0).gameObject.SetActive(false);
		Vector3 leftSpawnPosition = context.transform.position + (context.transform.forward * .5f) + (context.transform.right * .5f);
		leftSpawnPosition.y = .5f;

		Vector3 rightSpawnPosition = context.transform.position - (context.transform.forward * .5f) - (context.transform.right * .5f);
		rightSpawnPosition.y = .5f;
		SlimeSpawner.SpawnSlime(leftSpawnPosition, context.transform.rotation);
		SlimeSpawner.SpawnSlime(rightSpawnPosition, context.transform.rotation);
	}
}
