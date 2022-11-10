using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeltManager : MonoBehaviour
{
	public GameObject   item;
	public string       instrument;
	public GameObject   launchLocation;

	public string direction;

	public  int   spawnInterval = 5;
	public  float spawnProb = 75f;
	private int   nextSpawn = 0;
	
	public  int beltSpeed = 5;
	private int nextUpdate = 0;

	// Start is called before the first frame update
	private void Awake()
	{
		nextSpawn = spawnInterval;
		nextUpdate = beltSpeed;
		AudioManager.beatUpdated += CreateItem;
		AudioManager.beatUpdated += MoveItems;
	}

	private void OnDestroy()
	{
		AudioManager.beatUpdated -= CreateItem;
		AudioManager.beatUpdated -= MoveItems;

	}

	private void CreateItem()
	{
		if (nextSpawn > 0)
		{
			nextSpawn--;
		}
		else
		{
			int r = Random.Range(0,100);
			if (r < spawnProb)
			{
				GameObject go = Instantiate(item, launchLocation.transform.position, Quaternion.identity,
					this.transform);
				go.GetComponent<ItemManager>().instrument = instrument;

				Debug.Log("Spawn");
			}
			nextSpawn = spawnInterval - 1;
		}
	}
	
	private void MoveItems()
	{
		if (nextUpdate > 0)
		{
			nextUpdate--;
		}
		else
		{
			BroadcastMessage("UpdatePosition");
			nextUpdate = beltSpeed - 1;
		}
		
	}
}
