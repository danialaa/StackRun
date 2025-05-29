using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isRunning { get; private set; }
    [HideInInspector] public bool isStacked { get; private set; }

    float yAmountToElevate = 1f;
    float zAmountToSit = -1f;

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
        isRunning = false;
        isStacked = true;
        gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        gameObject.GetComponent<Animator>().SetBool("isStacked", true);

        gameObject.GetComponent<Rigidbody>().useGravity = false;
        MoveWithRunner(transform.position);
    }

    public void MoveWithRunner(Vector3 runnerPosition)
    {
        transform.position = new Vector3(runnerPosition.x, runnerPosition.y + yAmountToElevate, runnerPosition.z + zAmountToSit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            if (!isRunning)
            {
                StartRunning();
                GameManager.Instance.SetNewRunner(gameObject);
            }
            else
            {
                StackCharacter();
            }
        }
    }
}
