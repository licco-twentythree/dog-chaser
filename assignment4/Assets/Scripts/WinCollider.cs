using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCollider : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("GameController");
        gm = g.GetComponent<GameManager>();
    }

    private void Update()
    {
        playerLost();
    }

    private void playerLost() {
        if (gm.playerHasLost == true) {
            loseScreen.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
                ReloadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gm.setPlayerHasWon(true);
        winScreen.SetActive(true);
    }

    void ReloadScene() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
