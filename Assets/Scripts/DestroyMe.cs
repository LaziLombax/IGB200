using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float check = 28f;
    private void Update()
    {
        if (GameHandler.Instance.player.gameObject.transform.position.z > transform.position.z + check) Destroy(gameObject);
    }
}
