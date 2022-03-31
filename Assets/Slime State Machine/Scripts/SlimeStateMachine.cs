using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class SlimeStateMachine : MonoBehaviour
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

	public IState<SlimeStateMachine> CurrentState;

	public List<ScriptableObject> AvailableStates;

	private void Start()
	{
		if(AvailableStates.Any())
			CurrentState = AvailableStates[0] as IState<SlimeStateMachine>;
	}

	private void Update()
    {
		Age += Time.deltaTime;

		var fruitColliders = Physics.OverlapBox(BoxCollider.transform.TransformPoint(BoxCollider.center), BoxCollider.size * .5f * SizeController.Size, BoxCollider.transform.rotation, FruitLayerMask.value);

		FruitInsideSlime = fruitColliders.Select(x => x.transform.parent.GetComponent<Fruit>()).ToList();

		FruitWithinSlimeRange = Physics.OverlapSphere(BoxCollider.transform.position, SlimeSeeFruitDistance, FruitLayerMask.value).Select(x => x.transform.parent.GetComponent<Fruit>()).Except(FruitInsideSlime).ToList();

		var nextState = CurrentState.CheckTransition(this);

		if(nextState != null && AvailableStates.Contains(nextState as ScriptableObject))
		{
			CurrentState = nextState;
		}
		CurrentState.PerformUpdate(this);
    }

	private void FixedUpdate()
	{
		CurrentState.PerformFixedUpdate(this);
	}
}
