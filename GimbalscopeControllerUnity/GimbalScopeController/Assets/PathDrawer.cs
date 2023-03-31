using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class PathDrawer : MonoBehaviour
{

    public enum DrawMode{
        keyframes,
        stream
    }

    public DrawMode drawMode = DrawMode.keyframes;

    public List<string> filenames = new List<string>();
    public List<Color> colors = new List<Color>();
    public List<int> rotation_list = new List<int>();
    int rotation_list_index = 0;
    public Transform penHead;
    public GameObject endMarkerPrefab;
    public GameObject midMarkerPrefab;
    public float distance = 10f;

    public GameObject linePrefab;

    public Material normalTargetMat;
    public Material highlightedTargetMat;

    public Transform masterTransform;

    [Serializable]
    public struct CompletedIdentificaionTrialsWrapper
    {
        public CompletedTrial[] completedTrials;
    }
    [Serializable]
    public struct CompletedTrial
    {
        [SerializeField]
        public int trial_id;
        [SerializeField]
        public int trial_number;
        [SerializeField]
        public Vector3[] orientationKeyframes;
        [SerializeField]
        public Vector3[] orientationStream;
        [SerializeField]
        public int identification_target_angle;
        [SerializeField]
        public bool correct;
        [SerializeField]
        public int requestsCount;
    }

    void SetRendererAllTargets()
    {
        TargetScript[] targetScripts = FindObjectsOfType<TargetScript>();
        foreach(TargetScript t in targetScripts)
        {
            t.GetComponent<MeshRenderer>().material = normalTargetMat;
        }
    }
    void SetRendererSpecificTarget(int orientation)
    {
        TargetScript[] targetScripts = FindObjectsOfType<TargetScript>();
        foreach (TargetScript t in targetScripts)
        {
            if(t.orientation == orientation)
            {
                t.GetComponent<MeshRenderer>().material = highlightedTargetMat;
            }
           
        }
    }

    // Start is called before the first frame update

    public int angleToDisplay = 45;




    private void Update()
    {
        bool render = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            render = true;
            rotation_list_index -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            render = true;
            rotation_list_index += 1;
        }
        if(rotation_list_index < 0)
        {
            rotation_list_index = rotation_list.Count - 1;
        }
        if(rotation_list_index >= rotation_list.Count)
        {
            rotation_list_index = 0;
        }
        if (render)
        {
            int angle = rotation_list[rotation_list_index];
            RenderAngle(angle);
        }
    }

    private void Start()
    {
        RenderAngle(angleToDisplay);
    }
    void RenderAngle(int angle_display)
    {
        FindObjectOfType<Text>().text = angle_display.ToString();
        foreach (Transform child in masterTransform)
        {
            Destroy(child.gameObject);
        }
        // do all 45 degree ones
        SetRendererAllTargets();
        SetRendererSpecificTarget(angle_display);


        int i = 0;
        foreach (string filename in filenames)
        {
            if(drawMode == DrawMode.keyframes) DrawKeyframePath(filename, angle_display, colors[i]);
            if (drawMode == DrawMode.stream) DrawStreamPath(filename, angle_display, colors[i]);


            i++;
        }
        
    }

    void DrawStreamPath(string filename, int angle, Color col)
    {
        string jsonString = LoadJSONtoString(filename);
        CompletedIdentificaionTrialsWrapper completedIdentificaionTrialsWrapper = JsonUtility.FromJson<CompletedIdentificaionTrialsWrapper>(jsonString);
        CompletedTrial[] completedTrials = completedIdentificaionTrialsWrapper.completedTrials;
        // draw the first list of orientations


        // find the trial index for the angle
        int index = 0;
        foreach (CompletedTrial c in completedTrials)
        {
            if (c.identification_target_angle == angle)
            {
                break;
            }

            index++;
        }
        if (index > completedTrials.Length - 1)
        {
            Debug.Log("index > completedTrials.Length - 1 ");
        }

        Debug.Log(index);
        Vector3[] orientationStreamFreames = completedTrials[index].orientationStream;

        for (int i = 0; i < orientationStreamFreames.Length; i++)
        {
            if(Math.Abs(orientationStreamFreames[i].x) > 15)
            {
                orientationStreamFreames[i].x = orientationStreamFreames[i].x / 10;
            }
            
            //orientationStreamFreames[i].x = 0;
        }

        // foreach orientation keyframe
        List<Vector3> drawPoints = new List<Vector3>();
        GameObject lineInstance = Instantiate(linePrefab, penHead.position, Quaternion.identity);
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();
        Quaternion lastRotation = Quaternion.Euler(Vector3.forward);
        Vector3 lastPos = Vector3.zero;
        int c_i = 0;
        foreach (Vector3 v in orientationStreamFreames)
        {
            Vector3 orientation = v;
            orientation.x += UnityEngine.Random.Range(-2, 2);
            Debug.Log(orientation);
            Quaternion nextKeyframe = Quaternion.Euler(orientation);
            
            // interpolate between start Quaternion and end Quaternion with timestep. add drawpoint
            int maxSamples = 10;
            for (int j = 0; j < maxSamples; j++)
            {
                float t = (float)j / (float)maxSamples;
                Quaternion rot = Quaternion.Slerp(lastRotation, nextKeyframe, t);
                penHead.rotation = rot;
                Vector3 drawPoint = penHead.transform.forward * distance;
                drawPoints.Add(drawPoint);
                lastPos = drawPoint;
            }

            penHead.rotation = nextKeyframe;


            c_i += 1;


            lastRotation = penHead.rotation;


        }

        lineRenderer.positionCount = drawPoints.Count;
        lineRenderer.SetPositions(drawPoints.ToArray());
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = col;
        GameObject endMarkerInstance = Instantiate(endMarkerPrefab, lastPos, Quaternion.identity);
        endMarkerInstance.transform.LookAt(penHead.transform.position);
        Material material = endMarkerInstance.GetComponentInChildren<MeshRenderer>().material;
        material.color = col;
        MeshRenderer[] meshRenderers = endMarkerInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = material;
        }
        endMarkerInstance.transform.parent = masterTransform;
        lineInstance.transform.parent = masterTransform;
    }


    void DrawKeyframePath(string filename,  int angle, Color col)
    {
        string jsonString = LoadJSONtoString(filename);
        CompletedIdentificaionTrialsWrapper completedIdentificaionTrialsWrapper = JsonUtility.FromJson<CompletedIdentificaionTrialsWrapper>(jsonString);
        CompletedTrial[] completedTrials = completedIdentificaionTrialsWrapper.completedTrials;
        // draw the first list of orientations


        // find the trial index for the angle
        int index = 0;
        foreach(CompletedTrial c in completedTrials)
        {
            if (c.identification_target_angle == angle)
            {
                break;
            }

            index++;
        }
        if(index > completedTrials.Length - 1)
        {
            Debug.Log("index > completedTrials.Length - 1 ");
        }

        Debug.Log(index);
        Vector3[] orientationKeyframes = completedTrials[index].orientationKeyframes;

        for (int i = 0; i < orientationKeyframes.Length; i++)
        {
            // orientationKeyframes[i].x = orientationKeyframes[i].x / 10;
            orientationKeyframes[i].x = 0;
        }
        
        // foreach orientation keyframe
        List<Vector3> drawPoints = new List<Vector3>();
        GameObject lineInstance = Instantiate(linePrefab, penHead.position, Quaternion.identity);
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();
        Quaternion lastRotation = Quaternion.Euler(Vector3.forward);
        Vector3 lastPos = Vector3.zero;
        int c_i = 0;
        foreach (Vector3 v in orientationKeyframes)
        {
            Vector3 orientation = v;
            orientation.x += UnityEngine.Random.Range(-5, 5);
            Debug.Log(orientation);
            Quaternion nextKeyframe = Quaternion.Euler(orientation);
            /*
            // interpolate between start Quaternion and end Quaternion with timestep. add drawpoint
            int maxSamples = 3;
            for (int j = 0; j < maxSamples; j++)
            {
                float t = (float)j / (float)maxSamples;
                Quaternion rot = Quaternion.Slerp(lastRotation, nextKeyframe, t);
                penHead.rotation = rot;
                Vector3 drawPoint = penHead.transform.forward * distance;
                drawPoints.Add(drawPoint);
                lastPos = drawPoint;
            }
            */
            penHead.rotation = Quaternion.Euler(orientation);
            Vector3 drawPoint = penHead.transform.forward * distance;
            drawPoints.Add(drawPoint);
            lastPos = drawPoint;

            // place a normal marker at lastpos
            if (c_i != orientationKeyframes.Length - 1)
            {
                GameObject midMarkerInstance = Instantiate(midMarkerPrefab, lastPos, Quaternion.identity);
                midMarkerInstance.transform.LookAt(penHead.transform.position);
                midMarkerInstance.transform.parent = masterTransform;
                Material m_material = midMarkerInstance.GetComponentInChildren<MeshRenderer>().material;
                m_material.color = Color.Lerp(Color.white, col, (float)c_i / (float)(orientationKeyframes.Length));
                MeshRenderer[] m_meshRenderers = midMarkerInstance.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mr in m_meshRenderers)
                {
                    mr.material = m_material;
                }
            }
            c_i += 1;


            lastRotation = penHead.rotation;


        }
        lineRenderer.positionCount = drawPoints.Count;
        lineRenderer.SetPositions(drawPoints.ToArray());
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = col;
        GameObject endMarkerInstance = Instantiate(endMarkerPrefab, lastPos, Quaternion.identity);
        endMarkerInstance.transform.LookAt(penHead.transform.position);
        Material material = endMarkerInstance.GetComponentInChildren<MeshRenderer>().material;
        material.color = col;
        MeshRenderer[] meshRenderers = endMarkerInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = material;
        }
        endMarkerInstance.transform.parent = masterTransform;
        lineInstance.transform.parent = masterTransform;
    }

    // Update is called once per frame


    string LoadJSONtoString(string filename)
    {
        string path = "D:\\Repos\\Gimbalscope\\Gimbalscope\\GimbalscopeControllerUnity\\GimbalScopeController\\Assets\\identification_test_results_dir\\" + filename + ".json";
        string json_string = "";
        using (StreamReader r = new StreamReader(path))
        {
             json_string = r.ReadToEnd();
        }
        return json_string;
    }
}
