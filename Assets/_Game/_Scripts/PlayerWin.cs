using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    [SerializeField] private GameObject standFrontChestPostion;
    [SerializeField] private Player player;

    public void WhenPlayerWin()
    {
        player.transform.position = standFrontChestPostion.transform.position;
    }
}
