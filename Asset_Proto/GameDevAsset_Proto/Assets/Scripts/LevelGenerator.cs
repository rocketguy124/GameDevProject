using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;

    public Color startColor, endColor;
    //public GameObject startRoom;
   // public GameObject endRoom;

    public int distanceToExit;

    public Transform generationPoint;

    public enum Direction { up, right, down, left }; //Clockwise direcitons
    public Direction selectedDirection;

    public float xOffset = 22f;
    public float yOffset = 13f;

    public LayerMask roomLayout;

    private GameObject endRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenPoint();

        for(int i = 0; i < distanceToExit; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if(i + 1 == distanceToExit)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenPoint();

            while(Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayout))
            {
                MoveGenPoint(); //On overlap, moves same direction until no overlap (Removes chance of infinite moving between up and down / left and right)
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
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
}
