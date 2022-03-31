using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeState/Eat")]
public class EatState : ScriptableObject, IState<SlimeStateMachine>
{
	public float DigestionRate;
	public float EatForce = 10f;

	public MoveState Move;
	public ReproduceState Reproduce;
	public DeathState Death;

	public void PerformFixedUpdate(SlimeStateMachine context)
	{
		foreach (Fruit fruit in context.FruitInsideSlime)
		{
			if (fruit != null)
			{
				Vector3 targetDirection = context.BoxCollider.transform.position - fruit.rigidbody.position;
				if (targetDirection.magnitude > .2f)
					fruit.rigidbody.velocity = targetDirection.normalized * EatForce;
			}
		}
	}

	public void PerformUpdate(SlimeStateMachine context)
	{
		float eatenMass = DigestionRate * Time.deltaTime;
		foreach (Fruit fruit in context.FruitInsideSlime)
		{
			if (fruit != null)
			{
				context.SizeController.Size += fruit.Eat(eatenMass);
			}
		}
	}

	public IState<SlimeStateMachine> CheckTransition(SlimeStateMachine context)
	{
		if (context.SizeController.Size >= 2f)
			return Reproduce;

		if (context.Age > context.LifeSpan)
			return Death;

		if (!context.FruitInsideSlime.Any())
			return Move;

		return null;
	}
}
