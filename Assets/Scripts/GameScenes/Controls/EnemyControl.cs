using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class EnemyControl : MonoBehaviour
{
    public event EventHandler OnCollisionWithGhostTile;
    public bool IgnoreStart = false;

    public event EventHandler <OnCollisionWithFakeGhostTileEventArgs> OnCollisionWithFakeGhostTile;
    public class OnCollisionWithFakeGhostTileEventArgs : EventArgs
    {
        public Transform Transform; 
        public ContactPoint2D Point;
    }

    public float moveSpeed = 1f;
    public float dX = 0;
    public float dY = 0;
    Rigidbody2D rb;

    Vector2 velocityResume;
    State state = State.Waiting;
    State stateResume;

    internal enum State
    {
        Moving,
        Stopped,
        Waiting,
    }

    void Update()
    {
        switch (state)
        {
            case State.Moving:
                CheckMovement();
                break;
            case State.Stopped:
                break;
            case State.Waiting:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == State.Moving)
        {
            if (collision.gameObject.CompareTag("TilemapGhost"))
            {
                OnCollisionWithGhostTile?.Invoke(this, EventArgs.Empty);
            }
            
            if (collision.gameObject.CompareTag("TilemapFakeGhost"))
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    OnCollisionWithFakeGhostTile?.Invoke(this, new OnCollisionWithFakeGhostTileEventArgs { Transform = transform, Point = point }); 
                }
            }
        }
    }

    void CheckMovement()
    {
        if (rb.velocity.x == 0 || rb.velocity.y == 0)
        {
            if (!IgnoreStart)
            {
                Vector2 movement = new(dX, dY);
                rb.velocity = moveSpeed * movement;
            }
        }
    }

    public static double CalculateY(double x)
    {
        if (x < -1.0 || x > 1.0)
        {
            throw new ArgumentOutOfRangeException("x must be in the range -1 to 1.");
        }

        return Math.Sqrt(1 - x * x);
    }

    internal void StartMovement()
    {
        Random rand = new();
        double x = rand.NextDouble() * 2 - 1;
        double y = CalculateY(x);
        dX = (float)x;
        dY = (float)y;
        rb = GetComponent<Rigidbody2D>();

        Vector2 movement = new(dX, dY);
        if (!IgnoreStart)
        {
            rb.velocity = moveSpeed * movement;
        }

        state = State.Moving;
    }

    internal void EnemyMoving()
    {
        state = stateResume;
        if (state == State.Moving)
        {
            rb.velocity = velocityResume;
            velocityResume = Vector2.zero;
        }
    }

    internal void EnemyStopped()
    {
        stateResume = state;
        state = State.Stopped;
        if (stateResume == State.Moving)
        {
            velocityResume = new Vector2(rb.velocity.x, rb.velocity.y);
            rb.velocity = Vector2.zero;
        }
    }

    internal bool CheckColisionWithTiles(List<Vector3Int> tiles, Tilemap tilemapBackground)
    {
        Vector3 cellPositionConverted = GetEnemyPoint(tilemapBackground);

        if (cellPositionConverted != null)
        {
            foreach (var tile in tiles)
            {
                if (cellPositionConverted.x == tile.x && cellPositionConverted.y == tile.y)
                {
                    return true;
                }
            }
        }
        return false;
    }

    internal Vector3 GetEnemyPoint(Tilemap tilemapBackground)
    {
        Vector3 enemyCenter = transform.position;
        Vector3Int cellPosition = tilemapBackground.WorldToCell(enemyCenter);
        return new Vector3(cellPosition.x, cellPosition.y, cellPosition.z);
    }
}
