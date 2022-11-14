using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class BeltManager : MonoBehaviour
{
	public int beltNum;
	
	public GameObject[]       item;
	public Sprite[]           boxes;
	public string             instrument;
	public string             instrumentEffect;
	public GameObject         launchLocation;

	public int			score;
	public GameObject	counter;
	
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
			// Only Spawn on the 1
			if (beginningBeat && AudioManager.lastBeat != 1)
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
						GameObject go;
						
						// Choose whether this is a "good" or "bad" item
						int g = Random.Range(0, 100);

						if ( g < goodProb)
						{
							// Good item
							go = Instantiate(item[0], launchLocation.transform.position, Quaternion.identity,
								this.transform);
							go.GetComponent<ItemManager>().instrument = instrument;
						}
						else
						{
							// Bad item
							go = Instantiate(item[1], launchLocation.transform.position, Quaternion.identity,
								this.transform);
						}
						// Set the image
						go.GetComponent<SpriteRenderer>().sprite = boxes[beltNum];
							
						// dividing by 180 gives us 0 for left & 1 for right
						int dir = (int)this.transform.rotation.y / 1;
						go.GetComponent<ItemManager>().direction = dir;
					}

					nextSpawn = spawnInterval - 1;
				}

				beginningBeat = false;
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
		TextMeshProUGUI tmp = GameObject.Find("Counter_" + beltNum).GetComponent<TextMeshProUGUI>();
		tmp.text = score.ToString();
		
		// Send to the GameManager for total score
		GameManager.S.UpdateScore(amount);
	}
	
	public void ResetScore()
	{
		// Subtract the former score from the overall score
		GameManager.S.UpdateScore(-score);
		
		// Then reset the score to zero
		score = 0;
	}
	
	
}
