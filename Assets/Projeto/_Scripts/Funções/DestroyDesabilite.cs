using UnityEngine;

public class DestroyDesabilite : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Desabilite()
    {
        gameObject.SetActive(false);
    }
}
