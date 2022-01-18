using UnityEngine;

public class SH_Ending : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Key"))
            return;

        FindObjectOfType<SH_GameManager>().Ending();
    }
}
