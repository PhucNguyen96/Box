using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject box;

    public void Spawn()
    {
        box.transform.position = transform.position;

        Instantiate(box);
    }
}
