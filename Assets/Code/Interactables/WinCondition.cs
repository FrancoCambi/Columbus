using UnityEngine;

public class TriggerGanaste : MonoBehaviour
{
    public GameObject pantallaGanaste;

    private void OnTriggerEnter(Collider other)
    {
        pantallaGanaste.SetActive(true);
    }
}