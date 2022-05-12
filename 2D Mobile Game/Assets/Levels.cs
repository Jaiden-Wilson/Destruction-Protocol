using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using FirstGearGames.SmoothCameraShaker;
public class Levels : PlayButtonScript
{
    //Variable declarations 

    //Lists for block gameobjects, classes, and instance ids
    public static List<GameObject> blocks, sapphireBlocks, emeraldBlocks, rubyBlocks, diamondBlocks, allBlocks, capsules, ballClones;
    public static List<int> idList, sapphireIdList, emeraldIdList, rubyIdList, diamondIdList, augmentedIdList, ballCloneIdList;
    public static List<All> allObjects;
    public static List<Sapphire> sapphireObjects;
    public static List<Emerald> emeraldObjects;
    public static List<Ruby> rubyObjects;
    public static List<Diamond> diamondObjects;

    // Focal gameobjects
    public static GameObject primaryBlock, primarySapphire, primaryEmerald, primaryRuby, primaryDiamond;
    public static GameObject healthBarOutline, shieldBar, shieldStatus;
    public static GameObject ball, cam, powerUpCapsule, missile, colPad;
    public static GameObject paddle, invPs;
    public static GameObject och, mch, ich, pause_menu;


    //Object shaker modules for each block type
    public static List<ObjectShaker> sapphireShakers, emeraldShakers, rubyShakers, diamondShakers;
    public static List<PowerUps> powerUps;
    public static List<Ball> activeBalls;

   
    // Reused positions and scales for gameobjects
    public static Vector3 originalBallPos, originalPaddlePos, originalHealthBarPos;
    public static Vector3 lmp, originalPaddleScale;

    //Variables for switching and reverting ball and paddle states
    public static bool ballReleased = false, shieldActivated, invincibility, healthActivated;
    public static bool paddleActivated = false, lasersActivated, KEActivated, doubleCurrency, teleportation;
    static string activePaddle;
    public static Sprite ogPadSprite;

    //Numerical data
    public static int highScore=0,powerUpBonus, KEBonus,  missileBonus,multiBonus;
    public static int lives,  deltaPoints, startingPoints, pointIncrease,resVel, dmgTol,shieldDmgTol,hitCount;
    public static float theta = Mathf.PI / 4;
    Rigidbody2D rigBod;
    //Power-up related variables
    public static bool missileActivated, missileLaunched,lasersLaunched;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiation of block lists
        blocks = new List<GameObject>();
        sapphireBlocks = new List<GameObject>();
        emeraldBlocks = new List<GameObject>();
        rubyBlocks = new List<GameObject>();
        diamondBlocks = new List<GameObject>();
        capsules = new List<GameObject>();
        allBlocks = new List<GameObject>();
        ballClones = new List<GameObject>(10);
        ball = GameObject.Find("Ball");
        ballClones.Add(ball);
        powerUps = new List<PowerUps>();
        //Instantiation of idLists
        idList = new List<int>();
        sapphireIdList = new List<int>();
        emeraldIdList = new List<int>();
        rubyIdList = new List<int>();
        diamondIdList = new List<int>();
        augmentedIdList = new List<int>();
        ballCloneIdList = new List<int>();
        ballCloneIdList.Add(ballClones.ElementAt(0).GetComponent<BoxCollider2D>().GetInstanceID());
        //Instantiation and population of gem lists
        allObjects = new List<All>();
        sapphireObjects = new List<Sapphire>();
        emeraldObjects = new List<Emerald>();
        rubyObjects = new List<Ruby>();
        diamondObjects = new List<Diamond>();
        //Instantiation of objectshaker lists for sapphires, emeralds, rubies and diamonds
        sapphireShakers = new List<ObjectShaker>();
        emeraldShakers = new List<ObjectShaker>();
        rubyShakers = new List<ObjectShaker>();
        diamondShakers = new List<ObjectShaker>();

        //gameobject assignments
        healthBarOutline = GameObject.Find("Health Bar");
        originalPaddleScale = GameObject.Find("Paddle").transform.localScale;
        healthBar = GameObject.Find("H");
        powerUpCapsule = GameObject.Find("powerUpCapsule");
        primaryBlock = GameObject.Find("Block");
        primarySapphire = GameObject.Find("Sapphire");
        primaryEmerald = GameObject.Find("Emerald");
        primaryRuby = GameObject.Find("Ruby");
        primaryDiamond = GameObject.Find("Diamond");
        paddle = GameObject.Find("Paddle");
        cam = GameObject.Find("Main Camera");
        colPad = GameObject.Find("ColliderPadding");
        missile = GameObject.Find("missile_red");
        invPs = GameObject.Find("InvincibilityPS");
        och = GameObject.Find("OuterCrossHair");
        mch = GameObject.Find("MiddleCrossHair");
        ich = GameObject.Find("Circle");
        shieldBar = GameObject.Find("shield_bar");
        shieldStatus = GameObject.Find("S");
        pause_menu = GameObject.Find("pause_menu");
        pause_menu.SetActive(false);

        //Position and scale initializations
        originalBallPos = new Vector3(0, -6.33f, 0);
        originalPaddlePos = new Vector3(0, -6.84f, 0);
        lmp = new Vector3(50, 0, 0);
        originalShieldScale = shieldStatus.transform.localScale.x;
        originalHealthBarScale = healthBar.transform.localScale.x;
        originalHealthBarPos = healthBar.transform.localPosition;

        //Paddle and ball state initializations
        activePaddle = "default";
        lasersActivated = false;
        lasersLaunched = false;
        KEActivated = false;
        invPs.SetActive(false);
        missileLaunched = false;
        healthActivated = true;
        shieldActivated = false;
        missileActivated = false;
        invincibility = false;
        doubleCurrency = false;
        teleportation = false;
        ogPadSprite = paddle.GetComponent<SpriteRenderer>().sprite;
        
        //Numerical data
        powerUpBonus = 15;
        KEBonus = 30;
        multiBonus = 10;
        missileBonus = 50;
        pointIncrease = 0;
        basePoints = 0;
        bonusPoints = 0;
        lives = 5;
        coins = 100;
        hitCount = 0;
        health = 30;
        shield = 22;
        dmgTol = 30;
        shieldDmgTol = 22;
        //
        rigBod = cam.GetComponent<Rigidbody2D>();
        activeBalls = new List<Ball>();
        activeBalls.Add(new Ball());
        disableShield();
        levelOne();
    }

    // Update is called once per frame
    void Update()
    {
        applyPurchases();
       if(allBlocks.Count==0)
        {
            
            for (int i = 0; i < coinList.Count; i++)
            {
                Destroy(coinList.ElementAt(i));
               
            }
            coinList.Clear();
        }
       if(Input.GetKey("i"))
        {
            paddleActivated = false;
            pressedPlay = false;
            pause_menu.SetActive(true);
        }
       
        nextLevel();
        restart();
        if (pressedPlay == true)
            pause_menu.SetActive(false);
    }
    public void restart()
    {
        if(basePoints>highScore)
        {
            highScore = basePoints;
        }
        if(lives==0||health==0)
        {
            pressedPlay = false;
           
            for (int i = 0; i < blocks.Count; i++)
            {
                Destroy(blocks.ElementAt(i));
            }
            for (int i = 0; i < sapphireBlocks.Count; i++)
            {
                Destroy(sapphireBlocks.ElementAt(i));
            }
            for (int i = 0; i < emeraldBlocks.Count; i++)
            {
                Destroy(emeraldBlocks.ElementAt(i));
            }
            for (int i = 0; i < rubyBlocks.Count; i++)
            {
                Destroy(rubyBlocks.ElementAt(i));
            }
            for (int i = 0; i < diamondBlocks.Count; i++)
            {
                Destroy(diamondBlocks.ElementAt(i));
            }
            clearAll();
            
            
            lives = 5;
            coins = 0;
            health = 30;
            healthBar.transform.localScale = new Vector3(7.1f, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            healthBar.transform.localPosition = new Vector3(0, healthBar.transform.localPosition.y, healthBar.transform.localPosition.z);

            rigBod.MovePosition(lmp);
            levelOne();
        }
    }
    void nextLevel()
    {
        if(level==1&&basePoints==startingPoints+deltaPoints)
        {
            levelTwo();
        }
        if (level == 2 && basePoints == startingPoints + deltaPoints)
        {
            levelThree();
        }
        if (level == 3 && basePoints == startingPoints + deltaPoints)
        {
            levelFour();
        }
        if (level == 4 && basePoints == startingPoints + deltaPoints)
        {
            levelFive();
        }
        if (level == 5 && basePoints == startingPoints + deltaPoints)
        {
            levelSix();
        }
        if (level == 6 && basePoints == startingPoints + deltaPoints)
        {
            levelSeven();
        }
        if (level == 7 && basePoints == startingPoints + deltaPoints)
        {
            levelEight();
        }
        if (level == 8 && basePoints == startingPoints + deltaPoints)
        {
            levelNine();
        }
        if (level == 9 && basePoints == startingPoints + deltaPoints)
        {
            levelTen();
        }
        if (level == 10 && basePoints == startingPoints + deltaPoints)
        {
            levelEleven();
        }
        if (level == 11 && basePoints == startingPoints + deltaPoints)
        {
            levelTwelve();
        }
        if (level == 12 && basePoints == startingPoints + deltaPoints)
        {
            levelThirteen();
        }
        if (level == 13 && basePoints == startingPoints + deltaPoints)
        {
            levelFourteen();
        }
    }
    //LevelOne *Population of blocks for the first level*
    public void levelOne()
    {
        resetBallPos();
        nullifyPowerUps();

        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);

        level = 1;
        startingPoints = 0;
        for (int i=0;i<4;i++)
        {
            for(int j=0;j<5;j++)
            {
               blocks.Add( Instantiate(primaryBlock, new Vector3(-3.89f+(2.593333f*i), 6.705f-(1.5f*j), 0), Quaternion.identity));
            }
        }
        populateIdLists();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);
        addCoins();
        addPowerUps();
    }
    void levelTwo()
    {

        nullifyPowerUps();

        level = 2;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-4.39f, 6.705f, 0), Quaternion.identity));
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3 (4.39f, 6.705f, 0), Quaternion.identity));
        for (int i = 0; i < 3; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-3.39f+i,5.805f -(0.90f*i), 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(3.39f - i, 5.805f - (0.90f * i), 0), Quaternion.identity));
        }

        blocks.Add(Instantiate(primaryBlock, new Vector3(blocks.ElementAt(blocks.Count - 1).transform.position.x - 0.3f, blocks.ElementAt(blocks.Count - 1).transform.position.y - 0.95f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(blocks.ElementAt(blocks.Count - 1).transform.position.x -2.15f, blocks.ElementAt(blocks.Count - 1).transform.position.y , 0), Quaternion.identity));
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(blocks.ElementAt(blocks.Count - 1).transform.position.x + 1, blocks.ElementAt(blocks.Count - 1).transform.position.y-0.95f, 0), Quaternion.identity));
        for (int i = 1; i < 4; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(sapphireBlocks.ElementAt(2).transform.position.x-0.075f -i, sapphireBlocks.ElementAt(2).transform.position.y - 0.95f*i, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(sapphireBlocks.ElementAt(2).transform.position.x+0.075f +i, sapphireBlocks.ElementAt(2).transform.position.y - 0.95f*i, 0), Quaternion.identity));
        }
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(sapphireBlocks.ElementAt(2).transform.position.x - 4.075f , sapphireBlocks.ElementAt(2).transform.position.y - 3.8f, 0), Quaternion.identity));
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(sapphireBlocks.ElementAt(2).transform.position.x + 4.075f, sapphireBlocks.ElementAt(2).transform.position.y - 3.8f, 0), Quaternion.identity));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);


    }
    void levelThree()
    {
        nullifyPowerUps();

        level = 3;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        for(int i=0;i<2;i++)
        {
            for (int j = 0; j < 2; j++)
            {
                sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-4.39f + 1.4f * i, 6.705f - 0.57f * j, 0), Quaternion.identity));
                sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(4.39f - 1.4f * i, 6.705f - 0.57f * j, 0), Quaternion.identity));
            }
        }
       sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(0,3.855f,0), Quaternion.identity));
        for(int i=0;i<7;i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-4.2f + 1.4f*i,3.005f,0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-4.2f + 1.4f * i, 1.005f, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(0, 0.155f, 0), Quaternion.identity));
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-4.39f + 1.4f * i, -1.845f - 0.57f * j, 0), Quaternion.identity));
                sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(4.39f - 1.4f * i, -1.845f - 0.57f * j, 0), Quaternion.identity));
            }
        }
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);
    }
    void levelFour()
    {
        nullifyPowerUps();
        startingPoints = basePoints;
        level=4;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        //Form the upper '2'
        for(int i=0;i<4;i++)
        {
           sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-1.79f,6.705f-0.57f*i,0), Quaternion.identity));
           sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-3.19f, 4.955f-0.57f*i, 0), Quaternion.identity));
        }
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-3.19f, 6.705f , 0), Quaternion.identity));
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-1.79f, 3.215f, 0), Quaternion.identity));
        //Form the lower '2'
        for (int i = 0; i < 4; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.79f, -0.395f - 0.57f * i, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-3.19f, -2.105f - 0.57f * i, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(-3.19f, -0.395f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(-1.79f, -3.815f, 0), Quaternion.identity));
        //Form the zero
        for (int i = 0; i < 5; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(0.75f, 6.135f - 0.57f * i, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(3.55f, 6.135f - 0.57f * i, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(2.15f, 6.705f , 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(2.15f, 3.285f , 0), Quaternion.identity));
        //Form the one
        for (int i = 0; i < 7; i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(2.15f, -0.395f - 0.57f * i, 0), Quaternion.identity));
        }
       sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(0.75f, -0.965f, 0), Quaternion.identity));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelFive()
    {
        nullifyPowerUps();
        startingPoints = basePoints;
        level = 5;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(0, 5.045f, 0), Quaternion.identity));
        //Rows with 2 blocks
        for(int i=0;i<2;i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-0.7f+(1.4f*i),4.475f,0),Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-0.7f + (1.4f * i), -1.225f, 0), Quaternion.identity));
        }
        //Rows with 3 blocks
        for (int i = 0; i < 2; i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-1.4f + (2.8f * i), 3.905f, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-1.4f + (2.8f * i), -0.655f, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(0,3.905f,0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(0, -0.655f, 0), Quaternion.identity));
        //Rows with 4 blocks
        for (int i = 0; i < 2; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-0.7f + (1.4f * i), 3.335f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-0.7f + (1.4f * i), -0.085f, 0), Quaternion.identity));

        }
        for (int i = 0; i < 2; i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.1f + (4.2f * i), 3.335f, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.1f + (4.2f * i), -0.085f, 0), Quaternion.identity));

        }
        //Rows with 5 blocks
        for (int i = 0; i < 3; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f + (1.4f * i), 2.765f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f + (1.4f * i), 0.485f, 0), Quaternion.identity));

        }
        for (int i = 0; i < 2; i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.8f + (5.6f * i), 2.765f, 0), Quaternion.identity));
           emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.8f + (5.6f * i), 0.485f, 0), Quaternion.identity));

        }
        //Rows with 6 blocks
        for (int i = 0; i < 4; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + (1.4f * i), 2.195f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + (1.4f * i), 1.055f, 0), Quaternion.identity));

        }
        for (int i = 0; i < 2; i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-3.5f + (7.0f * i), 2.195f, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-3.5f + (7.0f * i), 1.055f, 0), Quaternion.identity));

        }
        //Middle Row
        for (int i = 0; i < 5; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.8f + (1.4f * i), 1.625f, 0), Quaternion.identity));

        }
        for (int i = 0; i < 2; i++)
        {
           emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-4.2f + (8.4f * i), 1.625f, 0), Quaternion.identity));

        }
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(0, -1.795f, 0), Quaternion.identity));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelSix()
    {
        nullifyPowerUps();
        level =6;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
       
        for (int i=0;i<13;i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(0,5-(0.57f*i),0), Quaternion.identity));
        }
        //Form the branches of the tree

        for (int i = 0; i < 4; i++)
        {
            

            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-1.4f - 0.7f * i, (blocks.ElementAt(3).transform.position.y + 0.285f) + (0.57f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(1.4f + 0.7f * i, (blocks.ElementAt(3).transform.position.y + 0.285f) + (0.57f * i), 0), Quaternion.identity));
            
        }

        for (int i = 0; i < 3; i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-1.4f - 0.7f * i, (blocks.ElementAt(8).transform.position.y + 0.285f) + (0.57f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(1.4f + 0.7f * i, (blocks.ElementAt(8).transform.position.y + 0.285f) + (0.57f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-0.7f - 0.7f * i, (blocks.ElementAt(0).transform.position.y + 0.57f) + (0.57f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(0.7f + 0.7f * i, (blocks.ElementAt(0).transform.position.y + 0.57f) + (0.57f * i), 0), Quaternion.identity));

        }
        for (int i = 0; i < 2; i++)
        {

            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.1f - 0.7f * i, (blocks.ElementAt(8).transform.position.y - 0.285f) - (0.57f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(2.1f + 0.7f * i, (blocks.ElementAt(8).transform.position.y - 0.285f) - (0.57f * i), 0), Quaternion.identity));

        }
        //Base of the Tree
        for (int i = 0; i < 5; i++)
        {

            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-2.8f + 1.4f * i, (blocks.ElementAt(12).transform.position.y - 0.57f) , 0), Quaternion.identity));

        }
        for (int i = 0; i < 4; i++)
        {

            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-2.1f + 1.4f * i, (blocks.ElementAt(12).transform.position.y - 1.14f), 0), Quaternion.identity));

        }
        for (int i = 0; i < 3; i++)
        {

            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-1.4f + 1.4f * i, (blocks.ElementAt(12).transform.position.y - 1.71f), 0), Quaternion.identity));

        }
        for (int i = 0; i < 2; i++)
        {

            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-0.7f + 1.4f * i, (blocks.ElementAt(12).transform.position.y - 2.28f), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-3.5f-0.7f*i, (blocks.ElementAt(3).transform.position.y + 0.285f)-0.57f*i, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(3.5f+0.7f*i, (blocks.ElementAt(3).transform.position.y + 0.285f)-0.57f*i, 0), Quaternion.identity));

        }
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(0, (blocks.ElementAt(12).transform.position.y - 2.85f), 0), Quaternion.identity));



        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelSeven()
    {
        nullifyPowerUps();
        level =7;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();

        for(int i=0;i<3;i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(0,3.305f-0.85f*i,0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(0,0.755f-0.85f*i), Quaternion.identity));
        }
        for (int i = 0; i < 2; i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(1.05f+i, 4.125f + 0.85f * i, 0), Quaternion.identity));
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-1.05f - i, 4.125f + 0.85f * i, 0), Quaternion.identity));

            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(2.05f + i,5.825f + 0.85f * i, 0), Quaternion.identity));
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-2.05f - i, 5.825f + 0.85f * i, 0), Quaternion.identity));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(1.05f + i, emeraldBlocks.ElementAt(2).transform.position.y-0.85f-(0.85f*i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-1.05f - i, emeraldBlocks.ElementAt(2).transform.position.y - 0.85f - (0.85f * i), 0), Quaternion.identity));

            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(2.05f + i, emeraldBlocks.ElementAt(2).transform.position.y - 0.85f*3 - (0.85f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.05f - i, emeraldBlocks.ElementAt(2).transform.position.y - 0.85f * 3 - (0.85f * i), 0), Quaternion.identity));
        }
        for(int i=0;i<3;i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(2.05f + i, sapphireBlocks.ElementAt(2).transform.position.y + (0.85f * i), 0), Quaternion.identity));
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-2.05f - i, sapphireBlocks.ElementAt(2).transform.position.y + (0.85f * i), 0), Quaternion.identity));

            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(2.05f + i, emeraldBlocks.ElementAt(0).transform.position.y - (0.85f * i), 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.05f - i, emeraldBlocks.ElementAt(0).transform.position.y - (0.85f * i), 0), Quaternion.identity));
        }
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelEight()
    {
        nullifyPowerUps();
        level =8;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();

        for(int i=0;i<5;i++)
        {
            for(int j=0;j<7;j++)
            { 
             emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-4.2f + 1.4f * j, 6.705f - 1.14f * i, 0), Quaternion.identity));   
            }
        }
        for (int i = 0; i < 9; i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(4.2f, (emeraldBlocks.ElementAt(34).transform.position.y-0.57f)-0.57f*i, 0), Quaternion.identity));
        }
        for (int i = 0; i < 8; i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(1.4f, (emeraldBlocks.ElementAt(34).transform.position.y - 1.14f) - 0.57f * i, 0), Quaternion.identity));
        }
        for (int j = 0; j < 4; j++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3( - 1.4f * j, (emeraldBlocks.ElementAt(34).transform.position.y - 1.14f) , 0), Quaternion.identity));
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                
                
                    blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f - 1.4f * j, (emeraldBlocks.ElementAt(34).transform.position.y - 2.28f) - 1.14f * i, 0), Quaternion.identity));
                
            }
        }
        
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(4.2f, emeraldBlocks.ElementAt(6).transform.position.y-0.57f, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-4.2f, emeraldBlocks.ElementAt(7).transform.position.y - 0.57f, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(4.2f, emeraldBlocks.ElementAt(20).transform.position.y - 0.57f, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-4.2f, emeraldBlocks.ElementAt(21).transform.position.y - 0.57f, 0), Quaternion.identity));
      
        sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(2.8f, sapphireBlocks.ElementAt(8).transform.position.y, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(-4.2f, blocks.ElementAt(0).transform.position.y+0.57f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f, blocks.ElementAt(0).transform.position.y-0.57f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(-4.2f, blocks.ElementAt(8).transform.position.y + 0.57f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f, blocks.ElementAt(8).transform.position.y - 0.57f, 0), Quaternion.identity));

        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelNine()
    {
        nullifyPowerUps();
        level =9;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        for(int i=0;i<3;i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f+1.4f*i,5.565f,0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.41f + 1.4f * i, 4.425f, 0), Quaternion.identity));

           
           
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f + 1.4f * i, -1.555f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f + 1.4f * i, -2.695f, 0), Quaternion.identity));
        }
        for (int i = 0; i < 2; i++)
        {
            
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f + 2.8f * i, 1.435f, 0), Quaternion.identity));

            
        }
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(0,1.435f,0), Quaternion.identity));
        for (int i = 0; i < 4; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + 1.4f * i, 4.995f, 0), Quaternion.identity));

            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + 1.4f * i, -2.125f, 0), Quaternion.identity));
        }
        for (int i = 0; i < 7; i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-3.69f,3.145f-0.57f*i,0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(3.69f, 3.145f - 0.57f * i, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(0, 2.005f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(0, 0.865f, 0), Quaternion.identity));

        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.29f, 3.145f , 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.29f, 3.145f , 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.29f, -0.275f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.29f, -0.275f , 0), Quaternion.identity));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelTen()
    {
        nullifyPowerUps();
        level =10;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.1f + 1.4f * j, (6.705f - 0.57f * 8) - 0.57f * i, 0), Quaternion.identity));
            }
        }
        for (int j = 0; j < 18; j++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-3.5f, (6.705f - 0.57f * 18) + 0.57f * j, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(3.5f, (6.705f - 0.57f * 18) + 0.57f * j, 0), Quaternion.identity));

        }
        for (int j = 0; j < 4; j++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + 1.4f * j, 6.705f - 0.57f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + 1.4f * j, 6.705f - 1.14f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-2.1f + 1.4f * j, -3.555f, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(0, 6.705f, 0), Quaternion.identity));
        Destroy(emeraldBlocks.ElementAt(14));
        emeraldBlocks.Remove(emeraldBlocks.ElementAt(14));
        blocks.Add(Instantiate(primaryBlock, new Vector3(emeraldBlocks.ElementAt(13).transform.position.x+1.4f,emeraldBlocks.ElementAt(13).transform.position.y,0), Quaternion.identity));
        Destroy(emeraldBlocks.ElementAt(16));
        emeraldBlocks.Remove(emeraldBlocks.ElementAt(16));
        blocks.Add(Instantiate(primaryBlock, new Vector3(emeraldBlocks.ElementAt(15).transform.position.x + 1.4f, emeraldBlocks.ElementAt(15).transform.position.y,0), Quaternion.identity));
        Destroy(emeraldBlocks.ElementAt(20));
        emeraldBlocks.Remove(emeraldBlocks.ElementAt(20));
        blocks.Add(Instantiate(primaryBlock, new Vector3(emeraldBlocks.ElementAt(19).transform.position.x + 1.4f, emeraldBlocks.ElementAt(19).transform.position.y,0), Quaternion.identity));
        Destroy(emeraldBlocks.ElementAt(22));
        emeraldBlocks.Remove(emeraldBlocks.ElementAt(22));
        blocks.Add(Instantiate(primaryBlock, new Vector3(emeraldBlocks.ElementAt(21).transform.position.x + 1.4f, emeraldBlocks.ElementAt(21).transform.position.y,0), Quaternion.identity));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelEleven()
    {
        nullifyPowerUps();
        level =11;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();

        for(int i=0;i<7;i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-4.49f,4.22f-0.57f*i,0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3( 4.49f, 4.22f - 0.57f*i, 0), Quaternion.identity));
        }
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-3.79f, 4.79f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3( 3.79f, 4.79f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-3.79f, 0.23f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(3.79f, 0.23f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.1f, rubyBlocks.ElementAt(7).transform.position.y, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.1f, rubyBlocks.ElementAt(7).transform.position.y, 0), Quaternion.identity));

        for (int i = 0; i < 4; i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.1f+1.4f*i, rubyBlocks.ElementAt(9).transform.position.y, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.1f + 1.4f * i, rubyBlocks.ElementAt(5).transform.position.y, 0), Quaternion.identity));
        }
        for (int i = 0; i < 5; i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(0 , rubyBlocks.ElementAt(3).transform.position.y+0.57f*i, 0), Quaternion.identity));
           
            
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(0, rubyBlocks.ElementAt(11).transform.position.y-0.57f*i, 0), Quaternion.identity));



        }
        for (int i = 0; i < 3; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f+1.4f*i,6.5f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f + 1.4f * i, rubyBlocks.ElementAt(13).transform.position.y-2.28f, 0), Quaternion.identity));
        }

        populateIdLists();
        addCoins();
        addPowerUps();
        startingPoints = basePoints;
    }
    void levelTwelve()
    {
        nullifyPowerUps();
        level =12;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        //Hat
        for(int i=0;i<6; i++)
        {
            for(int j=0;j<3;j++)
            {
                diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-1.4f+1.4f*j,6.705f-0.57f*i,0), Quaternion.identity));
            }
        }
        //Brim of Hat
        for (int i = 0; i < 7; i++)
        {
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-4.2f+1.4f*i, diamondBlocks.ElementAt(17).transform.position.y-0.57f, 0), Quaternion.identity));   
        }
        //Cheeks/temples
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.8f , diamondBlocks.ElementAt(24).transform.position.y - 0.57f, 0), Quaternion.identity));
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3( 2.8f, diamondBlocks.ElementAt(24).transform.position.y - 0.57f, 0), Quaternion.identity));
        for (int i = 0; i < 7; i++)
        {
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(-4.2f, (emeraldBlocks.ElementAt(1).transform.position.y - 1.14f) - 0.57f * i, 0), Quaternion.identity));
            sapphireBlocks.Add(Instantiate(primarySapphire, new Vector3(4.2f, (emeraldBlocks.ElementAt(1).transform.position.y - 1.14f) - 0.57f * i, 0), Quaternion.identity));
        }
        
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-3.5f, diamondBlocks.ElementAt(24).transform.position.y - 1.14f,0), Quaternion.identity));
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3( 3.5f, diamondBlocks.ElementAt(24).transform.position.y - 1.14f, 0), Quaternion.identity));
        //
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-3.5f, sapphireBlocks.ElementAt(13).transform.position.y - 0.57f, 0), Quaternion.identity));
        emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(3.5f, sapphireBlocks.ElementAt(13).transform.position.y - 0.57f, 0), Quaternion.identity));
        //Jaw/chin
        for (int i = 0; i < 2; i++)
        {
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-2.8f+1.4f*i, (emeraldBlocks.ElementAt(5).transform.position.y - 0.57f) - 0.57f * i, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(2.8f-1.4f*i, (emeraldBlocks.ElementAt(5).transform.position.y - 0.57f) - 0.57f * i, 0), Quaternion.identity));
            emeraldBlocks.Add(Instantiate(primaryEmerald, new Vector3(-0.7f+1.4f*i, (emeraldBlocks.ElementAt(5).transform.position.y - 1.71f) , 0), Quaternion.identity));
        }
        //Eyes
       rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-1.735f,1.293f,0), Quaternion.identity));
       rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(1.735f, 1.293f, 0), Quaternion.identity));
        //Base of Mouth
         rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(0, -2.412f, 0), Quaternion.identity));
        // Ends of Mouth
        for(int i=0;i<1;i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3( -1.4f-1.4f*i, -1.842f +0.57f*i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(1.4f+ 1.4f*i, -1.842f+0.57f*i, 0), Quaternion.identity));
        }


        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void levelThirteen()
    {
        nullifyPowerUps();
        level =13;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        //Body/Abdomen of the Spider
        for (int i=0;i<9;i++)
        {
            for(int j=0;j<3;j++)
            {
                diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-1.4f+1.4f*j,3-0.57f*i,0 ), Quaternion.identity));
            }
        }
        
        //Frontal and Rear appendages of the spider
        for (int i = 0; i < 5; i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-3.5f, diamondBlocks.ElementAt(21).transform.position.y - 1.425f - 0.57f * i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(3.5f, diamondBlocks.ElementAt(21).transform.position.y - 1.425f - 0.57f * i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-3.5f, diamondBlocks.ElementAt(0).transform.position.y + 0.855f + 0.57f * i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(3.5f, diamondBlocks.ElementAt(0).transform.position.y + 0.855f + 0.57f * i, 0), Quaternion.identity));
        }   
        // Head
        for (int i = 0; i < 2; i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-0.7f, diamondBlocks.ElementAt(21).transform.position.y - 1.14f - 0.57f * i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(0.7f, diamondBlocks.ElementAt(21).transform.position.y - 1.14f - 0.57f * i, 0), Quaternion.identity));
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-1.4f, diamondBlocks.ElementAt(24).transform.position.y - 1.71f - 0.57f * i, 0), Quaternion.identity));
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(1.4f, diamondBlocks.ElementAt(24).transform.position.y - 1.71f - 0.57f * i, 0), Quaternion.identity));
        }
       
        //Central appendages
        for (int i = 0; i < 3;i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f-0.7f*i, diamondBlocks.ElementAt(17).transform.position.y - 0.57f * i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.8f+0.7f*i, diamondBlocks.ElementAt(17).transform.position.y - 0.57f*i , 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f - 0.7f * i, diamondBlocks.ElementAt(11).transform.position.y + 0.57f * i, 0), Quaternion.identity));
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.8f + 0.7f * i, diamondBlocks.ElementAt(11).transform.position.y + 0.57f * i, 0), Quaternion.identity));
        }
        //Hooked part of rear appendages
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f, ((diamondBlocks.ElementAt(0).transform.position.y + 0.57f * 5)+0.855f), 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.8f, ((diamondBlocks.ElementAt(0).transform.position.y + 0.57f * 5)+0.855f), 0), Quaternion.identity));
        //Beginning of rear appendages
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f, diamondBlocks.ElementAt(1).transform.position.y+0.285f , 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.8f, diamondBlocks.ElementAt(1).transform.position.y+0.285f , 0), Quaternion.identity));
        //Beginning of frontal appendages
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f, diamondBlocks.ElementAt(26).transform.position.y-0.285f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.8f, diamondBlocks.ElementAt(26).transform.position.y-0.285f, 0), Quaternion.identity));
        //Hooked part of frontal appendages
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f, ((diamondBlocks.ElementAt(24).transform.position.y-0.57f*6)-0.285f), 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(2.8f, ((diamondBlocks.ElementAt(24).transform.position.y - 0.57f * 6)-0.285f), 0), Quaternion.identity));
        Destroy(diamondBlocks.ElementAt(12));
        diamondBlocks.Remove(diamondBlocks.ElementAt(12));
        Destroy(diamondBlocks.ElementAt(13));
        diamondBlocks.Remove(diamondBlocks.ElementAt(13));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(0, diamondBlocks.ElementAt(12).transform.position.y, 0), Quaternion.identity));
        Destroy(diamondBlocks.ElementAt(12));
        diamondBlocks.Remove(diamondBlocks.ElementAt(12));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);


    }
    void levelFourteen()
    {
        nullifyPowerUps();
        level =14;
        startingPoints = basePoints;
        ballReleased = false;
        resetBallPos();
        paddle.transform.SetPositionAndRotation(originalPaddlePos, Quaternion.identity);
        releaseBall();
        //Upper 2
        for(int i=0;i<2;i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3((-2.8f) + 1.4f * i, 6.705f-0.59f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3((-2.8f) + 1.4f * i, 6.705f-1.18f-0.59f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3((-2.8f) + 1.4f * i, 6.705f - 2.36f-0.59f, 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(-1.4f, 6.705f - 1.18f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(-2.8f, 6.705f - 2.36f, 0), Quaternion.identity));
        
        
        for(int i=0;i<7;i++)
        {
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-4.2f+1.4f*i ,6.705f, 0), Quaternion.identity));
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-4.2f + 1.4f * i, -5.1f, 0), Quaternion.identity));
           
        }
        for(int i=0;i<19;i++)
        {
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(-4.2f, 6.705f - 0.6f - 0.59f * i, 0), Quaternion.identity));
            diamondBlocks.Add(Instantiate(primaryDiamond, new Vector3(4.2f, 6.705f - 0.6f - 0.59f * i, 0), Quaternion.identity));
        }
        //Lower 2
        for (int i = 0; i < 2; i++)
        {
            blocks.Add(Instantiate(primaryBlock, new Vector3((2.8f) - 1.4f * i, diamondBlocks.ElementAt(43).transform.position.y+0.02f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3((2.8f) - 1.4f * i, diamondBlocks.ElementAt(47).transform.position.y+0.02f, 0), Quaternion.identity));
            blocks.Add(Instantiate(primaryBlock, new Vector3((2.8f) - 1.4f * i, diamondBlocks.ElementAt(51).transform.position.y+0.02f , 0), Quaternion.identity));
        }
        blocks.Add(Instantiate(primaryBlock, new Vector3(2.8f, diamondBlocks.ElementAt(43).transform.position.y - 0.59f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(1.4f, diamondBlocks.ElementAt(51).transform.position.y + 0.59f, 0), Quaternion.identity));
       
        //Fill negative space with ruby blocks
        for(int i=0;i<14;i++)
        {
            for (int j = 0; j < 3; j++)
            {
                rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(0 + 1.4f * j,6.115f-0.59f*i,0),Quaternion.identity));
            }
        }
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(-2.8f + 1.4f * j, 3.165f - 0.59f * i, 0), Quaternion.identity));
            }
        }
        for(int i=0;i<5;i++)
        {
            rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(blocks.ElementAt(11).transform.position.x-1.4f, rubyBlocks.ElementAt(41).transform.position.y-0.59f - (0.59f * i), 0), Quaternion.identity));
        }
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(blocks.ElementAt(9).transform.position.x, blocks.ElementAt(9).transform.position.y - 0.59f , 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(blocks.ElementAt(11).transform.position.x, blocks.ElementAt(11).transform.position.y - 0.59f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(blocks.ElementAt(0).transform.position.x, blocks.ElementAt(0).transform.position.y - 0.59f, 0), Quaternion.identity));
        rubyBlocks.Add(Instantiate(primaryRuby, new Vector3(blocks.ElementAt(4).transform.position.x, blocks.ElementAt(4).transform.position.y - 0.59f, 0), Quaternion.identity));
        blocks.Add(Instantiate(primaryBlock, new Vector3(rubyBlocks.ElementAt(18).transform.position.x, rubyBlocks.ElementAt(18).transform.position.y, 0), Quaternion.identity));
        Destroy(rubyBlocks.ElementAt(18));
        rubyBlocks.Remove(rubyBlocks.ElementAt(18));
        blocks.Add(Instantiate(primaryBlock, new Vector3(rubyBlocks.ElementAt(35).transform.position.x, rubyBlocks.ElementAt(35).transform.position.y, 0), Quaternion.identity));
        Destroy(rubyBlocks.ElementAt(35));
        rubyBlocks.Remove(rubyBlocks.ElementAt(35));
        populateIdLists();
        addCoins();
        addPowerUps();
        deltaPoints = (10 * blocks.Count) + (15 * sapphireBlocks.Count) + (20 * emeraldBlocks.Count) + (25 * rubyBlocks.Count) + (30 * diamondBlocks.Count);

    }
    void populateIdLists()
    {
        //Populate id list for regular blocks
        for (int i = 0; i < blocks.Count; i++)
        {
            idList.Add(blocks.ElementAt(i).GetComponent<BoxCollider2D>().GetInstanceID());
        }
        //Populate sapphireIdList
        for (int i = 0; i < sapphireBlocks.Count; i++)
        {
            sapphireIdList.Add(sapphireBlocks.ElementAt(i).GetComponent<BoxCollider2D>().GetInstanceID());
        }
        //Populate sapphireObjects
        for (int i = 0; i < sapphireBlocks.Count; i++)
        {
            sapphireObjects.Add(new Sapphire(0));
        }
        //Populate sapphireShakers list
        for (int i = 0; i < sapphireBlocks.Count; i++)
        {
            sapphireShakers.Add(sapphireBlocks.ElementAt(i).GetComponent<ObjectShaker>());
        }
        //Populate emeraldIdList
        for (int i = 0; i < emeraldBlocks.Count; i++)
        {
            emeraldIdList.Add(emeraldBlocks.ElementAt(i).GetComponent<BoxCollider2D>().GetInstanceID());
        }
        //Populate emeraldObjects
        for (int i = 0; i < emeraldBlocks.Count; i++)
        {
            emeraldObjects.Add(new Emerald(0));
        }
        //Populate emeraldShakers list
        for (int i = 0; i < emeraldBlocks.Count; i++)
        {
            emeraldShakers.Add(emeraldBlocks.ElementAt(i).GetComponent<ObjectShaker>());
        }
        //Populate rubyIdList
        for (int i = 0; i < rubyBlocks.Count; i++)
        {
            rubyIdList.Add(rubyBlocks.ElementAt(i).GetComponent<BoxCollider2D>().GetInstanceID());
        }
        //Populate rubyObjects
        for (int i = 0; i < rubyBlocks.Count; i++)
        {
            rubyObjects.Add(new Ruby(0));
        }
        //Populate rubyShakers list
        for (int i = 0; i < rubyBlocks.Count; i++)
        {
            rubyShakers.Add(rubyBlocks.ElementAt(i).GetComponent<ObjectShaker>());
        }
        // Populate diamondIdList
        for (int i = 0; i < diamondBlocks.Count; i++)
        {
            diamondIdList.Add(diamondBlocks.ElementAt(i).GetComponent<BoxCollider2D>().GetInstanceID());
        }
        //Populate diamondObjects
        for (int i = 0; i < diamondBlocks.Count; i++)
        {
            diamondObjects.Add(new Diamond(0));
        }
        //Populate diamondShakers list
        for (int i = 0; i < diamondBlocks.Count; i++)
        {
            diamondShakers.Add(diamondBlocks.ElementAt(i).GetComponent<ObjectShaker>());
        }

    }
    public bool releaseBall()
    {
        if (pressedPlay == true)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                ballReleased = true;
            }
          
        }
        else
        {
            ballReleased = false;
        }
        return ballReleased;
    }
    void addCoins()
    {
        
        List<int> coinBearers = new List<int>();
        for(int i=0;i<blocks.Count;i++)
        {
            allBlocks.Add(blocks.ElementAt(i));
            allObjects.Add(new All("regular", 10));
            augmentedIdList.Add(idList.ElementAt(i));
        }
        for (int i = 0; i < sapphireBlocks.Count; i++)
        {
            allBlocks.Add(sapphireBlocks.ElementAt(i));
            allObjects.Add(new All("sapphire",15));
            augmentedIdList.Add(sapphireIdList.ElementAt(i));

        }
        for (int i = 0; i < emeraldBlocks.Count; i++)
        {
            allBlocks.Add(emeraldBlocks.ElementAt(i));
            allObjects.Add(new All("emerald",20));
            augmentedIdList.Add(emeraldIdList.ElementAt(i));

        }
        for (int i = 0; i < rubyBlocks.Count; i++)
        {
            allBlocks.Add(rubyBlocks.ElementAt(i));
            allObjects.Add(new All("ruby",25));
            augmentedIdList.Add(rubyIdList.ElementAt(i));

        }
        for (int i = 0; i < diamondBlocks.Count; i++)
        {
            allBlocks.Add(diamondBlocks.ElementAt(i));
            allObjects.Add(new All("diamond",30));
            augmentedIdList.Add(diamondIdList.ElementAt(i));

        }
        
        int coinNum = Random.Range(0, allBlocks.Count );
        for (int i = 0; i < coinNum;i++)
        {
            coinBearers.Add(Random.Range(0, allBlocks.Count - 1));
        }
        for (int i = 0; i < coinBearers.Count; i++)
        {
            
                allObjects.ElementAt(coinBearers.ElementAt(i)).setCoinBearer(true);
            
        }
    }
    void addPowerUps()
    {
        int numOfPowerUps = Random.Range(0,4);
        
        for(int i=0;i<numOfPowerUps;i++)
        {
            allObjects.ElementAt(Random.Range(0,allBlocks.Count)).setPowerBearer(true);
        }
    }
    public void resetBallPos()
    {
        ballClones.ElementAt(0).transform.SetPositionAndRotation(originalBallPos, Quaternion.identity);
    }
    public void nullifyPowerUps()
    {
        //Destroy Ball Clones
        {
            if(ballClones.Count>1)
            {
                for(int i=ballClones.Count-1;i>0;i--)
                {
                    Destroy(ballClones.ElementAt(i));
                    ballClones.Remove(ballClones.ElementAt(i));
                    ballCloneIdList.Remove(ballCloneIdList.ElementAt(i));

                    activeBalls.Remove(activeBalls.ElementAt(i));
                }
            }
        }
       
        //Revert to original Paddle Sprite
        if (activePaddle == "default")
        {
            
                paddle.transform.localScale = originalPaddleScale;
                paddle.GetComponent<SpriteRenderer>().sprite = ogPadSprite;
                paddle.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            
        }
        else if(activePaddle=="breadmaker")
        {
            paddle.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            paddle.transform.position = new Vector3(paddle.transform.position.x, -6.851f, paddle.transform.position.z);
            paddle.GetComponent<BoxCollider2D>().size = new Vector2(2, 0.25f);
        }
        else if(activePaddle=="Teleporter")
        {
            paddle.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Teleporter").GetComponent<SpriteRenderer>().sprite;
            paddle.transform.localScale = new Vector3(2.5f, 2.5f, 1);
            paddle.transform.position = new Vector3(paddle.transform.position.x, -6.851f, paddle.transform.position.z);
            paddle.GetComponent<BoxCollider2D>().size = new Vector2(1.15f, 0.15f);
        }
        //Make crosshair invisible
        {
            och.GetComponent<SpriteRenderer>().enabled = false;
            mch.GetComponent<SpriteRenderer>().enabled = false;
            ich.GetComponent<SpriteRenderer>().enabled = false;
        }
        //Deactivate Missile
        missileActivated = false;
        missileLaunched = false;
        
        //Make Health Bar Visible Again
         healthBarOutline.GetComponent<SpriteRenderer>().enabled = true;
         healthBar.GetComponent<SpriteRenderer>().enabled = true;
        //Deactivate Lasers
        if(lasersActivated==true)
        {
            lasersActivated = false;
        }
        if(KEActivated==true)
        {
            KEActivated = false;
            ballClones.ElementAt(0).GetComponent<SpriteRenderer>().color = Color.white;
            resVel = 12;
            activeBalls.ElementAt(0).setBallSpeedY(resVel * Mathf.Sin(theta));
            activeBalls.ElementAt(0).setBallSpeedX(resVel * Mathf.Cos(theta));
            colPad.GetComponent<BoxCollider2D>().enabled = false;


        }
    }
    public static void applyPurchases()
    {
        //Refill
        if (purchase_Confirmed == true&&rfClicked==true)
        {
            
            health = 30;
            healthBar.transform.localScale = new Vector3(originalHealthBarScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            healthBar.transform.localPosition = originalHealthBarPos;
            purchase_Confirmed = false;
        }
        //Double-shield
        else if(purchase_Confirmed==true&&dsClicked==true)
        {
            coins -= cost;
            if (health>=dmgTol/2)
            {
                health= dmgTol;
            }
            else
            {
                health += dmgTol / 2;
            }
            

            healthBar.transform.localScale = new Vector3(((health/(dmgTol*0.1408f))),healthBar.transform.localScale.y,healthBar.transform.localScale.z);
            healthBar.transform.localPosition = new Vector3(healthBarOutline.transform.localPosition.x-(0.5f*((healthBarOutline.transform.localScale.x*53.26f)-healthBar.transform.localScale.x)), originalHealthBarPos.y,originalHealthBarPos.z);
            shield = (int)(health * 0.75f);
            shieldActivated = true;
            shieldStatus.transform.localScale = new Vector3(originalShieldScale, 0.2137156f, 1);
            shieldStatus.transform.localPosition = new Vector3(0.01f, 0.01000023f,-2);
            enableShield();
            purchase_Confirmed = false;
        }
        //Invincibility
        else if(purchase_Confirmed==true&&invClicked==true)
        {
            coins -= cost;
            activateInvincibility();
            purchase_Confirmed = false;
        }
        //Upgrade1
        else if (purchase_Confirmed == true && u1Clicked == true)
        {
            coins -= cost;
            activePaddle = "breadmaker";
            paddle.GetComponent<SpriteRenderer>().sprite=GameObject.Find("Breadmaker").GetComponent<SpriteRenderer>().sprite;
            doubleCurrency=true;
            paddle.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            paddle.transform.position = new Vector3(paddle.transform.position.x, -6.851f, paddle.transform.position.z);
            paddle.GetComponent<BoxCollider2D>().size = new Vector2(2, 0.25f);
            purchase_Confirmed = false;
        }
        //Upgrade2
        else if(purchase_Confirmed==true&&u2Clicked==true)
        {
            activePaddle = "Teleporter";
            teleportation = true;
            coins -= cost;
            paddle.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Teleporter").GetComponent<SpriteRenderer>().sprite;
            paddle.transform.localScale = new Vector3(2.5f, 2.5f, 1);
            paddle.transform.position = new Vector3(paddle.transform.position.x, -6.851f, paddle.transform.position.z);
            paddle.GetComponent<BoxCollider2D>().size = new Vector2(1.15f, 0.15f);
            purchase_Confirmed = false;
        }
    }
   
    public static void enableShield()
    {
        shieldActivated = true;
        for(int i=0;i<shieldBar.GetComponentsInChildren<SpriteRenderer>().Length;i++)
        {
            shieldBar.GetComponentsInChildren<SpriteRenderer>()[i].enabled = true;
        }
    }
    public static void disableShield()
    {
        shieldActivated = false;
        for (int i = 0; i < shieldBar.GetComponentsInChildren<SpriteRenderer>().Length; i++)
        {
            shieldBar.GetComponentsInChildren<SpriteRenderer>()[i].enabled = false;
        }
    }
    public static void activateInvincibility()
    {
        invincibility = true;
        //healthBarOutline.GetComponent<SpriteRenderer>().color = Color.black;
        healthBar.GetComponent<SpriteRenderer>().color = Color.white;
        invPs.SetActive(true);
    }
    public void disableInvincibility()
    {
        invincibility = false;
        healthBarOutline.GetComponent<SpriteRenderer>().color = Color.white;
        healthBar.GetComponent<SpriteRenderer>().color = new Vector4(0.1566009f, 0.9529412f, 0.007843133f,1) ;
        invPs.SetActive(false);
        hitCount = 0;
    }
    void clearAll()
    {
        blocks.Clear();
        idList.Clear();
        sapphireBlocks.Clear();
        sapphireIdList.Clear();
        sapphireObjects.Clear();
        sapphireShakers.Clear();
        emeraldBlocks.Clear();
        emeraldIdList.Clear();
        emeraldObjects.Clear();
        emeraldShakers.Clear();
        rubyBlocks.Clear();
        rubyIdList.Clear();
        rubyObjects.Clear();
        rubyShakers.Clear();
        diamondBlocks.Clear();
        diamondIdList.Clear();
        diamondObjects.Clear();
        diamondShakers.Clear();
        allBlocks.Clear();
        allObjects.Clear();
        augmentedIdList.Clear();
    }
}
