using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RocketController : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Sprite explosionTileSprite;

    [SerializeField] private int blastRadius;

    private Tilemap tilemap;

    public bool facingRight;

    private Vector3 direction;

    private bool exploded = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = !facingRight;
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        direction = facingRight? Vector3.right : Vector3.left;
    }

    void FixedUpdate()
    {
        CheckForExploded();

        if (!exploded)
        {
            transform.position += direction * speed;
        }
    }

    private void CheckForExploded()
    {
        Vector3Int tileLocation = tilemap.WorldToCell(transform.position);
        Sprite tileSprite = tilemap.GetSprite(tileLocation);

        if (tileSprite != null)
        {
            exploded = true;
            DestroyTileAndAdjacent(tileLocation);
        }
    }

    private void DestroyTileAndAdjacent(Vector3Int tileLocation)
    {
        GetComponent<Animator>().SetBool("Exploded", true);

        for (int i = -blastRadius; i <= blastRadius; i++)
        {
            for (int j = -blastRadius; j <= blastRadius; j++)
            {
                Vector3Int newLocation = tileLocation + new Vector3Int {x = i, y = j, z = 0};
                Sprite tileSprite = tilemap.GetSprite(newLocation);
                if (tileSprite == explosionTileSprite)
                {
                    tilemap.SetTile(newLocation, null);
                }
            }
        }
    }
}
