using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private GameObject player;
    [SerializeField] private float xOffset, yOffset, zOffset;
    [SerializeField] private float xRotation, yRotation, zRotaion;

    private void LateUpdate()
    {
        CameraFollowPlayer();
    }

    private void CameraFollowPlayer()
    {
        transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z + zOffset);
        transform.rotation = Quaternion.Euler(transform.rotation.x + xRotation, transform.rotation.y + yRotation, transform.rotation.z + zRotaion);
    }
}
