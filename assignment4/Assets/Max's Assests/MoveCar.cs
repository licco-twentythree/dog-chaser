using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private float carSpeed;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = car.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        Drive();
        if (car.transform.position.z > startingPos.z + maxDistance || car.transform.position.z < startingPos.z - maxDistance) {           
            Respawn();
        }

    }

    void Drive() {
        car.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);
    }

    void Respawn() {
        car.transform.position = startingPos;
    }
}
