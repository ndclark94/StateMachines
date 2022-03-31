using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSizeController : MonoBehaviour
{
	public Rigidbody Rigidbody;
	public float size;
	public float MassMultiplier;
	public float Size
	{
		get => size;
		set
		{
			size = value;
			Rigidbody.mass = Mathf.Pow(value * MassMultiplier, 3);
			transform.localScale = size * Vector3.one;
		}
	} 
}
