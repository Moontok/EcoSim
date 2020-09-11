using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class GroundCreator : MonoBehaviour
{
    [Space]
    [Header("Properties")]
    [SerializeField] Vector2Int landSize = new Vector2Int(1,1);
    [Space]
    [SerializeField] float tileSize = 1f;
    [SerializeField] float xOrg = 0;
    [SerializeField] float yOrg = 0;
    [SerializeField] float noiseScale = 1f;

    [Space]
    [Header("Ground Tiles")]
    [SerializeField] GameObject tile1 = null;
    [SerializeField] GameObject tile2 = null;
    [SerializeField] GameObject tile3 = null;    
    [SerializeField] [Range(0,1)] float tileBand1 = .3f;
    [SerializeField] [Range(0,1)] float tileBand2 = .6f;

    [Space]
    [Header("Edible Vegitation")]
    [SerializeField] GameObject veg = null;
    [SerializeField] [Range(0,1)] float vegDensity = 0f;

    [Space]
    [Header("Non-edible Plants and Rocks")]
    [SerializeField] GameObject tree = null;
    [SerializeField] Vector2 treeScaleRange = new Vector2(1,1);
    [SerializeField][Range(0,1)] float treeDensity = 0f;

    [Space]
    [SerializeField] bool autoUpdate = true;
    
    Vector3 startingTile = Vector3.zero;
    Vector3 currentTile = Vector3.zero;

    // TODO Track tile locations for tile change effects.
    List<Vector2> tilesWithGrass = new List<Vector2>();

    // TODO Track GameObjects instead of Vector2 locations.
    List<Vector2> treeLocations = new List<Vector2>();
    List<Vector2> edibleGrassLocations = new List<Vector2>();
    List<int> randNumbers = new List<int>();
    
    void Update() 
    {
        if(this.transform.hasChanged)
        {
            this.transform.hasChanged = false;
            return;
        }
        if(Selection.Contains(this.gameObject) && this.isActiveAndEnabled && autoUpdate)
        { 
            GenerateLand();
        }

    }

    public void GenerateLand()
    {
        DeleteLand(); // Clear all current land tiles and foliage.
                
        CalcStartingTile();

        float[,] tileNoise = CalcNoise();
        GameObject tileToSet = null;

        for (int x = 0; x < landSize.x; x++)
        {
            currentTile.z = startingTile.z;
            for (int z = 0; z < landSize.y; z++)
            {
                float tileNoiseSample = tileNoise[z,x];
                if(tileNoiseSample < tileBand1) 
                {
                    tileToSet = tile1;
                }
                else if(tileNoiseSample < tileBand2) 
                {
                    tileToSet = tile2;
                }
                else 
                {
                    tileToSet = tile3;
                    float xLoc = currentTile.x;
                    float zLoc = currentTile.z;
                    tilesWithGrass.Add(new Vector2(xLoc, zLoc));
                }

                GameObject tile = Instantiate(tileToSet, currentTile, Quaternion.identity, this.gameObject.transform);
                tile.transform.localScale = new Vector3(tileSize, 1, tileSize);
                currentTile.z += tileSize;                
            }
            currentTile.x += tileSize;
        }
        AddFoliage(veg, vegDensity, tilesWithGrass, edibleGrassLocations, randNumbers, new Vector2(1,1));
        AddFoliage(tree, treeDensity, tilesWithGrass, treeLocations, randNumbers, treeScaleRange);    
    }

    public void DeleteLand()
    {
        List<GameObject> children = new List<GameObject>();
        
        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }
        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
        currentTile = startingTile;
        tilesWithGrass.Clear();
        edibleGrassLocations.Clear();
        treeLocations.Clear();
        randNumbers.Clear();
    }

    float[,] CalcNoise()
    {        
        float [,] tileNoise = new float[landSize.y, landSize.x];
        float sample = 0f;

        for (int y = 0; y < landSize.y; y++)
        {
            for (int x = 0; x < landSize.x; x++)
            {
                float xCoord = xOrg + (float)x / (float)landSize.x * noiseScale;
                float yCoord = yOrg + (float)y / (float)landSize.y * noiseScale;
                sample = Mathf.PerlinNoise(xCoord, yCoord);
                tileNoise[y, x] = sample;                
            }
        }
        return tileNoise;
    }

    void CalcStartingTile()
    {
        float coordx = this.transform.position.x - landSize.x * tileSize / 2 + tileSize / 2;
        float coordy = this.transform.position.y;
        float coordz = this.transform.position.z - landSize.y * tileSize / 2 + tileSize / 2;

        startingTile = new Vector3(coordx, coordy, coordz);
    }

    void AddFoliage(GameObject plantToPlace, float density, List<Vector2> baseTiles, List<Vector2> flora, List<int> randomNumbers, Vector2 scaleRange)
    {        
        int numberToCreate = (int)(density * baseTiles.Count);

        while(flora.Count < numberToCreate)
        {
            int randNum = (int)UnityEngine.Random.Range(0, baseTiles.Count);
            if(!randomNumbers.Contains(randNum))
            {
                randomNumbers.Add(randNum);
                flora.Add(baseTiles[randNum]);
            }
        }
        
        foreach (Vector2 location in flora)
        {
            GameObject plant = Instantiate(plantToPlace, new Vector3(location.x, 0.0f, location.y), Quaternion.identity, this.gameObject.transform);
            float scale = UnityEngine.Random.Range(scaleRange.x, scaleRange.y);
            plant.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void OnValidate() 
    {
        landSize.x = Mathf.Max(landSize.x, 1);
        landSize.y = Mathf.Max(landSize.y, 1);
        tileSize = Mathf.Max(tileSize, 0);
    }
}
