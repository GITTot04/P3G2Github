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
    int framesWithoutDraw; //Keeps track of how many frames went since last draw input. (Should be 0 or 1 when drawing)
    bool isDrawing = false;


    public GameObject drawContainer;
    Container container;

    public int metaBallMaxCount = 50;
    public Vector3[] metaBallPositions;
    int metaBallArrayPosition;
    public int keepingBallsCount = 5; //Number of positions kept to 'make sausages overlap'.
 
    //For CheckInDrawZone();
    Vector3 lowestPosition;
    Vector3 highestPosition;

    //For shader glittering:
    [Header("Shader glittering")]
    public float averageSpeedMin;
    public float averageSpeedMax;

    Vector3 lastPosition;
    float[] speedData = new float[200];
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
        if (isDrawing && framesWithoutDraw > 3) //Then user is drawing but now stopped drawing
        {
            Debug.Log("stopped drawing");
            InstantiateDrawing(true); //Calls 'instantiateAsEndedDrawing=true', because user stopped drawing and does not need 'anti-sausaging'
            isDrawing = false;
        }
        framesWithoutDraw++;

        drawZoneDistance = container.transform.localScale.x - container.edgeSize*2 - container.boxOffsetCompentsater;

        //Shader glittering
        Shader.SetGlobalFloat("_MovementSpeed", movementSpeedFactor);
    }

    public void Draw(Vector3 position) //Method called when user adds draw position
    {
        AddSpeedDataPoint(position);
        isDrawing = true;
        framesWithoutDraw = 0; //Reset frames since last draw input


        if (!CheckInDrawZone(position, metaBallArrayPosition)) //New position out of drawzone.
        {
            Debug.Log("Out of drawzone");
            InstantiateDrawing(false);
        }
        if (metaBallArrayPosition >= metaBallMaxCount) //Array full!
        {
            Debug.Log("MetaballPositons full");
            InstantiateDrawing(false);
        }
        if (metaBallArrayPosition == 0) //IN case array empty
        {
            lowestPosition = position;
            highestPosition = position;
        }
        //Not matter what, at last add position.
        AddPosition(position);

    }
    
    void InstantiateDrawing (bool instantiateAsEndedDrawing) //Clears metaballs and makes 'new' drawing
    {
        container.ClearMetaBalls();
        if (instantiateAsEndedDrawing) //Does not keep position overlay
        {
            metaBallArrayPosition = 0;
        }
        else //Keeps position overlay for 'keeping sausages together'
        {
            for (int i = 0; i < keepingBallsCount; i++) //Sets lowest values of array to highest for 'anti-sausaging'
            {
                CheckInDrawZone(metaBallPositions[i], i);

                metaBallPositions[i] = metaBallPositions[metaBallArrayPosition + i - keepingBallsCount];
                container.AddMetaBall(metaBallPositions[i], lowestPosition);
            }
            metaBallArrayPosition = keepingBallsCount; //then reset array position
            
        }
    }
    
    bool CheckInDrawZone (Vector3 newPosition, int arrayPosition) //Keeps track wether new postion is out of the drawzone.
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
                container.AddMetaBall(position, lowestPosition); //Adds actual drawing
            }
            else return; //In case metaball position is too close to former position, no position is added.
        } 
        else 
        {
            metaBallPositions[metaBallArrayPosition] = position;
            metaBallArrayPosition++;
            container.AddMetaBall(position, lowestPosition); //Adds actual drawing
        }
    }

    void AddSpeedDataPoint(Vector3 position) //Method for shader to detect speed of user motions
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
