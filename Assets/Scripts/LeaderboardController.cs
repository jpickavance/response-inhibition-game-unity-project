using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Linq;
using TMPro;

public class LeaderboardController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ReadLeaderboardTop10(string tableName);
    [DllImport("__Internal")]
    private static extern void GetLeaderboardSize(string tableName);
    public string myToken;
    public int myIndex;
    public Text rank1;
    public Text rank2;
    public Text rank3;
    public Text rank4;
    public Text rank5;
    public Text rank6;
    public Text rank7;
    public Text rank8;
    public Text rank9;
    public Text rank10;
    public Text Token1;
    public Text Token2;
    public Text Token3;
    public Text Token4;
    public Text Token5;
    public Text Token6;
    public Text Token7;
    public Text Token8;
    public Text Token9;
    public Text Token10;
    public Text Score1;
    public Text Score2;
    public Text Score3;
    public Text Score4;
    public Text Score5;
    public Text Score6;
    public Text Score7;
    public Text Score8;
    public Text Score9;
    public Text Score10;
    public Text combo1;
    public Text combo2;
    public Text combo3;
    public Text combo4;
    public Text combo5;
    public Text combo6;
    public Text combo7;
    public Text combo8;
    public Text combo9;
    public Text combo10;
    public Text hitPerc1;
    public Text hitPerc2;
    public Text hitPerc3;
    public Text hitPerc4;
    public Text hitPerc5;
    public Text hitPerc6;
    public Text hitPerc7;
    public Text hitPerc8;
    public Text hitPerc9;
    public Text hitPerc10;
    public Text ssrt1;
    public Text ssrt2;
    public Text ssrt3;
    public Text ssrt4;
    public Text ssrt5;
    public Text ssrt6;
    public Text ssrt7;
    public Text ssrt8;
    public Text ssrt9;
    public Text ssrt10;
    public TextMeshProUGUI buttonText;
    public int n_rows;
    public int currentIndex;
    public bool top10;
    public GameObject ScrollButtons;
    public Color pink;
    public Color green;

    [System.Serializable]
    public class ProfileClass
    {
        public string tokenId;
        public string score;
        public string comboHigh;
        public string hitPerc;
        public string rSSRT;
    }
    [System.Serializable]
    public class ProfileClass2
    {
        public string tokenId;
        public int score;
        public string comboHigh;
        public string hitPerc;
        public string rSSRT;
    }
    
    public List<ProfileClass2> leaderboardData = new List<ProfileClass2>();
    public List<Text> rankCol = new List<Text>();
    public List<Text> tokenCol = new List<Text>();
    public List<Text> scoreCol = new List<Text>();
    public List<Text> comboCol = new List<Text>();
    public List<Text> hitPercCol = new List<Text>();
    public List<Text> ssrtCol = new List<Text>();


    void Awake()
    {
        currentIndex = 0;
        GetLeaderboardSize("JP_FBS_Pilot_Leaderboard");
        ReadLeaderboardTop10("JP_FBS_Pilot_Leaderboard");
        InitFields();
        myToken = UserInfo.Instance.tokenId;
        top10 = true;
    }
    
    void Start()
    {
        StartCoroutine(LoadLeaderboard());
    }

    public void setLeaderboardSize(int tableLength)
    {
        n_rows = tableLength;
    }

    ///This currently works by scanning the whole table of scores. Needs to be updated with a global secondary index (costs money per month)
    public void appendResult(string receivedData)
    {   
        ProfileClass profileItem = JsonUtility.FromJson<ProfileClass>(receivedData);
        int profileScoreInt = Convert.ToInt32(profileItem.score);
        leaderboardData.Add(new ProfileClass2(){tokenId = profileItem.tokenId.ToString(),
                                                score = profileScoreInt, 
                                                comboHigh = profileItem.comboHigh.ToString(), 
                                                hitPerc = profileItem.hitPerc.ToString(), 
                                                rSSRT = profileItem.rSSRT.ToString()});
    }

    public void ShowLeaderBoard()
    {
                for(int i = 0; i < 10; i++)
                {
                    rankCol[i].GetComponent<Text>().text = (currentIndex + i + 1).ToString();
                    tokenCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].tokenId;
                    scoreCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].score.ToString();
                    comboCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].comboHigh;
                    hitPercCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].hitPerc;
                    ssrtCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].rSSRT;
                    
                    if(leaderboardData[currentIndex + i].tokenId == myToken)
                    {
                        rankCol[i].GetComponent<Text>().color = pink;
                        tokenCol[i].GetComponent<Text>().color = pink;
                        scoreCol[i].GetComponent<Text>().color = pink;
                        comboCol[i].GetComponent<Text>().color = pink;
                        hitPercCol[i].GetComponent<Text>().color = pink;
                        ssrtCol[i].GetComponent<Text>().color = pink;
                    }
                    else
                    {
                        rankCol[i].GetComponent<Text>().color = green;
                        tokenCol[i].GetComponent<Text>().color = green;
                        scoreCol[i].GetComponent<Text>().color = green;
                        comboCol[i].GetComponent<Text>().color = green;
                        hitPercCol[i].GetComponent<Text>().color = green;
                        ssrtCol[i].GetComponent<Text>().color = green;
                    }
                }
    }

    public void errorCallback(string err)
    {
        Debug.LogError(err);
    }

    public void toggleTop10()
    {
        top10 = !top10;
        if(!top10)
        {
            if(myIndex > 3)
            {
                currentIndex = myIndex - 4;
            }
            else
            {
                currentIndex = 0;
            }
            ScrollButtons.SetActive(true);
            buttonText.text = "TOP 10";
        }
        else
        {
            currentIndex = 0;
            ScrollButtons.SetActive(false);
            buttonText.text = "MY SCORE";
        }
        ShowLeaderBoard();
    }

    
    public void ScrollUp()
    {
        if(currentIndex > 0)
        {
            currentIndex -= 1;
            ShowLeaderBoard();
        }
    }
    public void ScrollDown()
    {
        if(currentIndex < (n_rows - 10))
        {
            currentIndex += 1;
            ShowLeaderBoard();
        }
    }

    public void InitFields()
    {
        rankCol.Add(rank1);
        rankCol.Add(rank2);
        rankCol.Add(rank3);
        rankCol.Add(rank4);
        rankCol.Add(rank5);
        rankCol.Add(rank6);
        rankCol.Add(rank7);
        rankCol.Add(rank8);
        rankCol.Add(rank9);
        rankCol.Add(rank10);
        tokenCol.Add(Token1);
        tokenCol.Add(Token2);
        tokenCol.Add(Token3);
        tokenCol.Add(Token4);
        tokenCol.Add(Token5);
        tokenCol.Add(Token6);
        tokenCol.Add(Token7);
        tokenCol.Add(Token8);
        tokenCol.Add(Token9);
        tokenCol.Add(Token10);
        scoreCol.Add(Score1);
        scoreCol.Add(Score2);
        scoreCol.Add(Score3);
        scoreCol.Add(Score4);
        scoreCol.Add(Score5);
        scoreCol.Add(Score6);
        scoreCol.Add(Score7);
        scoreCol.Add(Score8);
        scoreCol.Add(Score9);
        scoreCol.Add(Score10);
        comboCol.Add(combo1);
        comboCol.Add(combo2);
        comboCol.Add(combo3);
        comboCol.Add(combo4);
        comboCol.Add(combo5);
        comboCol.Add(combo6);
        comboCol.Add(combo7);
        comboCol.Add(combo8);
        comboCol.Add(combo9);
        comboCol.Add(combo10);
        hitPercCol.Add(hitPerc1);
        hitPercCol.Add(hitPerc2);
        hitPercCol.Add(hitPerc3);
        hitPercCol.Add(hitPerc4);
        hitPercCol.Add(hitPerc5);
        hitPercCol.Add(hitPerc6);
        hitPercCol.Add(hitPerc7);
        hitPercCol.Add(hitPerc8);
        hitPercCol.Add(hitPerc9);
        hitPercCol.Add(hitPerc10);
        ssrtCol.Add(ssrt1);
        ssrtCol.Add(ssrt2);
        ssrtCol.Add(ssrt3);
        ssrtCol.Add(ssrt4);
        ssrtCol.Add(ssrt5);
        ssrtCol.Add(ssrt6);
        ssrtCol.Add(ssrt7);
        ssrtCol.Add(ssrt8);
        ssrtCol.Add(ssrt9);
        ssrtCol.Add(ssrt10);
    }

    IEnumerator LoadLeaderboard()
    {
        yield return new WaitForSeconds(2.0f);
        
            leaderboardData.Sort((x, y) => y.score.CompareTo(x.score));
            for(int i = 0; i < n_rows; i++ )
            {
                Debug.LogError(leaderboardData[i]);
            }
            myIndex = leaderboardData.FindIndex(p => p.tokenId == myToken);
            ShowLeaderBoard();
    }
}
