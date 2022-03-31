using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{ 
	public float EatenPercentage; 
	public Rigidbody rigidbody;
	public float Size;
    // Start is called before the first frame update
    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
		Eat(0f);
    } 

	public float Eat(float percentageToEat)
	{ 
		float oldSize = EatenPercentage * Size;
		EatenPercentage -= percentageToEat;

		float newSize = EatenPercentage * Size;
		

		transform.localScale = newSize * Vector3.one;

		if(rigidbody != null)
			rigidbody.mass = newSize;

		if (EatenPercentage <= 0f)
			Destroy(this.gameObject);

		return oldSize - newSize;
	}
}
