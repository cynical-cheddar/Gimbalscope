using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveExtractor : MonoBehaviour
{
    public AnimationCurve xs;
    public AnimationCurve ys;
    public AnimationCurve zs;

    public int samples = 2000;



    public List<Vector3> orientationFrames = new List<Vector3>();

    public string xsString = "xs = [";

    public string ysString = "ys = [";

    public string zsString = "zs = [";
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    void Generate()
    {

        xsString = "xs = [";
        ysString = "ys = [";
        zsString = "zs = [";


        for (int i = 0; i < samples; i++)
        {
            float t = Mathf.Lerp(0, 1, (float)i / samples);

            float x = xs.Evaluate(t);
            float y = ys.Evaluate(t);
            float z = zs.Evaluate(t);

            orientationFrames.Add(new Vector3(x, y, z));
        }


        foreach (Vector3 orientation in orientationFrames)
        {
            xsString = xsString + orientation.x.ToString() + ",";

            ysString = ysString + orientation.y.ToString() + ",";

            zsString = zsString + "0.0" + ",";
        }

        xsString = xsString + "]";

        ysString = ysString + "]";

        zsString = zsString + "]";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }
}
