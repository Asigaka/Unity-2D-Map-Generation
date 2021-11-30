using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapMove { Up, Down, Left, Right}
public class MapGenerate : MonoBehaviour
{
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;
    [SerializeField] private int scale = 20;
    [SerializeField] private float xOffset;
    [SerializeField] private float xOffsetStep = 0.1f;
    [SerializeField] private float yOffset;
    [SerializeField] private float yOffsetStep = 0.1f;

    [Space(4)]
    [SerializeField] private List<MapPiece> mapPieces = new List<MapPiece>();
    [SerializeField] private Transform mapContent;

    public static MapGenerate Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    public void GenerateMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                SpawnPiece(x, y);
    }

    private void SpawnPiece(int x, int y)
    {
        Instantiate(GetPieceByNumber(GetPerlinNumber(x, y)).Prefab, new Vector2(x, y), Quaternion.identity, mapContent);
    }

    private int GetPerlinNumber(int x, int y)
    {
        float xCoord = (float)x / width * scale + xOffset;
        float yCoord = (float)y / height * scale + yOffset;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        float mSample = sample * 10;

        return (int)mSample;
    }

    private MapPiece GetPieceByNumber(int number)
    {
        foreach (MapPiece piece in mapPieces)
            if (piece.PieceLayer == number)
                return piece;

        return null;
    }

    public void MoveMap(MapMove move)
    {
        switch (move)
        {
            case MapMove.Left: xOffset -= xOffsetStep; break;
            case MapMove.Right: xOffset += xOffsetStep; break;
            case MapMove.Up: yOffset += yOffsetStep; break;
            case MapMove.Down: yOffset -= yOffsetStep; break;
        }

        UpdateMap();
    }

    public void MoveMapUp() => MoveMap(MapMove.Up);
    public void MoveMapDown() => MoveMap(MapMove.Down);
    public void MoveMapLeft() => MoveMap(MapMove.Left);
    public void MoveMapRight() => MoveMap(MapMove.Right);

    public void UpdateMap()
    {
        ClearMap();
        GenerateMap();
    }

    private void ClearMap()
    {
        for (int i = 0; i < mapContent.childCount; i++)
            Destroy(mapContent.GetChild(i).gameObject);
    }
}
