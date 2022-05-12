using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class PlayButtonScript : MonoBehaviour
{
    public static GameObject cam, store, conf_menu, healthBar, purchase_btn, conf_query,error, error_message,x, success;
    Rigidbody2D camRB;
    Vector3 mainMenuPos,sector1Pos, lastPos;
    public static bool pressedPlay, defenseSec, padSec, ballSec, powSec, itemClicked,rfClicked, dsClicked , invClicked, u1Clicked, u2Clicked, u3Clicked,upgradesEnabled, gameToStore, mainToStore;
    public static bool purchase_Confirmed;
    public static int basePoints, bonusPoints, cost,coins;
    public static List<GameObject> coinList;
    public static int level, health,shield;
    public static float originalHealthBarScale, originalShieldScale;
    public static List<GameObject> paddleUpgrades, defenses;
    // Start is called before the first frame update
    void Start()
    {
        pressedPlay = false;
        cam = GameObject.Find("Main Camera");
        camRB = cam.GetComponent<Rigidbody2D>();
        mainMenuPos= new Vector3(-50, 0, 0);
        sector1Pos = new Vector3(0, 0, 0);
        coinList = new List<GameObject>();
        store = GameObject.Find("D-Mart"); 
        purchase_btn = GameObject.Find("Purchase");
        error = GameObject.Find("Error");
        
        upgradesEnabled = false;
        error.GetComponent<SpriteRenderer>().enabled = false;
        error.GetComponentsInChildren<Image>()[0].enabled = false;
        //Store Category Lists

        //Paddle Section
        paddleUpgrades = new List<GameObject>();
        paddleUpgrades.Add(GameObject.Find("Upgrade1"));
        paddleUpgrades.Add(GameObject.Find("Upgrade2"));
        paddleUpgrades.Add(GameObject.Find("Upgrade3"));
        disableUpgrades();

        //Defense Section
        defenses = new List<GameObject>();
        defenses.Add(GameObject.Find("Refill"));
        defenses.Add(GameObject.Find("DoubleShield"));
        defenses.Add(GameObject.Find("Invincibility"));
        //Store section initializations
        defenseSec = true;
        padSec = false;
        ballSec = false;
        powSec = false;
        itemClicked = false;
        rfClicked = false;
        dsClicked = false;
        invClicked = false;
        u1Clicked = false;
        u2Clicked = false;
        u3Clicked = false;
        lastPos = mainMenuPos;
        gameToStore = false;
        mainToStore = false;
        //
        conf_menu = GameObject.Find("Confirmation");
        conf_query = GameObject.Find("Confirmation_Query");
        hideConfirmationMenu();
        error_message = GameObject.Find("Error_Message");
        error_message.GetComponent<TextMeshProUGUI>().enabled = false;
        x = GameObject.Find("Exit (store)");
        success = GameObject.Find("Success");
        hideAcceptanceMessage();
        purchase_Confirmed = false;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void gamePlay()
    {
        camRB.MovePosition(sector1Pos);
        pressedPlay = true;
        basePoints = 0;
    }
    public void returnToGame()
    {
        camRB.MovePosition(sector1Pos);
        pressedPlay = true;
    }
    public void mainMenu()
    {
        camRB.MovePosition(mainMenuPos);
        cam.GetComponent<Camera>().orthographicSize = 9.693f;

    }
    public void shop()
    {
        camRB.MovePosition(store.transform.position);
        cam.GetComponent<Camera>().orthographicSize = 5.5f;
    }
    public void defense()
    {
        defenseSec = true;
        padSec = false;
        ballSec = false;
        powSec = false;
        disableUpgrades();
        enableDefenses();

    }
    public void pad()
    {
        defenseSec = false;
        padSec = true;
        ballSec = false;
        powSec = false;
        disableDefenses();
        enableUpgrades();
    }
    public void ball()
    {
        defenseSec = false;
        padSec = false;
        ballSec = true;
        powSec = false;
    }
    public void pow()
    {
        defenseSec = false;
        padSec = false;
        ballSec = false;
        powSec = true;
    }
    public void refill()
    {
        itemClicked = true;
        rfClicked = true;
        dsClicked = false;
        invClicked = false;
        cost = 8;
    }
    public void doubleShield()
    {
        itemClicked = true;

        dsClicked = true;
        rfClicked = false;
        invClicked = false;
        cost = 12;
    }
    public void invincibility()
    {
        itemClicked = true;

        invClicked = true;
        rfClicked = false;
        dsClicked = false;
        cost = 16;
    }
    public void u1()
    {
        itemClicked = true;
        u1Clicked = true;
        u2Clicked = false;
        u3Clicked = false;
        invClicked = false;
        rfClicked = false;
        dsClicked = false;
        cost = 20;
    }
    public void u2()
    {
        itemClicked = true;
        u1Clicked = false;
        u2Clicked = true;
        u3Clicked = false;
        invClicked = false;
        rfClicked = false;
        dsClicked = false;
        cost = 24;
    }
    public void u3()
    {
        itemClicked = true;
        u1Clicked = false;
        u2Clicked = false;
        u3Clicked = true;
        invClicked = false;
        rfClicked = false;
        dsClicked = false;
        cost = 28;
    }
    public void exit()
    {
        if(gameToStore==true)
        {
            camRB.MovePosition(sector1Pos);
        }
        else
        {
            camRB.MovePosition(mainMenuPos);

        }
        cam.GetComponent<Camera>().orthographicSize = 9.693f;

    }
    public void storeFromGame()
    {
        gameToStore = true;
        mainToStore = false;
    }
    public void storeFromMain()
    {
        mainToStore = true;
        gameToStore = false;
    }
    public void purchase()
    {

        hideStoreElements();
        if (itemClicked==true)
        {
            if(coins>=cost)
            {
                showConfirmationMenu();
            }
            else
            {
                showErrorMessage();
            }
        }
    }
    public void confirmPurchase()
    {
        purchase_Confirmed = true;
        hideConfirmationMenu();
        showStoreElements();
    }
    public void denyPurchase()
    {
        purchase_Confirmed = false;
        hideConfirmationMenu();
        showStoreElements();
    }
    public void hideStoreElements()
    {
        purchase_btn.GetComponent<Button>().interactable = false;
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("ItemHeaders").Length; i++)
        {
            GameObject.FindGameObjectsWithTag("ItemHeaders")[i].GetComponent<TextMeshProUGUI>().enabled = false;
        }
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("StoreItems").Length; i++)
        {
            GameObject.FindGameObjectsWithTag("StoreItems")[i].GetComponent<Image>().enabled = false;
        }

    }
    public void showStoreElements()
    {
        purchase_btn.GetComponent<Button>().interactable = true;
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("ItemHeaders").Length; i++)
        {
            GameObject.FindGameObjectsWithTag("ItemHeaders")[i].GetComponent<TextMeshProUGUI>().enabled = true;
        }
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("StoreItems").Length; i++)
        {
            GameObject.FindGameObjectsWithTag("StoreItems")[i].GetComponent<Image>().enabled = true;
        }
    }
    public void showConfirmationMenu()
    {
        x.GetComponent<Button>().interactable = false;

        conf_query.GetComponent<TextMeshProUGUI>().enabled = true;
        if (rfClicked==true)
        {
            conf_query.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to purchase a refill?";
        }
        else if(dsClicked==true)
        {
            conf_query.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to purchase double-shield?";
        }
        else if(invClicked==true)
        {
            conf_query.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to purchase invincibility?";

        }
        else if (u1Clicked == true)
        {
            conf_query.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to purchase the Breadmaker?";

        }
        else if (u2Clicked == true)
        {
            conf_query.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to purchase the Teleporter?";

        }
        conf_menu.GetComponent<SpriteRenderer>().enabled = true;
        for (int i = 0; i < 2; i++)
        {
            conf_menu.GetComponentsInChildren<Button>()[i].interactable = true;
            conf_menu.GetComponentsInChildren<Image>()[i].enabled = true;

        }
    }
    public void hideConfirmationMenu()
    {
        conf_menu.GetComponent<SpriteRenderer>().enabled = false;
        conf_query.GetComponent<TextMeshProUGUI>().enabled = false;

        for (int i = 0; i < 2; i++)
        {
            conf_menu.GetComponentsInChildren<Button>()[i].interactable = false;
            conf_menu.GetComponentsInChildren<Image>()[i].enabled = false;

        }
    }
    public void hideErrorMessage()
    {
        x.GetComponent<Button>().interactable = true;
        error.GetComponent<SpriteRenderer>().enabled = false;
        error.GetComponentsInChildren<Image>()[0].enabled = false;
        error_message.GetComponent<TextMeshProUGUI>().enabled = false;

    }
    public void showErrorMessage()
    {
        hideStoreElements();
        hideConfirmationMenu();
        x.GetComponent<Button>().interactable = false;
        error.GetComponent<SpriteRenderer>().enabled = true;
        error.GetComponentsInChildren<Image>()[0].enabled = true;
        error_message.GetComponent<TextMeshProUGUI>().enabled = true;
        error_message.GetComponent<TextMeshProUGUI>().text = "Sorry, you have insufficient funds to purchase this item.";


    }
    public void hideAcceptanceMessage()
    {
        x.GetComponent<Button>().interactable = true;

        success.GetComponent<SpriteRenderer>().enabled = false;
        success.GetComponentsInChildren<Button>()[0].interactable = false;
        success.GetComponentsInChildren<Image>()[0].enabled = false;
        success.GetComponentsInChildren<TextMeshProUGUI>()[0].enabled = false;
    }
    public void showAcceptanceMessage()
    {
        hideStoreElements();

        success.GetComponent<SpriteRenderer>().enabled = true;
        success.GetComponentsInChildren<Button>()[0].interactable = true;
        success.GetComponentsInChildren<Image>()[0].enabled = true;
        success.GetComponentsInChildren<TextMeshProUGUI>()[0].enabled = true;
        success.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Transaction Complete!";

    }
    public void disableUpgrades()
    {
        for (int i = 0; i < paddleUpgrades.Count; i++)
        {
            paddleUpgrades.ElementAt(i).GetComponent<Image>().enabled=false;
            paddleUpgrades.ElementAt(i).GetComponent<Button>().interactable = false;

        }
    }
    public void enableUpgrades()
    {
        upgradesEnabled = true;
        for (int i = 0; i < paddleUpgrades.Count; i++)
        {
            paddleUpgrades.ElementAt(i).GetComponent<Image>().enabled = true;
            paddleUpgrades.ElementAt(i).GetComponent<Button>().interactable = true;
        }
    }
    public void disableDefenses()
    {
        for (int i = 0; i < paddleUpgrades.Count; i++)
        {
            defenses.ElementAt(i).GetComponent<Image>().enabled = false;
            defenses.ElementAt(i).GetComponent<Button>().interactable = false;
        }
    }
    public void enableDefenses()
    {
        for (int i = 0; i < paddleUpgrades.Count; i++)
        {
            defenses.ElementAt(i).GetComponent<Image>().enabled = true;
            defenses.ElementAt(i).GetComponent<Button>().interactable = true;
        }
    }
}
