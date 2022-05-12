using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : Levels
{
    //Variable Declarations
    Rigidbody2D r;
    int coinSpeed = -6, xRoSpeed = 360;
    GameObject coinPs, pad;
    public AudioSource coinCollection;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        coinPs = GameObject.Find("Coin Particle System");
        pad = GameObject.Find("Paddle");
       
       
    }

    // Update is called once per frame
    void Update()
    {
        r.MovePosition(transform.position + new Vector3(0, coinSpeed, 0) * Time.deltaTime);
        transform.Rotate(Vector3.down * xRoSpeed * Time.deltaTime);
        
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.collider.name=="Paddle")
        {
            Destroy(gameObject);
            coinList.Remove(gameObject);
            if (doubleCurrency == true)
            {
                coins += 2;
            }
            else
            {
                coins++;
            }

            Instantiate(coinPs, paddle.transform.position, Quaternion.identity);
            coinCollection.Play();
        }
        if (collisionInfo.collider.name == "Floor")
        {
            Destroy(gameObject);
            coinList.Remove(gameObject);
            
        }
    }
}
