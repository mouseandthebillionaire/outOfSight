using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Random;

public class BeltManager : MonoBehaviour
{
	public int beltNum;
	
	public GameObject[] item;
	public string       instrument;
	public GameObject   launchLocation;

	public int				score;
	
	//public string direction;

	// We want to make sure that the very first beat is on the 1
	private bool beginningBeat = true;

	public  int   spawnInterval = 5;
	public  float spawnProb     = 75f;
	public  float goodProb		= 75f;
	private int   nextSpawn     = 0;
	
	public  int  beltSpeed   = 5;
	private bool beltRunning = true;
	private int  nextUpdate  = 0;

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
		// Are we paused?
		if (!beltRunning) return;
		else {
			if (beginningBeat && AudioManager.lastBeat != 8)
			{
				return;
			}
			else
			{
				if (nextSpawn > 0)
				{
					nextSpawn--;
				}
				else
				{
					// Do we even Spawn now?
					int r = Random.Range(0, 100);
					if (r < spawnProb)
					{
						// Choose whether this is a "good" or "bad" item
						int g = Random.Range(0, 100);
						if ( g < goodProb)
						{
							// Good item
							GameObject go = Instantiate(item[0], launchLocation.transform.position, Quaternion.identity,
								this.transform);
							go.GetComponent<ItemManager>().instrument = instrument;

						}
						else
						{
							// Bad item
							GameObject go = Instantiate(item[1], launchLocation.transform.position, Quaternion.identity,
								this.transform);
						}

					}

					nextSpawn = spawnInterval - 1;
				}

				beginningBeat = false;
			}
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

			if (hit.collider != null)
			{
				if(hit.collider.gameObject.name == "On/Off_" + beltNum) StartStop();
				if(hit.collider.gameObject.name == "Up_" + beltNum) SpeedUp();
				if(hit.collider.gameObject.name == "Down_" + beltNum) SlowDown();
			}
		}
	}

	private void MoveItems()
	{
		if (beltRunning)
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

	public void StartStop()
	{
		if (beltRunning) beltRunning = false;
		else beltRunning = true;
	}
	
	public void SpeedUp()
	{
		if (beltSpeed > 1)
		{
			beltSpeed = (beltSpeed / 2);
			spawnInterval = (spawnInterval / 2);
		}
	}
	
	public void SlowDown()
	{
		if (beltSpeed < 8)
		{
			beltSpeed = (beltSpeed * 2);
			spawnInterval = (spawnInterval * 2);
		}
	}
	
	public void UpdateScore(int amount)
	{
		score += amount;
	}
}
