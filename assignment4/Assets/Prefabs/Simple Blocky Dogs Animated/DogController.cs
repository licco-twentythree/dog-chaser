using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    private Animator animator;
    //[SerializeField] private Rigidbody rb;
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject dog;
    [SerializeField] private float deactivateTime;
    private NavMeshAgent agent;
    public GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        GameObject g = GameObject.FindGameObjectWithTag("GameController");
        gm = g.GetComponent<GameManager>();

        Invoke("Walk", 5f);
        Invoke("Destroy", deactivateTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Walk() {
        //rb.AddForce(Vector3.left * speed * Time.deltaTime, ForceMode.Impulse);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        animator.SetBool("isWalking", true);
        agent.destination = destination.position;
    }

    private void Destroy()
    {
        gm.setPlayerCanMove();
        dog.SetActive(false);
    }

}
