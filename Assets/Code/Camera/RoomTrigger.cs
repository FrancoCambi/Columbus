using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform cameraPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        RoomCameraController cameraController = FindFirstObjectByType<RoomCameraController>();

        if (cameraController != null)
        {
            cameraController.ChangeRoom(cameraPoint);
        }
    }
}