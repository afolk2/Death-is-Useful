using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    List<Room> rooms;
    Dictionary<Vector2, MapTile> mapTiles;

    [SerializeField]
    int minMapSize;

    [SerializeField]
    int maxMapSize;

    [SerializeField]
    int minRoomWallSize;

    [SerializeField]
    [Range(0, 1)]
    float minRoomAreaPercent;

    [SerializeField]
    [Range(0, 1)]
    float maxSmallSizePercent;

    [SerializeField]
    [Range(0, 1)]
    float maxMediumSizePercent;

    [SerializeField]
    [Range(0, 1)]
    float maxLargeSizePercent;

    IEnumerator enumerator;

    [SerializeField]
    int minNumSmall;
    [SerializeField]
    int maxNumSmall;
    [SerializeField]
    int minNumMedium;
    [SerializeField]
    int maxNumMedium;
    [SerializeField]
    int minNumLarge;
    [SerializeField]
    int maxNumLarge;

    [SerializeField]
    int mLength;
    [SerializeField]
    int mHeight;

    private void Awake()
    {
        //Init();
    }
    private void Update()
    {
        if (enumerator != null)
        {
            enumerator.MoveNext();
        }
    }
    public void Init()
    {
        rooms = new List<Room>();
        mapTiles = new Dictionary<Vector2, MapTile>();

        int length = Random.Range(minMapSize, maxMapSize);
        int height = Random.Range(minMapSize, maxMapSize);
        mLength = length;
        mHeight = height;
        int area = length * height;

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MapTile.TileType tileType = MapTile.TileType.Floor;
                if (x == 0 || x == length - 1 || y == 0 || y == height - 1)
                {
                    tileType = MapTile.TileType.Wall;
                }

                mapTiles.Add(new Vector2(x, y), new MapTile(tileType, LayoutType.Default));
            }
        }

        CreateRooms(length, height, area);

        DrawMap();
    }

    void CreateRooms(int mapLength, int mapHeight, int mapArea)
    {
        int minRoomArea = minRoomWallSize * minRoomWallSize;
        int maxSmallArea = (int)(mapArea * maxSmallSizePercent);
        int maxMediumArea = (int)(mapArea * maxMediumSizePercent);
        int maxLargeArea = (int)(mapArea * maxLargeSizePercent);

        float mapPerc = (float)(mapLength * mapHeight) / (maxMapSize * maxMapSize);

        int numSmall = Random.Range(minNumSmall, maxNumSmall);

        int numMedium = Random.Range(minNumMedium, maxNumMedium);

        int numLarge = Random.Range(minNumLarge, maxNumLarge);

        LayoutType layoutType;

        for (int i = 0; i < numSmall + numMedium + numLarge; i++)
        {
            Vector2 roomPosition;
            int x;
            int y;
            int length;
            int height;
            int maxLength;
            int maxHeight;



            if (i < numSmall)
            {
                layoutType = LayoutType.Storage;

                maxLength = (int)(mapLength * maxSmallSizePercent);

                length = Random.Range(minRoomWallSize, maxLength);
                maxHeight = Mathf.Min(maxSmallArea / length, mapHeight);
                height = Random.Range(minRoomWallSize, maxHeight);


            }
            else if (i < numSmall + numMedium)
            {
                layoutType = LayoutType.DinningHall;
                maxLength = (int)(mapLength * maxMediumSizePercent);

                length = Random.Range(minRoomWallSize, maxLength);
                maxHeight = Mathf.Min(maxMediumArea / length, mapHeight);
                height = Random.Range(minRoomWallSize, maxHeight);
            }
            else
            {
                layoutType = LayoutType.TortureChamber;

                maxLength = (int)(mapLength * maxLargeSizePercent);

                length = Random.Range(minRoomWallSize, maxLength);
                maxHeight = Mathf.Min(maxLargeArea / length, mapHeight);
                height = Random.Range(minRoomWallSize, maxHeight);
            }


            x = Random.Range(0, mapLength - length);
            y = Random.Range(0, mapHeight - height);

            roomPosition = new Vector2(x, y);

            rooms.Add(new Room(length, height, layoutType, roomPosition));
        }
    }

    void DrawMap()
    {
        rooms.Sort();



        if (GameObject.Find("Room Container"))
        {
            DestroyImmediate(GameObject.Find("Room Container"));
        }
        GameObject roomContainer = new GameObject("Room Container");

        #region room gen old
        for (int r = 0; r < rooms.Count; r++)
        {
            Vector2 rPosition = rooms[r].position;
            int rLength = rooms[r].length;
            int rHeight = rooms[r].height;

            for (int x = 0; x < rLength; x++)
            {
                for (int y = 0; y < rHeight; y++)
                {
                    if (r > 0)
                    {
                        for (int rcheck = r - 1; rcheck >= 0; rcheck--)
                        {
                            if (!rooms[rcheck].Contains(new Vector2(x, y) + rPosition))
                            {
                                MapTile.TileType tileType = MapTile.TileType.Floor;
                                if (x == 0 || x == rLength - 1 || y == 0 || y == rHeight - 1)
                                {
                                    tileType = MapTile.TileType.Wall;
                                }
                                mapTiles[new Vector2(x, y) + rPosition] = new MapTile(tileType, rooms[r].layoutType);
                            }
                        }
                    }
                    else
                    {
                        MapTile.TileType tileType = MapTile.TileType.Floor;
                        if (x == 0 || x == rLength - 1 || y == 0 || y == rHeight - 1)
                        {
                            tileType = MapTile.TileType.Wall;
                        }
                        mapTiles[new Vector2(x, y) + rPosition] = new MapTile(tileType, rooms[r].layoutType);
                    }
                }
            }

            GameObject room = new GameObject($"Room {r + 1}");

            BoxCollider2D collider2D = room.AddComponent<BoxCollider2D>();
            collider2D.size = new Vector2(rLength - 1, rHeight - 1);
            room.transform.position = rPosition + collider2D.size / 2;
            room.transform.parent = roomContainer.transform;
        }

        #endregion

        #region room gen new
        rooms.Reverse();
        for (int i = 0; i < rooms.Count; i++)
        {
            Vector2 position = rooms[i].position;
            LayoutType layoutType = rooms[i].layoutType;

            for (int x = 0; x < rooms[i].length; x++)
            {
                for (int y = 0; y < rooms[i].height; y++)
                {
                    MapTile.TileType tileType = MapTile.TileType.Floor;
                    if (x == 0 || x == rooms[i].length - 1 || y == 0 || y == rooms[i].height - 1)
                    {
                        tileType = MapTile.TileType.Wall;
                    }

                    MapTile newTile = new MapTile(tileType, layoutType);
                    mapTiles[new Vector2(x, y) + position] = newTile;
                }
            }
        }
        #endregion
        if (GameObject.Find("Map Container"))
        {
            DestroyImmediate(GameObject.Find("Map Container"));
        }
        GameObject mapContainer = new GameObject("Map Container");

        foreach (KeyValuePair<Vector2, MapTile> tile in mapTiles)
        {
            if (tile.Value.tileType == MapTile.TileType.Wall)
            {
                GameObject newWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                newWall.transform.position = tile.Key;
                newWall.transform.parent = mapContainer.transform;
            }
            else
            {
                GameObject newFloor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newFloor.transform.position = tile.Key;
                newFloor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                newFloor.transform.parent = mapContainer.transform;
            }
        }
    }

}

public class Room : System.IComparable<Room>
{
    public LayoutType layoutType { get; private set; }

    public Vector2 position { get; private set; }

    int area;

    public int length { get; private set; }
    public int height { get; private set; }

    public Room(int length, int height, LayoutType layoutType, Vector2 position)
    {
        this.length = length;
        this.height = height;
        area = length * height;
        this.layoutType = layoutType;
        this.position = position;
    }

    public bool Contains(Vector2 compare)
    {
        return (compare.x > this.position.x && compare.x < this.position.x + length) && (compare.y > this.position.y && compare.y < this.position.y + height);
    }
    public int CompareTo(Room other)
    {
        int value = 0;
        if (area < other.area)
        {
            value = -1;
        }
        else if (area > other.area)
        {
            value = 1;
        }
        return value;
    }

}
public struct MapTile
{
    public enum TileType { Wall, Floor }
    public TileType tileType;
    public LayoutType layoutType;



    public MapTile(TileType tileType, LayoutType layoutType)
    {
        this.tileType = tileType;
        this.layoutType = layoutType;
    }
}

/// <summary>
/// Probably replace this with a scriptableObject or something similar?
/// </summary>
public enum LayoutType
{
    Default,
    Storage,
    TortureChamber,
    DinningHall,
    BossRoom
}