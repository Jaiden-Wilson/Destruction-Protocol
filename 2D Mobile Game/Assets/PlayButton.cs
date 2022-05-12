using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    GameObject cam;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        rb = cam.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnClick()
    {
        rb.MovePosition(new Vector3(0, 0, 0));
    }
}
