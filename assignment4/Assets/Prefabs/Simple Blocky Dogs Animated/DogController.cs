using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    private Animator animator;
    //[SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject dog;
    [SerializeField] private float deactivateTime;
    private NavMeshAgent agent;
    [SerializeField] private Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Walk", 5f);
        Invoke("Destroy", deactivateTime);
    }

    private void Walk() {
        //rb.AddForce(Vector3.left * speed * Time.deltaTime, ForceMode.Impulse);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        animator.SetBool("isWalking", true);
        agent.destination = destination.position;
    }

    private void Destroy()
    {
        dog.SetActive(false);
    }

}
