using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isRunning { get; private set; }
    [HideInInspector] public bool isStacked { get; private set; }
    [HideInInspector] public int stackedID { get; private set; }

    float firstToElevate = 0.8f;
    float otherToElevate = 0.7f;
    float stackedXRotation = 24f;
    float sittingZDistance = -0.15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRunning()
    {
        isRunning = true;
        gameObject.GetComponent<Animator>().SetBool("isRunning", true);
    }

    public void StackCharacter()
    {
        stackedID = ++GameManager.Instance.stackCt;

        isRunning = false;
        isStacked = true;
        gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        gameObject.GetComponent<Animator>().SetBool("isStacked", true);

        gameObject.GetComponent<BoxCollider>().enabled = false;
        MoveWithRunner(transform.position, transform.rotation, true, transform.position.y);
    }

    public float MoveWithRunner(Vector3 runnerPosition, Quaternion runnerRotation, bool isFirstToElevate, float lastY)
    {
        transform.position = new Vector3(runnerPosition.x, lastY + (isFirstToElevate ? firstToElevate : otherToElevate), runnerPosition.z + sittingZDistance);
        transform.rotation = Quaternion.Euler(new Vector3(stackedXRotation, runnerRotation.eulerAngles.y, runnerRotation.eulerAngles.z));

        return transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            if (!isRunning && !isStacked)
            {
                GameManager.Instance.SetNewRunner(gameObject);
                StartRunning();
            }
            else if (isRunning && !isStacked)
            {
                StackCharacter();
            }
        }
    }
}
