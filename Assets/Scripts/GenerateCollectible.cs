using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCollectible : MonoBehaviour
{
    [SerializeField] GameObject CollectiblePrefab;
    private int maxNum = 2;
    public int currentNum = 0;
    private GameObject playerBall;
    private PlayerBasicMovement playerStatus;
    private Transform collectibleT;
    void Start()
    {
        playerBall = GameObject.Find("PlayerBall");
        playerStatus = playerBall.GetComponent<PlayerBasicMovement>();
        collectibleT = CollectiblePrefab.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //spawn during the game
        if (!playerStatus.gameStop)
        {
            if (currentNum < maxNum)
            {
                Vector3 newSpawnPos = new Vector3(Random.Range(-5.5f, 8f) + collectibleT.position.x, -3.5f, collectibleT.position.z);
                GameObject newCollectible = Instantiate(CollectiblePrefab, newSpawnPos, Quaternion.identity);
                currentNum++;
            }

        }
    }

    
}
