using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random; //do random number pick

public class CardPick : MonoBehaviour
{
    //For UI
    public GameObject[] PlayerHealth;
    public GameObject[] OpponentHealth;
    public GameObject ButtonAction;
    public GameObject WinUI;
    public Text WhoWinText;

    //Message
    public GameObject PlayerFinish,
        OppontmentFinish;
    public GameObject DecisionCheck;

    //For card place
    public GameObject[] Card;
    public GameObject PlayerArea;
    public GameObject OpponentArea;
    GameObject Player_PlaceCard;
    GameObject Opponent_PlaceCard;

    //situation booling
    bool Finish = false,
        OtherFinish = false,
        Winning = false,
        didPlayerWin = false,
        didPlayerLose = false;

    //Player
    int PlayerHP = 3,
        hold = 0;
    int[] PlayerHode = new int[3];
    //Opponent
    int OpponentHP = 3,
        O_hold = 0;
    int[] OpponentHode = new int[3];

    UserProfile userprofile;
    GlobalControl global;

    void Start()
    {
        //set text to 0
        global = (GlobalControl)FindObjectOfType(typeof(GlobalControl));
        //AI = (AI_Player)FindObjectOfType(typeof(AI_Player));
    }

    private void Update()
    {
        if (hold == 3)
            DecisionCheck.gameObject.SetActive(true);
        else
            DecisionCheck.gameObject.SetActive(false);

        if(OtherFinish)
            OppontmentFinish.gameObject.SetActive(true);

        if (Finish && OtherFinish)
        {
            OppontmentPlay();
            //Message.gameObject.SetActive(false);
            Result();
        }

        Finish = false;
        OtherFinish = false;

        HealthLose(PlayerHP, OpponentHP);

        //Reset all Health if player play again
        if (Winning == true && ButtonAction.activeSelf)
            ReviveHealth();
    }

    //button Finish
    public void ButFinish()
    {
        if(!Finish)
        {
            Finish = true;
            PlayerFinish.gameObject.SetActive(true);
        }
        OtherFinish = true;

    }

    //button clear
    public void ButClear()
    {
        if(!Finish)
        {
            clearCard();
            for (int i = 0; i < PlayerHode.Length; i++)
            {
                PlayerHode[i] = 0;
            }
        }
    }
    /// //////////////////////////////////////////////////////
    //battle button
    public void ButSword()
    {
        if(hold!=3 && !Finish)
        {
            PlayerHode[hold] = 1;
            hold++;
            PlayerShow(1);
        }
    }
    public void ButMagic()
    {
        if (hold != 3 && !Finish)
        {
            PlayerHode[hold] = 3;
            hold++;
            PlayerShow(3);
        }

    }
    public void ButShield()
    {
        if (hold != 3 && !Finish)
        {
            PlayerHode[hold] = 2;
            hold++;
            PlayerShow(2);
        }

        
    }
    //-------------------------------------------------------------------------------------
    //Clear the card
    private void clearCard()
    {
        //clear table card
        var max = GameObject.FindGameObjectsWithTag("Card");
        foreach(var a in max)
        {
            Destroy(a);
        }

        hold = 0;
        Finish = false;
        OtherFinish = false;
        OppontmentFinish.gameObject.SetActive(false);
        PlayerFinish.gameObject.SetActive(false);
    }

//-----------------------------------------------------------------------------------
    //Show card on the table
    private void PlayerShow(int num)
    {
        Player_PlaceCard = Instantiate(Card[num-1], new Vector2(0, 0), Quaternion.identity);
        Player_PlaceCard.transform.SetParent(PlayerArea.transform, false);

        //Debug.Log(max);
    }
    private void OpponentShow(int num)
    {
        Opponent_PlaceCard = Instantiate(Card[num - 1], new Vector2(0, 0), Quaternion.identity);
        Opponent_PlaceCard.transform.SetParent(OpponentArea.transform, false);
    }

    private void HealthLose(int P_HP, int O_HP)
    {
        //check PlayerHealth losing
        if (P_HP != 3)
            PlayerHealth[2].gameObject.SetActive(false);
        if (P_HP <= 1)
            PlayerHealth[1].gameObject.SetActive(false);
        if (P_HP <= 0)
            PlayerHealth[0].gameObject.SetActive(false);

        //check PlayerHealth losing
        if (O_HP == 2)
            OpponentHealth[2].gameObject.SetActive(false);
        if (O_HP <= 1)
            OpponentHealth[1].gameObject.SetActive(false);
        if (O_HP <= 0)
            OpponentHealth[0].gameObject.SetActive(false);
    }

    private void ReviveHealth()
    {
        for(int i = 0; i < 3; i++)
        {
            PlayerHealth[i].gameObject.SetActive(true);
            OpponentHealth[i].gameObject.SetActive(true);
        }

        OpponentHP = 3;
        PlayerHP = 3;

        Winning = false;
    }

    //----------------------------------------------------------------
    //oppontment term
    public int RandomNumber()
    {
        int pick = Random.Range(1, 3);
        return pick;
    }
    private void OppontmentPlay()
    {
        for (int i = 0; i < 3; i++)
        {
            OpponentHode[i] = RandomNumber();
            //Debug.Log("other:" + OpponentHode[i]);
            //O_hold++;
            OpponentShow(OpponentHode[i]);
        }
    }
    //--------------------------------------------------------------------
    //result
    private void Result()
    {
        //check result
        for(int i=0;i<3;i++)
        {
            //Debug.Log("player:" + PlayerHode[i]);
            //Debug.Log("other:" + OpponentHode[i]);

            if (PlayerHode[i] == 1 && OpponentHode[i] == 2||
                PlayerHode[i] == 2 && OpponentHode[i] == 3||
                PlayerHode[i] == 3 && OpponentHode[i] == 1)
                PlayerHP--;
            else if (OpponentHode[i] == 1 && PlayerHode[i] == 2 ||
                OpponentHode[i] == 2 && PlayerHode[i] == 3 ||
                OpponentHode[i] == 3 && PlayerHode[i] == 1)
                OpponentHP--;
        }
        Debug.Log("player:" + PlayerHP);
        Debug.Log("other:" + OpponentHP);

        //anyone lose health
        if (OpponentHP <= 0 || PlayerHP <= 0)
        {
            Winning = true;
            WinUI.gameObject.SetActive(true);
            ButtonAction.gameObject.SetActive(false);

            if (PlayerHP < 0 && OpponentHP < 0)
            {
                WhoWinText.text = "Tie!";
            }
            else if (PlayerHP < 0 && didPlayerLose == false)
            {
                WhoWinText.text = "Opponent Win!";
                didPlayerLose = true;
                //global.lost += 1;
            }
            else if (OpponentHP < 0 && didPlayerWin == false)
            {
                didPlayerWin = true;
                WhoWinText.text = "Player Win!";
                //global.wins += 1;
                //global.xp += 4;
            }
        }
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        clearCard();
    }

}
