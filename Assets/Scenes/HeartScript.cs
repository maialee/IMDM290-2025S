
// UMD IMDM290 
// Instructor: Myungin Lee
    // [a <-----------> b]
    // Lerp : Linearly interpolates between two points. 
    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    public float r;
    LineRenderer[] lineRenderer;
    static int numSphere = 200; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        lineRenderer = new LineRenderer[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            startPosition[i] = new Vector3( Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 0.5f), Random.Range(-0.5f, 0.5f));        
            // Heart shape end position
            t = i* 2 * Mathf.PI / numSphere;
            endPosition[i] = new Vector3( 
                        r *Mathf.Sqrt(2f) * Mathf.Sin(t) *  Mathf.Sin(t) *  Mathf.Sin(t),
                        r * (- Mathf.Cos(t) * Mathf.Cos(t) * Mathf.Cos(t) - Mathf.Cos(t) * Mathf.Cos(t) + 2 *Mathf.Cos(t)) + 3f,
                        10f + Mathf.Sin(time));
        }
        
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Position
            initPos[i] = startPosition[i];
        
        }
        // Line
        for (int i = 1; i < numSphere; i++){
            //For creating line renderer object
            lineRenderer[i] = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer[i].material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer[i].endColor = Color.HSVToRGB(Random.Range(0.8f, 1f), Random.Range(0.8f, 0.94f), Random.Range(3f, 5f));
            lineRenderer[i].startWidth = 0.3f;
            lineRenderer[i].endWidth = 0.4f;
            lineRenderer[i].positionCount = 2;
                            
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Measure Time 
        time += Time.deltaTime*4; // Time.deltaTime = The interval in seconds from the last frame to the current one
        // what to update over time?
        for (int i =1; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            // let it oscillate over time using sin function
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            lineRenderer[i].SetPosition(0, Vector3.Lerp(startPosition[i-1], endPosition[i-1], lerpFraction)); //x,y and z position of the starting point of the line
            lineRenderer[i].SetPosition(1, Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction)); //x,y and z position of the end point of the line
            int randindex = (int)Random.Range(1,numSphere);
            // lineRenderer[i].SetPosition(1, Vector3.Lerp(startPosition[randindex], endPosition[randindex], lerpFraction)); //x,y and z position of the end point of the line

        }
    }
}

