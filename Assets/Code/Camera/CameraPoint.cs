using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [Header("Objects to Hide")]
    [SerializeField] private GameObject[] objectsToHide;

    public GameObject[] ObjectsToHide => objectsToHide;
}