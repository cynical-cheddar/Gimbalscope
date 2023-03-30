using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class PathDrawer : MonoBehaviour
{

    public List<string> filenames = new List<string>();


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
    void Start()
    {
        DrawPathFromFile(filenames[0], 0);
    }

    void DrawPathFromFile(string filename,  int index)
    {
        string jsonString = LoadJSONtoString(filename);
        CompletedIdentificaionTrialsWrapper completedIdentificaionTrialsWrapper = JsonUtility.FromJson<CompletedIdentificaionTrialsWrapper>(jsonString);
        CompletedTrial[] completedTrials = completedIdentificaionTrialsWrapper.completedTrials;
        // draw the first list of orientations
        Vector3[] orientationKeyframes = completedTrials[0].orientationKeyframes;
        for (int i = 0; i < orientationKeyframes.Length; i++)
        {
            orientationKeyframes[i].x = orientationKeyframes[i].x / 10;
        }
        SetRendererAllTargets();
        SetRendererSpecificTarget(completedTrials[0].identification_target_angle);
        // foreach orientation keyframe
        List<Vector3> drawPoints = new List<Vector3>();
        GameObject lineInstance = Instantiate(linePrefab, penHead.position, Quaternion.identity);
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();
        Quaternion lastRotation = Quaternion.Euler(Vector3.forward);
        Vector3 lastPos = Vector3.zero;
        int c_i = 0;
        foreach (Vector3 orientation in orientationKeyframes)
        {
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
                m_material.color = Color.Lerp(Color.white, Color.red, (float)c_i / (float)(orientationKeyframes.Length));
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
        lineRenderer.endColor = Color.red;
        GameObject endMarkerInstance = Instantiate(endMarkerPrefab, lastPos, Quaternion.identity);
        endMarkerInstance.transform.LookAt(penHead.transform.position);
        Material material = endMarkerInstance.GetComponentInChildren<MeshRenderer>().material;
        material.color = Color.red;
        MeshRenderer[] meshRenderers = endMarkerInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = material;
        }
        endMarkerInstance.transform.parent = masterTransform;
        lineInstance.transform.parent = masterTransform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
