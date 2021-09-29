using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject terrainTilePrefab = null;    
    private Vector3 terrainSize; //Currently 6.26 x 4.17
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private float noiseScale = 3, cellSize = 1;
    [SerializeField]
    private int radiusToRender = 4;
    [SerializeField]
    private Transform[] gameTransforms;
    [SerializeField]
    private Transform playerTransform;

    private Vector2 startOffset;
    private Dictionary<Vector2, GameObject> terrainTiles = new Dictionary<Vector2, GameObject>();
    private Vector2[] previousCenterTiles;
    private List<GameObject> previousTileObjects = new List<GameObject>();

    private void Start()
    {
        SetTerrainSize();
        InitialLoad();
    }

    public void InitialLoad()
    {
        DestroyTerrain();
        startOffset = new Vector2(UnityEngine.Random.Range(0f, 256f), UnityEngine.Random.Range(0f, 256f));        
    }

    private void SetTerrainSize()
    {        
        SpriteRenderer tileSprite = terrainTilePrefab.GetComponent<SpriteRenderer>();
        Debug.Log(tileSprite.sprite.rect.width);
        terrainSize = new Vector3(tileSprite.sprite.rect.width / tileSprite.sprite.pixelsPerUnit, tileSprite.sprite.rect.height / tileSprite.sprite.pixelsPerUnit, 1);
    }

    private void Update()
    {        
        Vector2 playerTile = TileFromPosition(playerTransform.position);        
        List<Vector2> centerTiles = new List<Vector2>();
        centerTiles.Add(playerTile);
        ChangeTiles(playerTile, centerTiles);
        previousCenterTiles = centerTiles.ToArray();
    }

    private void ChangeTiles(Vector2 playerTile, List<Vector2> centerTiles)
    {
        foreach (Transform t in gameTransforms)
            centerTiles.Add(TileFromPosition(t.position));


        if (previousCenterTiles == null || HaveTilesChanged(centerTiles))
        {
            List<GameObject> tileObjects = new List<GameObject>();
            ActivateNewTiles(playerTile, centerTiles, tileObjects);
            DeactivateOldTiles(tileObjects);
        }
    }

    private void ActivateNewTiles(Vector2 playerTile, List<Vector2> centerTiles, List<GameObject> tileObjects)
    {
        foreach (Vector2 tile in centerTiles)
        {
            bool isPlayerTile = tile == playerTile;
            int radius = isPlayerTile ? radiusToRender : 1;
            for (int i = -radius; i <= radius; i++)
                for (int j = -radius; j <= radius; j++)
                    ActivateOrCreateTile((int)tile.x + i, (int)tile.y + j, tileObjects);
        }
    }

    private void DeactivateOldTiles(List<GameObject> tileObjects)
    {
        foreach (GameObject g in previousTileObjects)
            if (!tileObjects.Contains(g))
                g.SetActive(false);

        previousTileObjects = new List<GameObject>(tileObjects);
    }

    private void ActivateOrCreateTile(int xIndex, int yIndex, List<GameObject> tileObjects)
    {
        if (!terrainTiles.ContainsKey(new Vector2(xIndex, yIndex)))
        {
            tileObjects.Add(CreateTile(xIndex, yIndex));
        }
        else
        {
            GameObject t = terrainTiles[new Vector2(xIndex, yIndex)];
            tileObjects.Add(t);
            if (!t.activeSelf)
                t.SetActive(true);
        }
    }

    private GameObject CreateTile(int xIndex, int yIndex)
    {
        GameObject terrain = Instantiate(
            terrainTilePrefab,
            new Vector3(terrainSize.x * xIndex, terrainSize.y * yIndex, 0),
            Quaternion.identity
        );
        terrain.name = TrimEnd(terrain.name, "(Clone)") + " [" + xIndex + " , " + yIndex + "]";

        terrainTiles.Add(new Vector2(xIndex, yIndex), terrain);       

        return terrain;
    }

    private Vector2 TileFromPosition(Vector3 position)
    {
        return new Vector2(Mathf.FloorToInt(position.x / terrainSize.x), Mathf.FloorToInt(position.y / terrainSize.y));
    }

    private bool HaveTilesChanged(List<Vector2> centerTiles)
    {
        if (previousCenterTiles.Length != centerTiles.Count)
            return true;
        for (int i = 0; i < previousCenterTiles.Length; i++)
            if (previousCenterTiles[i] != centerTiles[i])
                return true;
        return false;
    }

    public void DestroyTerrain()
    {
        foreach (KeyValuePair<Vector2, GameObject> terrain in terrainTiles)
            Destroy(terrain.Value);
        terrainTiles.Clear();
    }

    private static string TrimEnd(string str, string end)
    {
        if (str.EndsWith(end))
            return str.Substring(0, str.LastIndexOf(end));
        return str;
    }

}
