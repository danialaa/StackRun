using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isRunning { get; private set; }
    [HideInInspector] public bool isStacked { get; private set; }
    [HideInInspector] public int stackedID { get; private set; }

    float yAmountToElevate = 1.6f;

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
        MoveWithRunner(transform.position, transform.rotation, 1);
    }

    public void MoveWithRunner(Vector3 runnerPosition, Quaternion runnerRotation, int numberInStack)
    {
        transform.position = new Vector3(runnerPosition.x, runnerPosition.y + yAmountToElevate + numberInStack, runnerPosition.z);
        transform.rotation = runnerRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            if (!isRunning && !isStacked)
            {
                StartRunning();
                GameManager.Instance.SetNewRunner(gameObject);
            }
            else if (isRunning && !isStacked)
            {
                StackCharacter();
            }
        }
    }
}
