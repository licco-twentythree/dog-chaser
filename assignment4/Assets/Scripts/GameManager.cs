using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool playerCanMove;
    public bool readytoCountdown;
    public bool playerHasWon;
    public bool playerHasLost;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void setPlayerCanMove(bool set) {
        playerCanMove = set;
    }

    public void setReadytoCountDown(bool set) {
        readytoCountdown = set;
    }

    public void setPlayerHasWon(bool set)
    {
        playerHasWon = set;
    }
    
    public void setPlayerHasLost(bool set)
    {
        playerHasLost = set;
    }
}
