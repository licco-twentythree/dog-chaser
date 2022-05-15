using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("GameController");
        gm = g.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gm.setPlayerHasWon(true);
        winScreen.SetActive(true);
    }
}
