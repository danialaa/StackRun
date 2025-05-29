using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isRunning = false;

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

}
