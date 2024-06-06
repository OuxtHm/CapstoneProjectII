using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Player player;
    Camera mainCamera;
    public bool isMove = false;    //ī�޶� �̵� ���� Ȯ��
    float directionX;    // �÷��̾ ������ X����
    float directionY;    // �÷��̾ ������ Y����
    public int moveCount = 0;   //ī�޶� �� �̻� �̵��� �� ���� ��ġ���� Ȯ���ϴ� ���� 

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Dash")))
        {
            player = collision.GetComponent<Player>();
            if (player != null && !isMove)
            {
                isMove = true;
            }
            else
            {
                Debug.Log("�÷��̾ �� �ҷ���");
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Dash")))
        {
            player = collision.GetComponent<Player>();
            if (player != null && !isMove)
            {
                isMove = true;
            }
            else
            {
                Debug.Log("�÷��̾ �� �ҷ���");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Dash")))
        {
            player = collision.GetComponent<Player>();
            if (player != null)
            {
                directionX = transform.position.x - player.transform.position.x;
                if (isMove)
                {
                    float UpSize = 8f;
                    float DownSize = 5f;
                    if (tag == "CameraDown")
                    {
                        if(directionX < 0)
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - DownSize, mainCamera.transform.position.z);
                        else
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + DownSize, mainCamera.transform.position.z);

                    }
                    else if (tag == "CameraUp")
                    {
                        if (directionX < 0)
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + UpSize, mainCamera.transform.position.z);
                        else
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - UpSize, mainCamera.transform.position.z);
                    }
                    MovingCamera();
                }
            }
            else
            {
                Debug.Log("�÷��̾ �� �ҷ���");
            }
        }
    }

    IEnumerator MoveCameraToNextPosX(float targetX)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        float moveSpeed = 50;
        float duration = Mathf.Abs((targetX - mainCamera.transform.position.x) / moveSpeed) * Time.deltaTime;
        Vector3 targetPositionX = new Vector3(targetX, mainCamera.transform.position.y, mainCamera.transform.position.z);
        boxCollider.isTrigger = false;
        while (Vector3.Distance(mainCamera.transform.position, targetPositionX) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPositionX, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMove = false;
        yield return new WaitForSeconds(0.5f);
        boxCollider.isTrigger = true;
        
    }

    IEnumerator MoveCameraToNextPosY(float targetY)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        float moveSpeed = 50;
        float duration = Mathf.Abs((targetY - mainCamera.transform.position.y) / moveSpeed) * Time.deltaTime;
        Vector3 targetPositionY = new Vector3(mainCamera.transform.position.x, targetY, mainCamera.transform.position.z);
        boxCollider.isTrigger = false;
        while (Vector3.Distance(mainCamera.transform.position, targetPositionY) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPositionY, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMove = false;
        yield return new WaitForSeconds(1f);
        boxCollider.isTrigger = true;

    }

    void MovingCamera()
    {
        moveCount = directionX < 0 ? 1 : -1;

        if (tag == "YDown" || tag == "YUp")
        {
            float moveDistanceY = 14;
            float nextPosY = mainCamera.transform.position.y + (tag == "CameraUp" ? moveDistanceY : -moveDistanceY);
            StartCoroutine(MoveCameraToNextPosY(nextPosY));
        }
        else
        {
            float moveDistanceX = 22;
            float nextPosX = mainCamera.transform.position.x + (moveCount > 0 ? moveDistanceX : -moveDistanceX);

            StartCoroutine(MoveCameraToNextPosX(nextPosX));
        }     
    }
}
