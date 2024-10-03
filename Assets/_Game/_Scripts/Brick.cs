using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerBrickLists = new List<GameObject>();
    [SerializeField] private GameObject newBrickPrefabs;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject model;

    private Vector3 heightBrickOffSet = new Vector3(0, 0.3f, 0);

    public void AddBrick()
    {
        GameObject newBrick = Instantiate(newBrickPrefabs);
        newBrick.transform.position = playerHead.transform.position + heightBrickOffSet * playerBrickLists.Count;
        newBrick.transform.SetParent(playerHead.transform);

        model.transform.position = newBrick.transform.position;
        playerBrickLists.Add(newBrick);
    }

    public void RemoveBrick()
    {
        if (playerBrickLists.Count > 0)
        {
            GameObject topBrick = playerBrickLists[playerBrickLists.Count - 1];
            playerBrickLists.RemoveAt(playerBrickLists.Count - 1);
            Destroy(topBrick);

            Vector3 newHeightPosition = (playerBrickLists.Count > 0)
                ? playerBrickLists[playerBrickLists.Count - 1].transform.position
                : playerHead.transform.position;
            model.transform.position = newHeightPosition;
        }
    }

    public void ClearAllBrick()
    {
        foreach (var brick in playerBrickLists)
        {
            Destroy(brick);
        }

        playerBrickLists.Clear();

        model.transform.position = playerHead.transform.position;
    }

}
