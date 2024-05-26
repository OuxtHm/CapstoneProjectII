using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Player player;
    public Camera camera;
    public bool ismove = false;
    public float Dir;

    private void Awake()
    {
        camera = GameObject.FindObjectOfType<Camera>();
    }


    void OnTriggerEnter2D(Collider2D collision)  //충돌 대미지 주기
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                ismove = true;
            }
            else
                Debug.Log("플레이어를 못 불러옴");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Dir = this.transform.position.x - player.transform.position.x;
                if(ismove)
                {
                    if (this.tag == "CameraDown")
                        camera.transform.position = new Vector3(camera.transform.position.x, -2, camera.transform.position.z);
                    else if(this.tag == "CameraUp")
                        camera.transform.position = new Vector3(camera.transform.position.x, 4, camera.transform.position.z);
                    MovingCamera();
                }
                
            }
            else
                Debug.Log("플레이어를 못 불러옴");
        }
    }


    IEnumerator MoveCameraToNextPos(float targetX)
    {
        float moveSpeed = 18; // 카메라 이동속도
        float duration = Mathf.Abs((targetX - camera.transform.position.x) / moveSpeed) * Time.deltaTime;
        Vector3 targetPosition = new Vector3(targetX, camera.transform.position.y, camera.transform.position.z);

        while (Vector3.Distance(camera.transform.position, targetPosition) > 0.01f)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        ismove = false;
    }

    void MovingCamera()
    {
        float moveDistance = 24;    //카메라 사이즈에 맞게 설정한 이동거리임
        float nextPos = camera.transform.position.x + (Dir < 0 ? moveDistance : -moveDistance);

        StartCoroutine(MoveCameraToNextPos(nextPos));
    }

}
