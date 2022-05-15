using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CountDownTimer : MonoBehaviour
{
    public GameManager gm;
    public float time;
    [SerializeField] private float startingTime;
    [SerializeField] TextMeshProUGUI countDownText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("GameController");
        gm = g.GetComponent<GameManager>();
       

        time = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.readytoCountdown == true)
        {
            CountDown();
        }
    }

    void CountDown() {
            time -= 1 * Time.deltaTime;
            countDownText.text = time.ToString("0.0") + "s";
        if (time <= 0f) {
            gm.setPlayerHasLost(true);
        }
    }
}
