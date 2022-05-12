using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class HighScoreScript : Levels
{
    public TextMeshProUGUI bestScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bestScore.text = Convert.ToString(highScore);
    }
}
