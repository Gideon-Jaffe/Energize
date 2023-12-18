using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class ShotControllerBase : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] protected Tile tileToInteractWith;

    [SerializeField] private int blastRadius;

    protected Tilemap tilemap;

    [HideInInspector] public bool facingRight;

    private Vector3 direction;

    protected bool exploded = false;  
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected void Init()
    {
        GetComponent<SpriteRenderer>().flipX = !facingRight;
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        direction = facingRight? Vector3.right : Vector3.left;
    
    }

    // Update is called once per frame
    void Update()
    {
        CheckForExploded();

        if (!exploded)
        {
            transform.position += direction * speed;
        }
    }

    public abstract void CheckForExploded();
}
