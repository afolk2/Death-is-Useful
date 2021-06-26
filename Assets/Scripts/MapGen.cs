using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    #region Old
    //// Start is called before the first frame update
    //[SerializeField] private Grid _grid;
    //[SerializeField] private Tilemap _currentTileMap;
    //[SerializeField] private Tilemap _nextTileMap;

    //[SerializeField] private int _minroomSize;
    //[SerializeField] private int _maxroomSize;

    //[SerializeField] private int _smallestSectionArea;

    //[SerializeField] private int _minSmallSections;
    //[SerializeField] private int _maxSmallSections;

    //[SerializeField] private int _minMediumSections;
    //[SerializeField] private int _maxMediumSections;

    //[SerializeField] private int _minLargeSections;
    //[SerializeField] private int _maxLargeSections;

    //private bool[,] _roomLayout;

    //private void Awake()
    //{
    //    GenerateRoom();

    //    GameObject room = new GameObject("Room");

    //    for (int h = 0; h < _roomLayout.GetLength(0); h++)
    //    {
    //        for (int w = 0; w < _roomLayout.GetLength(1); w++)
    //        {
    //            if (h == 0 || h == _roomLayout.GetLength(0) - 1)
    //            {
    //                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                go.transform.position = new Vector3(w, h, 0);
    //                go.transform.parent = room.transform;
    //            }
    //            else if (w == 0 || w == _roomLayout.GetLength(1) - 1)
    //            {
    //                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                go.transform.position = new Vector3(w, h, 0);
    //                go.transform.parent = room.transform;
    //            }
    //        }
    //    }
    //}
    //void Start()
    //{

    //}

    //void GenerateRoom()
    //{
    //    _roomLayout = new bool[Random.Range(_minroomSize, _maxroomSize), Random.Range(_minroomSize, _maxroomSize)];
    //    int roomHeight = _roomLayout.GetLength(0);
    //    int roomWidth = _roomLayout.GetLength(1);

    //    int roomArea = roomHeight * roomWidth;
    //    _smallestSectionArea = (int)(roomArea * (1 / 16));

    //    int maxSmallSize = roomArea / 4;
    //    int maxMediumSize = roomArea / 2;
    //    int maxLargeSize = (int)(roomArea / 1.5f);

    //    int numSmallSections = Random.Range(_minSmallSections, _maxSmallSections);
    //    int numMediumSections = Random.Range(_minMediumSections, _maxMediumSections);
    //    int numLargeSections = Random.Range(_minLargeSections, _maxLargeSections);

    //    bool[,] sectionLayout;
    //    int sectionArea;

    //    for (int i = 0; i < numSmallSections; i++)
    //    {
    //        GameObject section = new GameObject();
    //        section.name = "Small " + i;
    //        // pick an index to start the section from limited by 3 quarters of the room (don't want a section that is too small)
    //        int startIndexA = Random.Range(0, (int)(roomHeight * 0.75));
    //        int startIndexB = Random.Range(0, (int)(roomWidth * 0.75));

    //        // determine the area of the section
    //        sectionArea = GetSectionArea(roomHeight, roomWidth, maxSmallSize, startIndexA, startIndexB);
    //        // determine the length and width of the section
    //        sectionLayout = GetSectionLayout(sectionArea, roomHeight, roomWidth, startIndexA, startIndexB);

    //        // Test section size

    //        for (int h = 0; h < sectionLayout.GetLength(0); h++)
    //        {
    //            for (int w = 0; w < sectionLayout.GetLength(1); w++)
    //            {
    //                if (sectionLayout[h, w])
    //                {
    //                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                    go.transform.position = new Vector3(w, h, 0);
    //                    go.transform.parent = section.transform;
    //                }
    //            }
    //        }
    //    }

    //    for (int i = 0; i < numMediumSections; i++)
    //    {
    //        GameObject section = new GameObject();
    //        section.name = "Medium " + i;

    //        // pick an index to start the section from limited by 3 quarters of the room (don't want a section that is too small)
    //        int startIndexA = Random.Range(0, (int)(roomHeight * 0.66));
    //        int startIndexB = Random.Range(0, (int)(roomWidth * 0.66));

    //        // determine the area of the section
    //        sectionArea = GetSectionArea(roomHeight, roomWidth, maxMediumSize, startIndexA, startIndexB);
    //        // determine the length and width of the section
    //        sectionLayout = GetSectionLayout(sectionArea, roomHeight, roomWidth, startIndexA, startIndexB);

    //        // Test section size

    //        for (int h = 0; h < sectionLayout.GetLength(0); h++)
    //        {
    //            for (int w = 0; w < sectionLayout.GetLength(1); w++)
    //            {
    //                if (sectionLayout[h, w])
    //                {
    //                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                    go.transform.position = new Vector3(w, h, 0);
    //                    go.transform.parent = section.transform;
    //                }
    //            }
    //        }
    //    }
    //}

    //private int GetSectionArea(int roomHeight, int roomWidth, int maxSectionArea, int startHeight, int startWidth)
    //{
    //    int maxArea = Mathf.Min(maxSectionArea, (roomHeight - startHeight) * (roomWidth - startWidth));
    //    int sectionArea;

    //    if (_smallestSectionArea < maxArea)
    //    {
    //        sectionArea = Random.Range(_smallestSectionArea, maxArea);
    //    }
    //    else
    //    {
    //        sectionArea = _smallestSectionArea;
    //    }

    //    return sectionArea;
    //}

    //private bool[,] GetSectionLayout(int sectionArea, int roomHeight, int roomWidth, int startHeight, int startWidth)
    //{
    //    bool[,] sectionLayout = new bool[roomHeight, roomWidth];

    //    int minHeight = (int)(roomHeight * 0.2);
    //    int minWidth = (int)(roomWidth * 0.2);

    //    int sectionHeight = Random.Range(minHeight, roomHeight - startHeight);
    //    int sectionWidth = Random.Range(minWidth, Mathf.Min(sectionArea / sectionHeight, roomWidth - startWidth));

    //    for (int h = startHeight; h < startHeight + sectionHeight; h++)
    //    {
    //        for (int w = startWidth; w < startWidth + sectionWidth; w++)
    //        {
    //            if ((h == startHeight || h == startHeight + sectionHeight - 1) || (w == startWidth || w == startWidth + sectionWidth - 1))
    //            {
    //                sectionLayout[h, w] = true;
    //            }
    //        }
    //    }

    //    return sectionLayout;
    //}
    #endregion

    #region New
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

    List<Vector2> map;


    private void Awake()
    {
        map = new List<Vector2>();
        GenerateMap();
    }

    public void GenerateMap()
    {
        int length = Random.Range(minMapSize, maxMapSize);
        int width = Random.Range(minMapSize, maxMapSize);
        int area = length * width;

        GenerateRooms(length, width, area);

        //CutExits();
    }

    void GenerateRooms(int mapLength, int mapHeight, int mapArea)
    {
        int minRoomArea = (int)(minRoomAreaPercent * mapArea);

        int maxSmallArea = (int)(mapArea * maxSmallSizePercent);
        int maxMediumArea = (int)(mapArea * maxMediumSizePercent);
        int maxLargeArea = (int)(mapArea * maxLargeSizePercent);

        UsefulShortcuts.ClearConsole();
        int numSmall = Random.Range(mapArea / maxSmallArea, mapArea / minRoomArea);
        Debug.Log($"numSmall = Random.Range({mapArea / maxSmallArea},{mapArea / minRoomArea}) = {numSmall}");

        int numMedium = Random.Range(mapArea / maxMediumArea, mapArea / maxSmallArea);
        Debug.Log($"numMedium = Random.Range({mapArea / maxMediumArea},{mapArea / maxSmallArea}) = {numMedium}");

        int numLarge = Random.Range(mapArea / maxLargeArea, mapArea / maxMediumArea);
        Debug.Log($"numSmall = Random.Range({mapArea / maxLargeArea},{mapArea / maxMediumArea}) = {numLarge}");


        Room[] rooms = new Room[numSmall + numMedium + numLarge];

        for (int i = 0; i < numSmall; i++)
        {
            rooms[i] = MakeRoom(mapLength, mapHeight, minRoomArea, maxSmallArea);

            rooms[i].Init();
        }
        for (int i = numSmall; i < numSmall + numMedium; i++)
        {
            rooms[i] = MakeRoom(mapLength, mapHeight, maxSmallArea, maxMediumArea);

            rooms[i].Init();
        }
        for (int i = numMedium; i < numSmall + numMedium + numLarge; i++)
        {
            rooms[i] = MakeRoom(mapLength, mapHeight, maxMediumArea, maxLargeArea);

            rooms[i].Init();
        }

        CutRooms(rooms, mapLength, mapHeight);

        DrawMap();
    }

    Room MakeRoom(int mapLength, int mapHeight, int minRoomArea, int maxRoomArea)
    {

        int area = Random.Range(minRoomArea, maxRoomArea);

        Vector2 start = new Vector2(
            Random.Range(
                0, (int)(mapLength * 0.8f)),
            Random.Range(
                0, (int)(mapHeight * 0.8f)));

        int length = Mathf.Min(Random.Range(5, area / 5), mapLength - (int)start.x);
        int height = Mathf.Min(Random.Range(5, Mathf.Max(5, area / length)), mapHeight - (int)start.y);

        return new Room(start, length, height);
    }


    private void UpdateMap(Room[] rooms, int mapLength, int mapHeight)
    {
        if (map != null)
        {
            map.Clear();
        }


        foreach (Room room in rooms)
        {
            // find every vector2 in room that is not inside the map list and add it.
            map.AddRange(
                room.layout.FindAll(
                    item => !map.Contains(item)));
        }


        for (int x = 0; x <= mapLength; x++)
        {
            for (int y = 0; y <= mapHeight; y++)
            {
                if (x == mapLength || x == 0 || y == mapHeight || y == 0)
                {
                    Vector2 mapOutlineWall = new Vector2(x, y);
                    if (!map.Contains(mapOutlineWall))
                    {
                        map.Add(mapOutlineWall);
                    }
                }

            }
        }
    }
    private void DrawMap()
    {
        if (GameObject.Find("map"))
        {
#if UNITY_EDITOR
            DestroyImmediate(GameObject.Find("map"));
#else
            Destroy(GameObject.Find("map"));
#endif
        }
        GameObject mapContainer = new GameObject("map");


        foreach (Vector2 location in map)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
            wall.transform.position = location;
            wall.transform.parent = mapContainer.transform;
        }
    }

    void CutRooms(Room[] rooms, int mapLength, int mapHeight)
    {
        for (int a = 0; a < rooms.Length - 1; a++)
        {
            for (int b = a + 1; b < rooms.Length; b++)
            {
                if (rooms[a].Intersects(rooms[b]))
                {
                    Room small = rooms[a];
                    Room large = rooms[b];
                    // cut the larger room
                    if (rooms[a].area > rooms[b].area)
                    {
                        small = rooms[b];
                        large = rooms[a];
                    }
                    else if (rooms[b].area > rooms[a].area)
                    {
                        small = rooms[a];
                        large = rooms[b];
                    }
                    else
                    {
                        if (Random.Range(0, 1) == 0)
                        {
                            small = rooms[b];
                            large = rooms[a];
                        }
                    }
                    small.CutFrom(large);
                    UpdateMap(rooms, mapLength, mapHeight);
                    large.CalculateArea(map);
                    if (large.area < 10)
                    {
                        large.layout.Clear();
                    }
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

    internal class Room
    {
        public Vector2 startingPosition { get; private set; }
        int length, height;
        public int area;
        public List<Vector2> layout;

        public Room(Vector2 startingPosition, int length, int height)
        {
            this.startingPosition = startingPosition;
            this.length = length;
            this.height = height;

            area = length * height;
        }

        public bool Intersects(Room b)
        {
            int count = 0;
            foreach (Vector2 location in layout)
            {
                if (b.layout.Contains(location))
                {
                    count++;
                }
            }
            return count > 0;
        }

        public void CutFrom(Room b)
        {
            ///ToDo: Cut the intersecting wall of this from section B

            // Removes all tiles that fall inside a from b
            b.layout.RemoveAll(location => location.x > startingPosition.x && location.x < startingPosition.x + length && location.y > startingPosition.y && location.y < startingPosition.y + height);

            // Add the walls from this room that are inside room b.
            b.layout.AddRange(layout.FindAll(item =>
                (item.x == startingPosition.x || item.x == startingPosition.x + length) && (item.y == startingPosition.y || item.y == startingPosition.y + height) &&
                (item.x > b.startingPosition.x && item.x < b.startingPosition.x + length && item.y > b.startingPosition.y && item.y < b.startingPosition.y + height)
                ));


            /*
            ///ToDo: Cut the intersecting wall of this from section
            float largeX = layout.Max(item =>item.x);
            float smallX = layout.Min(item =>item.x);
            float largeY = layout.Max(item => item.y);
            float smallY = layout.Min(item => item.y);


            float largeBX = b.layout.Max(item => item.x);
            float smallBX = b.layout.Min(item => item.x);
            float largeBY = b.layout.Max(item => item.y);
            float smallBY = b.layout.Min(item => item.y);


            // Removes all tiles that fall inside a from b
            b.layout.RemoveAll(location => location.x >= smallX && location.x <= largeX && location.y >= smallY && location.y <= largeY);

            // Add the walls from this room that are inside room b.
            b.layout.AddRange(layout.FindAll(item =>
                (item.x == smallX || item.x == largeX) && (item.y == smallY || item.y == largeY) &&
                (item.x >= smallBX && item.x <= largeBX && item.y >= smallBY && item.y <= largeBY)
                ));
             */
        }

        public void CalculateArea(List<Vector2> searchArea)
        {
            area = length * height;

            for (int x = (int)startingPosition.x + 1; x < startingPosition.x + length; x++)
            {
                for (int y = (int)startingPosition.y + 1; y < startingPosition.y + height; y++)
                {
                    if (searchArea.Contains(new Vector2(x, y)))
                    {
                        area--;
                    }

                }
            }
        }

        public void Init()
        {
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
    }


#endregion
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