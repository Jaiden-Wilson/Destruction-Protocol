using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIBehaviour2 : Levels
{
    public TextMeshProUGUI finalScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        finalScore.text=""+(basePoints+bonusPoints);
    }
}
