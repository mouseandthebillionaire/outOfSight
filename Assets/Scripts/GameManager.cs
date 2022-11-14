using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;

    public static GameManager S;
    
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        float progressionChange = score;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Progression", progressionChange);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ConveyorBeltSequencer");
    }
}
