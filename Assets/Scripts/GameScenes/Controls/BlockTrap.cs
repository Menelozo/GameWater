using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrap : MonoBehaviour
{
    public bool horizontalPosition;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (horizontalPosition)
        {
            rb.velocity = new Vector3(5, 0, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, 5, 0);
        }
    }
}
