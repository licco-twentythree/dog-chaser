using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CountDownTimer : MonoBehaviour
{
    public GameManager gm;
    private float time;
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
        time -= 1 * Time.deltaTime;
        countDownText.text = time.ToString("0.##");

    }
}
