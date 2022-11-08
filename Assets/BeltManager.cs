using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeltManager : MonoBehaviour
{
	public GameObject item;
	public GameObject launchLocation;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(CreateItem());
	}

	// Update is called once per frame
	void Update()
	{

	}

	public IEnumerator CreateItem()
	{
		Instantiate(item, launchLocation.transform.position, Quaternion.identity, this.transform);
		yield return new WaitForSeconds(1);
		StartCoroutine(CreateItem());
	}
}
