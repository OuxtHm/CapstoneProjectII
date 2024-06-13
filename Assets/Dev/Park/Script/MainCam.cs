using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    StageManager stageManager;
    public static MainCam instance;

    private void Start()
    {
        instance = this;
        stageManager = StageManager.instance.GetComponent<StageManager>();
        CameraPosition();
    }

    public void CameraPosition()
    {
        if (stageManager.nowStage == 2)
        {
            Camera.main.orthographicSize = 8f;
            Camera.main.transform.localPosition = new Vector3(0, 4, Camera.main.transform.localPosition.z);
        }
        else
        {
            Camera.main.orthographicSize = 9f;
            Camera.main.transform.localPosition = new Vector3(0, 5, Camera.main.transform.localPosition.z);
        }
    }
}
