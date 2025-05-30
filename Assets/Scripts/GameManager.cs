using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Material> colorPool = new List<Material>();
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform plane;
    [SerializeField] int idleCt = 3;

    //make a pool of random unique vectors in plane and then randomly place characters into those ((could be a dictionary of vector bool to make sure its not already occupied))
    private List<PlayerController> characters = new List<PlayerController>();
    private PlayerController currentPlayer;

    public int stackCt = 0;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateFirstPlayer();
        CreateIdleCharacters();

        //float minX = plane.position.x - (plane.localScale.x / 2) + playerPrefab.transform.localScale.x;
        //float maxX = plane.position.x + (plane.localScale.x / 2) - playerPrefab.transform.localScale.x;
        //float minZ = plane.position.z - (plane.localScale.z / 2) + playerPrefab.transform.localScale.z;
        //float maxZ = plane.position.z + (plane.localScale.z / 2) - playerPrefab.transform.localScale.z;
    }

    private void CreateFirstPlayer()
    {
        GameObject newCharacter = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        currentPlayer = newCharacter.GetComponent<PlayerController>();
        currentPlayer.StartRunning();

        characters.Add(currentPlayer);
    }

    private void CreateIdleCharacters()
    {
        int[] pos = new int[3] { -10, -4, 4 };

        for (int i = 0; i < idleCt; i++)
        {
            //Vector3 randomPos = new Vector3(UnityEngine.Random.Range(minX, maxX), playerPrefab.transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
            GameObject newCharacter = Instantiate(playerPrefab, /*randomPos*/ new Vector3(playerPrefab.transform.position.x, playerPrefab.transform.position.y, pos[i]), Quaternion.identity);
            newCharacter.GetComponentInChildren<Renderer>().material = colorPool[UnityEngine.Random.Range(0, colorPool.Count)];

            characters.Add(newCharacter.GetComponent<PlayerController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        UpdateStackedCharacters();
    }

    void UpdateStackedCharacters()
    {
        List<PlayerController> stackedPlayers = characters.Where(c => c.isStacked).OrderByDescending(c => c.stackedID).ToList();
        float lastY = currentPlayer.transform.position.y;

        for (int i = 0; i < stackedPlayers.Count; i++)
        {
            lastY = stackedPlayers[i].MoveWithRunner(currentPlayer.transform.position, currentPlayer.transform.rotation, i == 0 ? true : false, lastY);
        }
    }

    public void SetNewRunner(GameObject newRunner)
    {
        currentPlayer = newRunner.GetComponent<PlayerController>();
    }
}
