using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeState/Move")]
public class MoveState : ScriptableObject, IState<SlimeStateMachine>
{ 
	public float MoveSpeed;
	public Vector3 MoveToDirection;

	public EatState Eat;
	public DeathState Death;

	public IState<SlimeStateMachine> CheckTransition(SlimeStateMachine context)
	{
		if (context.FruitInsideSlime.Any())
			return Eat;

		if (context.Age > context.LifeSpan)
			return Death;

		return null;
	}

	public void PerformFixedUpdate(SlimeStateMachine context)
	{
		MoveToDirection = Vector3.zero;
		if (context.FruitWithinSlimeRange.Any(x => x != null))
		{
			Fruit closest = context.FruitWithinSlimeRange.Where(x => x != null).OrderBy(x => Vector3.Distance(context.Rigidbody.position, x.rigidbody.position)).First();

			MoveToDirection = closest.rigidbody.position;
			MoveToDirection.y = context.SizeController.Size * .5f;

			if (context.Rigidbody != null && MoveToDirection != Vector3.zero)
			{
				context.Rigidbody.MovePosition(Vector3.MoveTowards(context.Rigidbody.position, MoveToDirection, MoveSpeed * Time.fixedDeltaTime));
			}
		}
	}

	public void PerformUpdate(SlimeStateMachine context)
	{ 
	}
}
