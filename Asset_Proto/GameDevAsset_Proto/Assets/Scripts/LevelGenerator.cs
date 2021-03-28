using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;

    public Color startColor, endColor, shopColor;

    public int distanceToExit;
    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;


    public Transform generationPoint;

    public enum Direction { up, right, down, left }; //Clockwise direcitons
    public Direction selectedDirection;

    public float xOffset = 22f;
    public float yOffset = 12f;

    public LayerMask roomLayout;

    private GameObject endRoom, shopRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop;
    public RoomCenter[] potentialCenters;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenPoint();

        for (int i = 0; i < distanceToExit; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToExit)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenPoint();

            while (Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayout))
            {
                MoveGenPoint(); //On overlap, moves same direction until no overlap (Removes chance of infinite moving between up and down / left and right)
            }
        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        //creating Outlines
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;
            if (outline.transform.position == Vector3.zero) //Starting Room Center Creation
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)//Ending Room Center Creation
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (includeShop)//Shop Room Center Creation
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (generateCenter)//All other room generation
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
#endif
    public void MoveGenPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generationPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.right:
                generationPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.down:
                generationPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.left:
                generationPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;

        }
    }
    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, roomLayout);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, roomLayout);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, roomLayout);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, roomLayout);

        int directionCount = 0;//Counts the number of adjacent rooms to minimized bool checks to create the outlines
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("No Rooms exist");
                break;

            case 1:
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.SingleDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;

            case 2:

                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                if (roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                break;

            case 3:

                if (roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpDownLeft, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightLeft, roomPosition, transform.rotation));
                }
                break;

            case 4:

                if (roomLeft && roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.Quad, roomPosition, transform.rotation));
                }
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, SingleDown, singleRight, singleLeft,
        doubleLeftRight, doubleUpDown, doubleUpRight, doubleRightDown, doubleLeftDown, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleUpDownLeft, tripleUpRightLeft,
        Quad;
}
