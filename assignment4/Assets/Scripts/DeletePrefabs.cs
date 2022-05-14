using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePrefabs : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        parent.SetActive(false);
    }

}
