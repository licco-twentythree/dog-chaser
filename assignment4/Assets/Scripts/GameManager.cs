using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool playerCanMove;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void setPlayerCanMove() {
        playerCanMove = true;
    }
}
