using System.Collections.Generic;
using UnityEngine;

public class PlayerBrick : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerBrickLists = new List<GameObject>();
    [SerializeField] private GameObject newBrickPrefabs;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject player;
    private Vector3 heightBrick = new Vector3(0,0.3f,0);

    public void AddBrick()
    {
        if (playerBrickLists != null)
        {
            // Add brick
            var newBrick = Instantiate(newBrickPrefabs);
            newBrick.transform.position = playerHead.transform.position + heightBrick * playerBrickLists.Count ;
            newBrick.transform.parent = playerHead.transform;
            player.transform.position = newBrick.transform.position;
            playerBrickLists.Add(newBrick);
        }
    }

    public void RemoveBrick()
    {
        if(playerBrickLists.Count > 0)
        {
            var topBrick = playerBrickLists[playerBrickLists.Count - 1];

            playerBrickLists.RemoveAt(playerBrickLists.Count - 1);
            Destroy(topBrick);

            if(playerBrickLists.Count > 0)
            {
                var newTopBrick = playerBrickLists[playerBrickLists.Count - 1];
                player.transform.position = newTopBrick.transform.position;
            }
            else
            {
                player.transform.position = playerHead.transform.position;
            }
        }
    }

    public void ClearAllBrick()
    {
        if (playerBrickLists.Count > 0)
        {
            foreach (var brick in playerBrickLists)
            {
                Destroy(brick);
            }

            playerBrickLists.Clear();
        }

        player.transform.position = playerHead.transform.position;
    }
   
}
