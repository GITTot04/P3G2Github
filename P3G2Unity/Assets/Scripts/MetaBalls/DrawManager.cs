using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawManager : MonoBehaviour
{

    public float drawZoneDistance;

    public float ballDensity;
    float instMetaBallClock = 0f;
    int framesWithoutDraw;
    bool isDrawing = false;


    public GameObject drawContainer;
    Container container;

    public int metaBallMaxCount = 50;
    public Vector3[] metaBallPositions;
    int metaBallArrayPosition;

    //For CheckInDrawZone();
    Vector3 lowestPosition;
    Vector3 highestPosition;
    private void Awake()
    {
        metaBallPositions = new Vector3[metaBallMaxCount];
        metaBallArrayPosition = 0;
    }
    void Start()
    {
        container = drawContainer.GetComponent<Container>();
    }


    void FixedUpdate()
    {
        if (isDrawing && framesWithoutDraw > 3)
        {
            Debug.Log("stopped drawing");
            InstantiateDrawing(true);
            isDrawing = false;
        }
        framesWithoutDraw++;

        drawZoneDistance = container.transform.localScale.x - container.edgeSize*2;
    }

    public void Draw(Vector3 position)
    {
        isDrawing = true;
        instMetaBallClock += Time.deltaTime;
        framesWithoutDraw = 0;


        if (!CheckInDrawZone(position))
        {
            Debug.Log("Out of drawzone");
            InstantiateDrawing(false);
        }
        if (metaBallArrayPosition >= metaBallMaxCount)
        {
            Debug.Log("MetaballPositons full");
            InstantiateDrawing(false);
        }
        if (metaBallArrayPosition== 0)
        {

            lowestPosition = position;
            highestPosition = position;
        }

        AddPosition(position);

    }
    
    void InstantiateDrawing (bool instantiateAsEndedDrawing)
    {
        container.InstantiateMetaBalls(metaBallPositions, lowestPosition);
        if (instantiateAsEndedDrawing)
        {
            metaBallArrayPosition = 0;
        }
        else
        {
            int keepingBallsCount = 5;
            for (int i = 0; i < keepingBallsCount; i++)
            {
                metaBallPositions[i] = metaBallPositions[metaBallArrayPosition + i - keepingBallsCount];
            }
            metaBallArrayPosition = keepingBallsCount;
            lowestPosition = metaBallPositions[0];
            highestPosition = metaBallPositions[0];
            for (int i = 1; i < keepingBallsCount; i++)
            {
                CheckInDrawZone(metaBallPositions[i]);
            }

        }
    }
    
    bool CheckInDrawZone (Vector3 newPosition)
    {
        if (metaBallArrayPosition == 0)
        {
            lowestPosition = newPosition;
            highestPosition = newPosition;
        } else {
            foreach (Vector3 position in metaBallPositions)
            {

                if (newPosition.x < lowestPosition.x)
                {
                    lowestPosition.x = newPosition.x;
                }
                else if (newPosition.x > highestPosition.x)
                {
                    highestPosition.x = newPosition.x;
                }
                if (newPosition.y < lowestPosition.y)
                {
                    lowestPosition.y = newPosition.y;
                }
                else if (newPosition.y > highestPosition.y)
                {
                    highestPosition.y = newPosition.y;
                }
                if (newPosition.z < lowestPosition.z)
                {
                    lowestPosition.z = newPosition.z;
                }
                else if (position.z > highestPosition.z)
                {
                    highestPosition.z = newPosition.z;
                }

                //Check distance across area
                if (highestPosition.x - lowestPosition.x > drawZoneDistance)
                {
                    return false;
                }
                else if (highestPosition.y - lowestPosition.y > drawZoneDistance)
                {
                    return false;
                }
                else if (highestPosition.z - lowestPosition.z > drawZoneDistance)
                {
                    return false;
                }
            }

        }
        return true;
    }
    void AddPosition(Vector3 position)
    {
        if (metaBallArrayPosition > 0)
        {
            if ((position - metaBallPositions[metaBallArrayPosition - 1]).magnitude > ballDensity)
            {
                metaBallPositions[metaBallArrayPosition] = position;
                metaBallArrayPosition++;
            }
            else return;
        } 
        else 
        {
            metaBallPositions[metaBallArrayPosition] = position;
            metaBallArrayPosition++;
        }
    }
}
