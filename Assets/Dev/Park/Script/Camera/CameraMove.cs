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
    int changeUp = 1;    //�� �Ʒ��� �����̴� ī�޶��̵� ������Ʈ�� 
    int changeDown = 1;

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
                directionY = transform.position.y - player.transform.position.y;
                if (isMove)
                {
                    if (tag == "CameraDown")
                    {
                        if(changeDown == 1)
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 5f, mainCamera.transform.position.z);
                        else
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 5f, mainCamera.transform.position.z);

                        changeDown *= -1;
                    }
                    else if (tag == "CameraUp")
                    {
                        
                        if (changeUp == 1)
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 5f, mainCamera.transform.position.z);
                        else
                            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 5f, mainCamera.transform.position.z);

                        changeUp *= -1;
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
        float moveSpeed = 18;
        float duration = Mathf.Abs((targetX - mainCamera.transform.position.x) / moveSpeed) * Time.deltaTime;
        Vector3 targetPositionX = new Vector3(targetX, mainCamera.transform.position.y, mainCamera.transform.position.z);
        boxCollider.isTrigger = false;
        while (Vector3.Distance(mainCamera.transform.position, targetPositionX) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPositionX, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMove = false;
        yield return new WaitForSeconds(1f);
        boxCollider.isTrigger = true;
        
    }

    IEnumerator MoveCameraToNextPosY(float targetY)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        float moveSpeed = 18;
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
        moveCount += directionX < 0 ? 1 : -1;
        //YmoveCount += directionY < 0 ? 1 : -1;

        if (moveCount >= 0)
        {
            float moveDistanceX = 20;
            float moveDistanceY = 10;
            float nextPosX = mainCamera.transform.position.x + (directionX < 0 ? moveDistanceX : -moveDistanceX);
            float nextPosY = mainCamera.transform.position.y + (directionY < 0 ? moveDistanceY : -moveDistanceY);
            StartCoroutine(MoveCameraToNextPosX(nextPosX));
        }
        else
        {
            moveCount = 0;
        }
    }
}
