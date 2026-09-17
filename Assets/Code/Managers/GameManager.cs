using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerCharacter playerCharacter;

    [Header("Camera")]
    [SerializeField] private RoomCameraController cameraController;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        // colocar el jugador en su SpawnPoint

        if (playerCharacter != null && playerCharacter.SpawnPoint != null)
        {
            playerCharacter.transform.position = playerCharacter.SpawnPoint.position;

            playerCharacter.transform.rotation = playerCharacter.SpawnPoint.rotation;
        }

        // colocar la cámara en el CameraPoint correspondiente

        if (playerCharacter != null && playerCharacter.CameraPoint != null)
        {
            cameraController.ChangeRoom(playerCharacter.CameraPoint);
        }
    }
}
