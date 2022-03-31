using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSpawner : MonoBehaviour
{
	public LayerMask ClickableLayers;

	public GameObject RightClickPrefab;
	public GameObject LeftClickPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, ClickableLayers.value))
		{
			Vector3 alteredHitPosition = hit.point;
			alteredHitPosition.y = .5f;
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A))
			{
				Instantiate(LeftClickPrefab, alteredHitPosition, Quaternion.identity);
			}
			else if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.D))
			{
				Instantiate(RightClickPrefab, alteredHitPosition, Quaternion.identity); 
			}
		}
    }
}
