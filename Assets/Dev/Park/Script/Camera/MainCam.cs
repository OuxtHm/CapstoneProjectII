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
        Camera.main.orthographicSize = 9f;
    }

    public void CameraPosition()
    {
        if (stageManager.nowStage == 1)
        {
            Camera.main.orthographicSize = 9f;
            transform.position = new Vector3(0, 5, transform.position.z);
        }
        else if (stageManager.nowStage == 2)
        {
            Camera.main.orthographicSize = 8f;
            transform.position = new Vector3(0, 4, transform.position.z);
        }
        else if (stageManager.nowStage == 3)
        {
            Camera.main.orthographicSize = 9f;
            transform.position = new Vector3(0, 5, transform.position.z);
        }
    }
}
