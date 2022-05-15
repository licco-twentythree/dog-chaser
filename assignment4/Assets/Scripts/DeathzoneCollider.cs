using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathzoneCollider : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform playerRespawnPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        player.position = playerRespawnPos.position;
        player.Rotate(Vector3.up * 180f);
        
    }
}
