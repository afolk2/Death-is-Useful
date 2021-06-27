using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    enum ExitDirection
    {
        North,
        South,
        East,
        West
    }


    // Used to store the last maps exit direction
    ExitDirection exitDirection;
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

    List<Vector2> mapLayout;

    public bool debug;


    private void Awake()
    {
        mapLayout = new List<Vector2>();
        GenerateMap();
    }

    public void GenerateMap()
    {
        //Length, width, and area of the map.
        int length = Random.Range(minMapSize, maxMapSize);
        int width = Random.Range(minMapSize, maxMapSize);
        int area = length * width;

        GenerateRooms(length, width, area);

        //CutExits();
    }

    void GenerateRooms(int mapLength, int mapHeight, int mapArea)
    {
        // Get the smallest room area (used for the smallest small room)
        int minRoomArea = (int)(minRoomAreaPercent * mapArea);

        // Max room areas.
        int maxSmallArea = (int)(mapArea * maxSmallSizePercent);
        int maxMediumArea = (int)(mapArea * maxMediumSizePercent);
        int maxLargeArea = (int)(mapArea * maxLargeSizePercent);

        // Clears the Console in between generating maps 
        UsefulShortcuts.ClearConsole();
        float mapPerc = (float)(mapLength * mapHeight) / (maxMapSize * maxMapSize);
        // Get a random number of small, medium, and large rooms based off of the map area and min/max room size.
        int numSmall = Mathf.Max((int)(Random.Range(mapArea / maxSmallArea, mapArea / minRoomArea) * (float)mapPerc), 1);

        int numMedium = Mathf.Max((int)(Random.Range(mapArea / maxMediumArea, mapArea / maxSmallArea) * (float)mapPerc), 1);

        int numLarge = Mathf.Max((int)(Random.Range(mapArea / maxLargeArea, mapArea / maxMediumArea) * (float)mapPerc), 1);

#if UNITY_EDITOR
        if (debug)
        {
            Debug.Log($"Map Percent: {mapPerc} Map Length: {mapLength} Map Height: {mapHeight}");
            Debug.Log($"numSmall = Random.Range({mapArea / maxSmallArea},{mapArea / minRoomArea}) = {numSmall}");
            Debug.Log($"numMedium = Random.Range({mapArea / maxMediumArea},{mapArea / maxSmallArea}) = {numMedium}");
            Debug.Log($"numSmall = Random.Range({mapArea / maxLargeArea},{mapArea / maxMediumArea}) = {numLarge}");
        }
#endif

        List<Room> rooms = new List<Room>();

        // Probably a better way to do this section
        // Generating each sized room.
        for (int i = 0; i < numSmall; i++)
        {
            rooms.Add(MakeRoom(mapLength, mapHeight, minRoomArea, maxSmallArea));
            rooms[i].Init();
        }
        for (int i = numSmall; i < numSmall + numMedium; i++)
        {
            rooms.Add(MakeRoom(mapLength, mapHeight, maxSmallArea, maxMediumArea));
            rooms[i].Init();
        }
        for (int i = numSmall + numMedium; i < numSmall + numMedium + numLarge; i++)
        {
            rooms.Add(MakeRoom(mapLength, mapHeight, maxMediumArea, maxLargeArea));
            rooms[i].Init();
        }

        //ToDo: Tilesets for rooms.

        //

        #region Debug Room Creation

        if (debug)
        {
            if (GameObject.Find("Debug Map Container"))
            {
#if UNITY_EDITOR
                DestroyImmediate(GameObject.Find("Debug Map Container"));
#else
            Destroy(GameObject.Find("Debug Map Container"));
#endif
            }
            GameObject debugMapContainer = new GameObject("Debug Map Container");

            rooms.Sort();
            rooms.Reverse();

            for (int i = 0; i < rooms.Count; i++)
            {
                GameObject debugRoomContainer = new GameObject($"Room {i}- Area: {rooms[i].area} L/H: {rooms[i].length}/{rooms[i].height}");
                debugRoomContainer.transform.parent = debugMapContainer.transform;

                for (int x = (int)rooms[i].startingPosition.x; x < rooms[i].startingPosition.x + rooms[i].length; x++)
                {
                    for (int y = (int)rooms[i].startingPosition.y; y < rooms[i].startingPosition.y + rooms[i].height; y++)
                    {
                        if (rooms[i].layout.Contains(new Vector2(x, y)))
                        {
                            // Draw Wall
                            GameObject debugWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                            debugWall.transform.position = new Vector3(x, y);
                            debugWall.transform.parent = debugRoomContainer.transform;
                        }
                    }
                }
                foreach (Vector2 location in rooms[i].layout)
                {

                }
            }
        }

        #endregion

        // Check for and cut intersecting rooms.
        CutRooms(rooms, mapLength, mapHeight);

        // Creates some quads to view the map layout for now.
        DrawMap();
    }

    Room MakeRoom(int mapLength, int mapHeight, int minRoomArea, int maxRoomArea)
    {
        Room newRoom;
        // random area based on min and max room area.
        int area = Random.Range(minRoomArea, maxRoomArea);
        // the start position (lower left) of the room
        Vector2 start = new Vector2(
            Random.Range(0, mapLength),
            Random.Range(0, mapHeight));

        if (start.x >= mapLength - minRoomWallSize || start.y >= mapHeight - minRoomWallSize)
        {
            // Empty Room
            newRoom = new Room(new Vector2(0, 0), 0, 0);
        }
        else
        {
            // Random value between 5 and 1/5 of the area or whatever length is left over from our starting position, whichever is smaller. Keeps the rooms within the bounds of the map.
            int length = Mathf.Min(Random.Range(minRoomWallSize, area / minRoomWallSize), mapLength - (int)start.x);
            // similar to the length, but we use the length to make sure we don't go over our area.
            int height = Mathf.Min(Random.Range(5, Mathf.Max(minRoomWallSize, area / length)), mapHeight - (int)start.y);

            newRoom = new Room(start, length, height);
        }


        return newRoom;
    }


    private void UpdateMap(List<Room> rooms, int mapLength, int mapHeight)
    {
        // used while in editor since the map isn't created in awake.
        if (mapLayout != null)
        {
            mapLayout.Clear();
        }
        else
        {
            mapLayout = new List<Vector2>();
        }


        foreach (Room room in rooms)
        {
            // find every vector2 in room that is not inside the map list and add it.
            mapLayout.AddRange(
                room.layout.FindAll(
                    item => !mapLayout.Contains(item)));
        }


        // Outline the map
        for (int x = 0; x <= mapLength; x++)
        {
            for (int y = 0; y <= mapHeight; y++)
            {
                if (x == mapLength || x == 0 || y == mapHeight || y == 0)
                {
                    Vector2 mapOutlineWall = new Vector2(x, y);
                    // if the mapLayout doesn't already contain the location we are looking at then add it (may be added from a room bordering the map bounds)
                    if (!mapLayout.Contains(mapOutlineWall))
                    {
                        mapLayout.Add(mapOutlineWall);
                    }
                }

            }
        }
    }
    private void DrawMap()
    {
        // Destroys the map GameObject that contains the quads to generate a new map.
        if (GameObject.Find("map"))
        {
#if UNITY_EDITOR
            DestroyImmediate(GameObject.Find("map"));
#else
            Destroy(GameObject.Find("map"));
#endif
        }
        GameObject mapContainer = new GameObject("map");

        // Create the quads at each location in the mapLayout and set the parent to the mapContainer.
        foreach (Vector2 location in mapLayout)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
            wall.transform.position = location;
            wall.transform.parent = mapContainer.transform;
        }
    }

    void CutRooms(List<Room> rooms, int mapLength, int mapHeight)
    {
        rooms.Sort();
        rooms.Reverse();
        for (int i = 1; i < rooms.Count; i++)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (rooms[i].Intersects(rooms[j]))
                {
                    rooms[i].CutFrom(rooms[j]);
                    UpdateMap(rooms, mapLength, mapHeight);
                    rooms[j].CalculateArea();
                }
            }
        }
    }

    void CutExits()
    {
        ///ToDo: Cut exit from opposite wall of exitDirection
        ///Match position of the current maps exit to the new maps exit.
        ///directionExit = 1 of 3 remaining exits
        ///cut exit from wall of exitDirection
    }

    internal class Room : System.IComparable<Room>
    {
        public Vector2 startingPosition { get; private set; }
        public int length { get; private set; }
        public int height { get; private set; }
        public int area;
        public List<Vector2> layout;

        //TileSet tileSet; Enum?

        public Room(Vector2 startingPosition, int length, int height)
        {
            this.startingPosition = startingPosition;
            this.length = length;
            this.height = height;

            area = (length - 2) * (height - 2) + 4;
        }

        public bool Intersects(Room b)
        {
            int count = 0;
            if (area > 0 || b.area > 0)
            {

                foreach (Vector2 location in layout)
                {
                    // if b and a contain the same location
                    if (b.layout.Contains(location))
                    {
                        // increment a counter
                        count++;
                    }
                }
                // if the counter is greater than 0 then they intersect.

            }
            return count > 0;
        }

        public void CutFrom(Room b)
        {
            ///ToDo: Cut the intersecting wall of this from section B

            // Remove all locations that are inside this room from room b.
            b.layout.RemoveAll(location => location.x >= startingPosition.x && location.x <= startingPosition.x + length && location.y >= startingPosition.y && location.y <= startingPosition.y + height);

            // Set the area that was removed inside b to the tileSet of this room.
        }

        /// <summary>
        /// Estimates remaining area
        /// </summary>
        public void CalculateArea()
        {
            if(layout.Count > 0 && length >0 && height > 0)
            {
                area = (layout.Count / (length * 2 + height * 2)) * ((length - 2) * (height - 2) + 4);
            }
            
        }

        public void Init()
        {
            //generating our layout based on the starting position.

            layout = new List<Vector2>();
            int xStop = length + (int)startingPosition.x;
            int yStop = height + (int)startingPosition.y;
            for (int x = (int)startingPosition.x; x <= xStop; x++)
            {
                for (int y = (int)startingPosition.y; y <= yStop; y++)
                {
                    if (x == startingPosition.x || x == xStop || y == startingPosition.y || y == yStop)
                    {
                        layout.Add(new Vector2(x, y));
                    }
                }
            }
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

}

static class UsefulShortcuts
{
    public static void ClearConsole()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

}