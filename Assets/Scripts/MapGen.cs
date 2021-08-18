using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [SerializeField]
    int seed;
    [SerializeField]
    bool useSeed;

    [SerializeField]
    GameObject[] prefabs;
    private void Awake()
    {
        Init();
        //StartCoroutine(Init());
    }
    public void Init()
    {
        if (!useSeed)
        {
            seed = Random.Range(0, 999999999);
        }
        Random.InitState(seed);
        Debug.Log(seed);
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

                mapTiles.Add(new Vector2(x, y), new MapTile(tileType, LayoutType.Default, new Vector2(x, y)));
            }
        }

        CreateRooms(length, height, area);
        UpdateMapTiles();
        DrawMap();
        //yield return new WaitForSeconds(5f);
        CarveDoorways();
        //CarveExits();
        DrawMap();
    }

    private void CarveDoorways()
    {
        List<List<Vector2>> roomPositions = new List<List<Vector2>>();
        List<Vector2> unConnected = mapTiles.Where(item => item.Value.tileType == MapTile.TileType.Floor).Select(item => item.Key).ToList();

        int listI = 0;

        //While there is unconnected tiles
        while (unConnected.Count > 0)
        {
            // Create a new room
            roomPositions.Add(new List<Vector2>());
            // Add the first tile from unconnected to the room
            roomPositions[listI].Add(unConnected[0]);
            //remove the first tile from unconnected
            unConnected.Remove(unConnected[0]);

            // check each tile in the room
            for (int i = 0; i < roomPositions[listI].Count; i++)
            {
                // get the neighbours of the tile
                foreach (MapTile item in mapTiles[roomPositions[listI][i]].neighbours)
                {
                    if (item.tileType == MapTile.TileType.Floor)
                    {
                        // If the neighbour is not already in the room list AND the neigbour is in the unconnected list
                        if (!roomPositions[listI].Contains(item.position) && unConnected.Contains(item.position))
                        {
                            // Add the neighbour to the room
                            roomPositions[listI].Add(item.position);
                            // Remove the neighbour from the unconnected list.
                            unConnected.Remove(item.position);
                        }
                    }
                }
            }
            listI++;
        }

        List<List<Vector2>> possibleDoors = new List<List<Vector2>>();

        for (int i = 0; i < roomPositions.Count; i++)
        {
            possibleDoors.Add(new List<Vector2>());
            foreach (Vector2 floorTile in roomPositions[i])
            {

                possibleDoors[i].AddRange(GetPossibleDoorsFromFloorTile(floorTile));
                //foreach (MapTile neighbour in mapTiles[floorTile].neighbours)
                //{
                //    if (!possibleDoors[i].Contains(neighbour.position))
                //    {
                //        if (neighbour.tileType == MapTile.TileType.Wall)
                //        {
                //            if (neighbour.position.x != 0 && neighbour.position.y != 0 && neighbour.position.x != mLength - 1 && neighbour.position.y != mHeight - 1)
                //            {
                //                possibleDoors[i].Add(neighbour.position);
                //            }
                //        }
                //    }

                //}
            }
        }

        

        int roomI = 0;
        bool foundDoor = false;
        while (possibleDoors.Count > 1)
        {
            roomI++;
            if (roomI == possibleDoors.Count)
            {
                roomI = 1;
                // if we looped through all of rooms and didn't find a door to carve in any
                if(!foundDoor)
                {
                    // pick a random possible door from the compared possible door list.
                    int index = Random.Range(0, possibleDoors[1].Count);
                    // make that wall tile a floor tile
                    MapTile editTile = mapTiles[possibleDoors[1][index]];
                    editTile.tileType = MapTile.TileType.Floor;
                    mapTiles[possibleDoors[1][index]] = editTile;

                    // get the list of possible doors from the floor tile we just created
                    List<Vector2> newPosibilities = GetPossibleDoorsFromFloorTile(possibleDoors[1][index]);
                    // add those possible doors with no duplicates into the list of possible doors from the compared room
                    foreach (Vector2 possibleDoor in newPosibilities)
                    {
                        if(!possibleDoors[1].Contains(possibleDoor))
                        {
                            possibleDoors[1].Add(possibleDoor);
                        }
                    }
                    possibleDoors[1].RemoveAt(index);
                }
                foundDoor = false;
            }
            // shared wall tiles between two rooms.
            List<Vector2> sharedDoors = possibleDoors[0].FindAll(item => possibleDoors[roomI].Contains(item));
            if (sharedDoors.Count > 0)
            {
                // pick a random door index
                int index = Random.Range(0, sharedDoors.Count - 1);
                // edit that door tile to make it a floor
                MapTile editTile = mapTiles[sharedDoors[index]];
                editTile.tileType = MapTile.TileType.Floor;
                mapTiles[sharedDoors[index]] = editTile;

                // add the second rooms possible doors to the first room ( no duplicates)
                possibleDoors[0] = possibleDoors[0].Union(possibleDoors[roomI]).ToList();
                // remove the tiles that we just made a floor
                possibleDoors[0].Remove(sharedDoors[index]);

                possibleDoors.RemoveAt(roomI--);

                foundDoor = true;
            }

            

        }
    }

    public List<Vector2> GetPossibleDoorsFromFloorTile(Vector2 floorTilePosition)
    {
        List<Vector2> possibleDoors = new List<Vector2>();

        foreach (MapTile neighbour in mapTiles[floorTilePosition].neighbours)
        {
            if (!possibleDoors.Contains(neighbour.position))
            {
                if (neighbour.tileType == MapTile.TileType.Wall)
                {
                    if (neighbour.position.x != 0 && neighbour.position.y != 0 && neighbour.position.x != mLength - 1 && neighbour.position.y != mHeight - 1)
                    {
                        possibleDoors.Add(neighbour.position);
                    }
                }
            }

        }
        return possibleDoors;
    }

    void CreateRooms(int mapLength, int mapHeight, int mapArea)
    {

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

                maxLength = (int)(mapLength * maxSmallSizePercent) - 2;
                maxHeight = (int)(mapHeight * maxSmallSizePercent) - 2;

                length = Random.Range(minRoomWallSize, maxLength);
                height = Random.Range(minRoomWallSize, maxHeight);
            }
            else if (i < numSmall + numMedium)
            {
                layoutType = LayoutType.DinningHall;

                maxLength = (int)(mapLength * maxMediumSizePercent)-2;
                maxHeight = (int)(mapHeight * maxMediumSizePercent)-2;

                length = Random.Range(minRoomWallSize, maxLength);
                height = Random.Range(minRoomWallSize, maxHeight);
            }
            else
            {
                layoutType = LayoutType.TortureChamber;

                maxLength = (int)(mapLength * maxLargeSizePercent)-2;
                maxHeight = (int)(mapHeight * maxLargeSizePercent)-2;

                length = Random.Range(minRoomWallSize, maxLength);
                height = Random.Range(minRoomWallSize, maxHeight);
            }


            x = Random.Range(2, mapLength - length - 2);
            y = Random.Range(2, mapHeight - height - 2);

            roomPosition = new Vector2(x, y);

            rooms.Add(new Room(length, height, layoutType, roomPosition));
        }
    }

    void UpdateMapTiles()
    {
        rooms.Sort();
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

                    MapTile test = mapTiles[new Vector2(x, y) + position];
                    test.layoutType = layoutType;
                    test.tileType = tileType;
                    mapTiles[new Vector2(x, y) + position] = test;
                    //MapTile newTile = new MapTile(tileType, layoutType, new Vector2(x, y) + position);
                    //mapTiles[new Vector2(x, y) + position] = newTile;
                }
            }
        }

        //Create neighbour lists.
        for (int x = 0; x < mLength; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                Vector2 position = new Vector2(x, y);
                MapTile newTile = mapTiles[position];

                if (x > 0)
                {
                    newTile.neighbours.Add(mapTiles[position + Vector2.left]);
                }
                if (x < mLength - 1)
                {
                    newTile.neighbours.Add(mapTiles[position + Vector2.right]);
                }
                if (y > 0)
                {
                    newTile.neighbours.Add(mapTiles[position + Vector2.down]);
                }
                if (y < mHeight - 1)
                {
                    newTile.neighbours.Add(mapTiles[position + Vector2.up]);
                }

                mapTiles[position] = newTile;
            }
        }
    }

    void DrawMap()
    {
        if (GameObject.Find("Room Container"))
        {
            DestroyImmediate(GameObject.Find("Room Container"));
        }
        GameObject roomContainer = new GameObject("Room Container");

        if (GameObject.Find("Map Container"))
        {
            DestroyImmediate(GameObject.Find("Map Container"));
        }
        GameObject mapContainer = new GameObject("Map Container");

        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject go = new GameObject($"Room {i}");
            float x = rooms[i].length % 2 == 0? -0.5f : 0f;
            float y = rooms[i].height % 2 == 0 ? -0.5f:0f;
            go.transform.position = rooms[i].position + new Vector2(rooms[i].length / 2 + x, rooms[i].height / 2 + y);
            BoxCollider2D collider2D = go.AddComponent<BoxCollider2D>();
            collider2D.size = new Vector2(rooms[i].length, rooms[i].height);
            go.transform.parent = roomContainer.transform;
        }

        foreach (KeyValuePair<Vector2, MapTile> tile in mapTiles)
        {
            GameObject go;
            if (tile.Value.tileType == MapTile.TileType.Wall)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            }
            else
            {
                switch (tile.Value.layoutType)
                {
                    
                    case LayoutType.Storage:
                        go = Instantiate(prefabs[2]);
                        break;
                    case LayoutType.TortureChamber:
                        go = Instantiate(prefabs[0]);
                        break;
                    case LayoutType.DinningHall:
                        go = Instantiate(prefabs[1]);
                        break;
                    case LayoutType.BossRoom:
                        go = Instantiate(prefabs[0]);
                        break;
                    default:
                        go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        break;
                }
                
            }
            go.transform.position = tile.Key;
            go.transform.parent = mapContainer.transform;
            go.name = tile.Key.ToString();
            if (tile.Value.tileType == MapTile.TileType.Floor)
            {
                go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
    public Vector2 position;

    public List<MapTile> neighbours;


    public MapTile(TileType tileType, LayoutType layoutType, Vector2 position)
    {
        this.tileType = tileType;
        this.layoutType = layoutType;
        neighbours = new List<MapTile>();
        this.position = position;
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
}