using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    [SerializeField] private GameObject chest;
    [SerializeField] private Player player;

    public void WhenPlayerWin()
    {
        player.transform.position = chest.transform.position;
    }
}
