using System.Collections;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSecurity : MonoBehaviour
{
    [SerializeField] private GameObject rotObj;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float minAngle = -45f;
    [SerializeField] private float maxAngle = 45f;

    [SerializeField] private bool notmove;

    private float angleRange;
    private float speed;

    bool stop = false;

    void Start()
    {
        stop = false;

        if (!notmove)
        {
            angleRange = maxAngle - minAngle;
            speed = Random.Range(rotationSpeed - 5, rotationSpeed + 5);
        }
    }

    void LateUpdate()
    {
        if (!notmove)
        {
            float zAngle = Mathf.PingPong(Time.time * speed, angleRange) + minAngle;
            Vector3 currentRotation = rotObj.transform.eulerAngles;
            rotObj.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, zAngle);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !stop)
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(NextLevel());
            stop = true;
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return new WaitForSeconds(2f);
        stop = false;
    }

}
