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
    public GameObject PlayerStayText;
    public GameObject OpponentStayText;
    public GameObject[] PlayerHealth;
    public GameObject[] OpponentHealth;
    public GameObject ButtonAction;
    public GameObject WinUI;
    public Text WhoWinText;

    //Message
    public GameObject Message;
    public Text Message_Text;

    //For card place
    public GameObject[] Card;
    public GameObject PlayerArea;
    public GameObject OpponentArea;
    GameObject Player_PlaceCard;
    GameObject Opponent_PlaceCard;

    //situation booling
    bool PlayerSetTwo = false,
        OpponentSettTwo = false,
        Hit = false,
        Winning = false,
        didPlayerWin = false,
        didPlayerLose = false;

    //Player
    int PlayerHP = 3,
        hold = 0;
    int[] PlayerHode;
    //Opponent
    int OpponentHP = 3;
    int[] OpponentHode;

    int PlayerNum, OpponentNum;

    UserProfile userprofile;
    GlobalControl global;

    void Start()
    {
        //set text to 0
        didPlayerWin = false;
        didPlayerLose = false;
        global = (GlobalControl)FindObjectOfType(typeof(GlobalControl));
        //AI = (AI_Player)FindObjectOfType(typeof(AI_Player));
    }

    private void Update()
    {
        if (Hit)
        {
            OppontmentPlay();
            Message.gameObject.SetActive(false);
        }

        Hit = false;
        
        HealthLose(PlayerHP, OpponentHP);

        if (Winning == true && ButtonAction.activeSelf)
            ReviveHealth();
    }

    public void ButFinish()
    {
        Hit = true;
        PlayerStay = true;
        PlayerStayText.gameObject.SetActive(true);
        if (PlayerStay == true && OpponentStay == true)
            Result();

        
    }
    public void ButClear()
    {
        //Reset();
    }

    //do random number
    public int RandomNumber()
    {
        int pick = Random.Range(1, 3);
        return pick;
    }

    public void ButSword()
    {
        Hit = true;

        if(hold!=2)
        {
            PlayerNum = 1;
            PlayerShow(PlayerNum);
            PlayerHode[hold] = 1;
        }

        hold++;
    }

    //Card and place them
    private void PlayerShow(int num)
    {
        Player_PlaceCard = Instantiate(Card[num-1], new Vector2(0, 0), Quaternion.identity);
        Player_PlaceCard.transform.SetParent(PlayerArea.transform, false);

        //Debug.Log(max);
    }

    //clear card on table
    private void clearCard()
    {
        var max = GameObject.FindGameObjectsWithTag("Card");

        foreach(var a in max)
        {
            Destroy(a);
        }

        PlayerSetTwo = false;
        OpponentSettTwo = false;

        Hit = false;
    }

    private void OpponentShow(int num)
    {
        Opponent_PlaceCard = Instantiate(Card[num - 1], new Vector2(0, 0), Quaternion.identity);
        Opponent_PlaceCard.transform.SetParent(OpponentArea.transform, false);
    }

    private void HealthLose(int P_HP, int O_HP)
    {
        //check PlayerHealth losing
        if (P_HP == 2)
            PlayerHealth[2].gameObject.SetActive(false);
        else if (P_HP == 1)
            PlayerHealth[1].gameObject.SetActive(false);
        else if (P_HP == 0)
            PlayerHealth[0].gameObject.SetActive(false);

        //check PlayerHealth losing
        if (O_HP == 2)
            OpponentHealth[2].gameObject.SetActive(false);
        else if (O_HP == 1)
            OpponentHealth[1].gameObject.SetActive(false);
        else if (O_HP == 0)
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

    private void OppontmentPlay()
    {
        for (int i = 0; i < 3; i++)
        {
            OpponentNum = RandomNumber();
            OpponentShow(OpponentNum);
        }
    }

    private void Result()
    {
        PlayerStayText.gameObject.SetActive(false);
        OpponentStayText.gameObject.SetActive(false);

        //check result
        for(int i=0;i<3;i++)
        {
            if(PlayerHode[i] == 1 && OpponentHode[i] == 2||
                PlayerHode[i] == 2 && OpponentHode[i] == 3||
                PlayerHode[i] == 3 && OpponentHode[i] == 1)
                PlayerHP--;
            else if (OpponentHode[i] == 1 && PlayerHode[i] == 2 ||
                OpponentHode[i] == 2 && PlayerHode[i] == 3 ||
                OpponentHode[i] == 3 && PlayerHode[i] == 1)
                OpponentHP--;
        }

        //anyone lose health
        if(OpponentHP == 0 || PlayerHP == 0)
        {
            Winning = true;
            WinUI.gameObject.SetActive(true);
            ButtonAction.gameObject.SetActive(false);

            if (PlayerHP == 0 && didPlayerLose == false)
            {
                WhoWinText.text = "Opponent Win!";
                didPlayerLose = true;
                global.lost += 1;
            }
            else if (OpponentHP == 0 && didPlayerWin == false)
            {
                didPlayerWin = true;
                WhoWinText.text = "Player Win!";
                global.wins += 1;
                global.xp += 4;
            }
        }
        clearCard();

    }
}
