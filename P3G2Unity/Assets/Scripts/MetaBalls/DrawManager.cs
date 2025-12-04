using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawManager : MonoBehaviour
{

    public float drawZoneDistance;

    public float ballDensity;
    int framesWithoutDraw;
    bool isDrawing = false;


    public GameObject drawContainer;
    Container container;

    public int metaBallMaxCount = 50;
    public Vector3[] metaBallPositions;
    int metaBallArrayPosition;
    public int keepingBallsCount = 5;
 
    //For CheckInDrawZone();
    Vector3 lowestPosition;
    Vector3 highestPosition;

    //For shader glittering:
    [Header("Shader glittering")]
    public float averageSpeedMin;
    public float averageSpeedMax;

    Vector3 lastPosition;
    float[] speedData = new float[400];
    int speedDataPosition = 0;
    float movementSpeedFactor = 0;
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

        drawZoneDistance = container.transform.localScale.x - container.edgeSize*2 - container.boxOffsetCompentsater;

        //Shader glittering
        Shader.SetGlobalFloat("_MovementSpeed", movementSpeedFactor);
    }

    public void Draw(Vector3 position)
    {
        AddSpeedDataPoint(position);
        isDrawing = true;
        framesWithoutDraw = 0;


        if (!CheckInDrawZone(position, metaBallArrayPosition))
        {
            Debug.Log("Out of drawzone");
            InstantiateDrawing(false);
        }
        if (metaBallArrayPosition >= metaBallMaxCount)
        {
            Debug.Log("MetaballPositons full");
            InstantiateDrawing(false);
        }
        if (metaBallArrayPosition == 0)
        {
            lowestPosition = position;
            highestPosition = position;
        }

        AddPosition(position);

    }
    
    void InstantiateDrawing (bool instantiateAsEndedDrawing)
    {
        container.ClearMetaBalls();
        if (instantiateAsEndedDrawing)
        {
            metaBallArrayPosition = 0;
        }
        else
        {
            for (int i = 0; i < keepingBallsCount; i++)
            {
                CheckInDrawZone(metaBallPositions[i], i);

                metaBallPositions[i] = metaBallPositions[metaBallArrayPosition + i - keepingBallsCount];
                container.AddMetaBall(metaBallPositions[i], lowestPosition);
            }
            metaBallArrayPosition = keepingBallsCount;
            
        }
    }
    
    bool CheckInDrawZone (Vector3 newPosition, int arrayPosition)
    {
        if (arrayPosition == 0)
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
            //Preventing metaballs being to close to each other;
            if ((position - metaBallPositions[metaBallArrayPosition - 1]).magnitude > ballDensity)
            {
                metaBallPositions[metaBallArrayPosition] = position;
                metaBallArrayPosition++;
                container.AddMetaBall(position, lowestPosition);
            }
            else return;
        } 
        else 
        {
            metaBallPositions[metaBallArrayPosition] = position;
            metaBallArrayPosition++;
            container.AddMetaBall(position, lowestPosition);
        }
    }

    void AddSpeedDataPoint(Vector3 position)
    {
        float newPoint = (lastPosition - position).magnitude;
        lastPosition = position;
        speedData[speedDataPosition] = newPoint; 
        if (speedDataPosition >= speedData.Count()-1) 
        {
            speedDataPosition = 0;
        }
        else
        {
            speedDataPosition++;
        }
      
        float averageSpeed = speedData.Average();
        movementSpeedFactor = (averageSpeed - averageSpeedMin) / (averageSpeedMax - averageSpeedMin);
        if (movementSpeedFactor > 1)
        {
            movementSpeedFactor = 1;
        } else if (movementSpeedFactor < 0) 
        {
            movementSpeedFactor = 0;
        }
    }
}
