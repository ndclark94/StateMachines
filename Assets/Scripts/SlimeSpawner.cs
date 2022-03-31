using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
	public static GameObject StaticSlimePrefab;
	public GameObject SlimePrefab;
	public static void SpawnSlime(Vector3 position, Quaternion rotation)
	{
		Instantiate(StaticSlimePrefab, position, rotation);
	}

	private void Start()
    {
		StaticSlimePrefab = SlimePrefab;
    } 
}
