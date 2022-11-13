using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public static class AudioHelper 
{
	// Helper Functions - Thanks to Allessandro Fama for the idea: https://alessandrofama.com/tutorials/fmod/unity/playoneshot-parameters
	
	public static void PlayOneShotWithParameters(string fmodEvent, Vector3 position, params(string name, float value)[] parameters) {
		FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

		foreach(var (name, value) in parameters) 
		{
			instance.setParameterByName(name, value);
		}

		instance.set3DAttributes(position.To3DAttributes());
		instance.start();
		instance.release();
	}
  
}