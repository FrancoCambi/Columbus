using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [Header("Objects to Hide")]
    [SerializeField] private GameObject[] objectsToHide;

    public GameObject[] ObjectsToHide => objectsToHide;

    public void HideObjects()
    {
        foreach (GameObject objectToHide in objectsToHide)
        {
            MeshRenderer meshRenderer = objectToHide.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
    }
}