using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] List<Material> colorPool = new List<Material>();
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform plane;
    [SerializeField] int idleCt = 6;

    private Dictionary<Vector3, bool> locationPool = new Dictionary<Vector3, bool>();
    private List<PlayerController> characters = new List<PlayerController>();
    private PlayerController currentPlayer;
    private int scoreValue = 0;
    private bool isGoingLeft = false;
    private bool isGoingRight = false;

    public int stackCt = 0;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        locationPool = new Dictionary<Vector3, bool>() { { new Vector3(2, 0, -1), false },
                                                         { new Vector3(0, 0, 3), false },
                                                         { new Vector3(-4, 0, -7), false },
                                                         { new Vector3(11, 0, 14), false },
                                                         { new Vector3(-9, 0, 25), false },
                                                         { new Vector3(13, 0, -5), false },
                                                         { new Vector3(6, 0, 20), false },
                                                         { new Vector3(-10, 0, -9), false },
                                                         { new Vector3(-13, 0, 35), false },
                                                         { new Vector3(1, 0, 17), false }};
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateFirstPlayer();
        CreateIdleCharacters();
        cameraController.SetOffset(currentPlayer.transform.position);
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
        for (int i = 0; i < idleCt; i++)
        {
            List<Vector3> availablePositions = locationPool.Where(l => !l.Value).Select(l => l.Key).ToList();
            int randomPos = UnityEngine.Random.Range(0, availablePositions.Count);

            GameObject newCharacter = Instantiate(playerPrefab, availablePositions[randomPos], Quaternion.identity);
            newCharacter.GetComponentInChildren<Renderer>().material = colorPool[UnityEngine.Random.Range(0, colorPool.Count)];

            characters.Add(newCharacter.GetComponent<PlayerController>());

            locationPool[availablePositions[randomPos]] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || isGoingLeft)
        {
            isGoingLeft = true;
            currentPlayer.MoveRunner(new Vector3(-0.1f, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.D) || isGoingRight)
        {
            isGoingRight = true;
            currentPlayer.MoveRunner(new Vector3(0.1f, 0, 0));
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            isGoingLeft = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            isGoingRight = false;
        }
    }

    private void LateUpdate()
    {
        UpdateStackedCharacters();
        cameraController.UpdateCameraToRunner(currentPlayer.transform.position);
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
        locationPool[locationPool.FirstOrDefault(l => l.Key == currentPlayer.transform.position).Key] = false;

        scoreValue++;
        scoreText.text = scoreValue.ToString();
    }
}
