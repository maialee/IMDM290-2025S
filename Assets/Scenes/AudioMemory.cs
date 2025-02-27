// UMD IMDM290 
// Instructor: Myungin Lee
// All the same Lerp but using audio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMemory : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 200; 
    float relativeTime = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;
    GameObject mother;
    float realTime =0f;
    float[] arrayAmp;
    static int lengthofPower = numSphere;
    private float previousAmp;
    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        arrayAmp = new float[lengthofPower];
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere];
        mother = GameObject.Find("leaf");
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        

            r = 3f; // radius of the circle
            // Circular end position
            endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere));
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            //spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i] = Instantiate(mother);

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];
            // spheres[i].transform.localRotation = Quaternion.EulerAngles(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            spheres[i].transform.localScale = new Vector3(Random.Range(0.2f, 0.3f), Random.Range(0.2f, 0.3f), Random.Range(0.2f, 0.3f));
            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ***Here, we use audio Amplitude, where else do you want to use?
        arrayAmp[0] = AudioSpectrum.audioAmp;
        arrayAmp[1] = previousAmp;     

        // Save amplitude value as a array
        for (int i = lengthofPower-1; i > 0 ; i--){
            arrayAmp[i] = arrayAmp[i-1];           
        }

        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?
        relativeTime += Time.deltaTime * AudioSpectrum.audioAmp; 
        realTime += Time.deltaTime;
        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(relativeTime) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numSphere;
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = 1f + arrayAmp[i];
            spheres[i].transform.localScale = new Vector3(scale, 1f, 1f);
            spheres[i].transform.Rotate(arrayAmp[i], 1f, 1f);
            
            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(relativeTime)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(relativeTime)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
        previousAmp = AudioSpectrum.audioAmp;

    }
}
