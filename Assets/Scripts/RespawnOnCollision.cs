using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnCollision : MonoBehaviour
{
    [SerializeField] Transform RespawnLocation;
    [SerializeField] Camera cam;
    [SerializeField] string PlayerLayerString = "Player"; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer(PlayerLayerString))
        {
            col.transform.position = RespawnLocation.position;
            cam.transform.position = RespawnLocation.position;
        }
    }
}
