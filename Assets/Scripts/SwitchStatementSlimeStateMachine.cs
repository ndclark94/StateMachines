using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;

public class SwitchStatementSlimeStateMachine : MonoBehaviour
{
	public Rigidbody Rigidbody;
	public BoxCollider BoxCollider;
	public SlimeSizeController SizeController;
	public List<Fruit> FruitInsideSlime;
	public List<Fruit> FruitWithinSlimeRange;
	public float SlimeSeeFruitDistance = 4f;
	public LayerMask FruitLayerMask;
	[ReadOnly] public float Age;
	public float LifeSpan = 45f; 

	public float DigestionRate;
	public float EatForce = 10f; 

	public float MoveSpeed;
	public Vector3 MoveToDirection; 

	public SlimeState CurrentState;

	public enum SlimeState
	{
		Move,
		Eat,
		Reproduce,
		Death
	}

	private void Update()
    { 
		Age += Time.deltaTime; 

		var fruitColliders = Physics.OverlapBox(BoxCollider.transform.TransformPoint(BoxCollider.center), BoxCollider.size * .5f * SizeController.Size, BoxCollider.transform.rotation, FruitLayerMask.value);

		FruitInsideSlime = fruitColliders.Select(x=> x.transform.parent.GetComponent<Fruit>()).ToList();

		FruitWithinSlimeRange = Physics.OverlapSphere(BoxCollider.transform.position, SlimeSeeFruitDistance, FruitLayerMask.value).Select(x => x.transform.parent.GetComponent<Fruit>()).Except(FruitInsideSlime).ToList();

		//Transitions
		switch (CurrentState)
		{
			case SlimeState.Move:
				if (FruitInsideSlime.Any())
					CurrentState = SlimeState.Eat;

				if (Age > LifeSpan)
					CurrentState = SlimeState.Death;
				break;
			case SlimeState.Eat:
				if (SizeController.Size >= 2f)
					CurrentState = SlimeState.Reproduce;

				if (Age > LifeSpan)
					CurrentState = SlimeState.Death;
				 
				if (!FruitInsideSlime.Any())
					CurrentState = SlimeState.Move;

				break;
			case SlimeState.Reproduce:
				CurrentState = SlimeState.Death;
				break;
			case SlimeState.Death: 
				break;
		}

		//State Update
		switch (CurrentState)
		{
			case SlimeState.Move:
				break;
			case SlimeState.Eat:
				float eatenMass = DigestionRate * Time.deltaTime;
				foreach (Fruit fruit in FruitInsideSlime)
				{
					if (fruit != null)
					{
						SizeController.Size += fruit.Eat(eatenMass); 
					}
				}
				break;
			case SlimeState.Reproduce:
				transform.GetChild(0).gameObject.SetActive(false);
				Vector3 leftSpawnPosition = transform.position + (transform.forward * .5f) + (transform.right * .5f);
				leftSpawnPosition.y = .5f;

				Vector3 rightSpawnPosition = transform.position - (transform.forward * .5f) - (transform.right * .5f);
				rightSpawnPosition.y = .5f;
				SlimeSpawner.SpawnSlime(leftSpawnPosition, transform.rotation);
				SlimeSpawner.SpawnSlime(rightSpawnPosition, transform.rotation);
				break;
			case SlimeState.Death:
				Destroy(this.gameObject);
				break;
		}
	}

	private void FixedUpdate()
	{
		switch (CurrentState)
		{
			case SlimeState.Move: 
				MoveToDirection = Vector3.zero;
				if(FruitWithinSlimeRange.Any(x => x != null))
				{
					Fruit closest = FruitWithinSlimeRange.Where(x => x != null).OrderBy(x => Vector3.Distance(Rigidbody.position, x.rigidbody.position)).First();
					
					MoveToDirection = closest.rigidbody.position;
					MoveToDirection.y = SizeController.Size * .5f;

					if (Rigidbody != null && MoveToDirection != Vector3.zero)
					{
						Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, MoveToDirection, MoveSpeed * Time.fixedDeltaTime));
					} 
				}
				break;
			case SlimeState.Eat:
				foreach(Fruit fruit in FruitInsideSlime)
				{
					if (fruit != null)
					{
						Vector3 targetDirection = BoxCollider.transform.position - fruit.rigidbody.position;
						if(targetDirection.magnitude > .2f)
							fruit.rigidbody.velocity = targetDirection.normalized * EatForce;
					}
				} 
				break;
			case SlimeState.Reproduce:
				break;
			case SlimeState.Death:
				break;
		}
	}
}
