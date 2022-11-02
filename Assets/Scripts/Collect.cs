using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private PlayerBasicMovement playerStatus;
    private GenerateCollectible collectibles;

    private void Start()
    {
        playerStatus = GameObject.Find("PlayerBall").GetComponent<PlayerBasicMovement>();
        collectibles = GameObject.FindGameObjectWithTag("Ground").GetComponent<GenerateCollectible>();
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerStatus.scoreCount++;
            collectibles.currentNum--;
            Destroy(this.gameObject);
        }
    }
}
