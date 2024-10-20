using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : Entity
{
    #region Player Variables

    [Space(10)]
    [Header("Player Movement Variables")]
    public float moveSpeed = 20f;
    public Vector3 spawnPoint = Vector3.zero;
    public bool isSlowed;
    public float slowTimer;
    public Rigidbody rb;
    public bool gotHit;
    public bool isRed;
    public float flashCount = 5f;
    public Renderer model;
    public MovementState state;
    public enum MovementState
    {
        moving,
        damaged
    };
    [HideInInspector] public InputHandler inputHandler;
    public List<GameObject> hatList = new List<GameObject>();

    //etc.

    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            _instance = FindObjectOfType<PlayerController>();
            return _instance;
        }
    }

    private AudioSource sharkBite;
    private AudioSource crabSnap;
    #endregion
    private void Awake()
    {
        gameHandler = GameHandler.Instance;
        gameHandler.playerPos = gameObject.transform;
        inputHandler = InputHandler.Instance;
        rb = GetComponent<Rigidbody>();
        sharkBite = gameHandler.gameAudioData.AddNewAudioSourceFromGroup("Hazard", "Shark", gameObject, "Bite");
        crabSnap = gameHandler.gameAudioData.AddNewAudioSourceFromGroup("Hazard", "Crab", gameObject, "Snap");
        if (gameHandler.stageName == "Reef")
        {

        }
    }

    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if(gameHandler.timerOn)
                PlayerInput();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !gotHit)
        {
            gameHandler.uiHandler.factToDisplay = gameHandler.currentLevelData.HazardFact(gameHandler.stageName,other.GetComponent<Info>().hazardName);
            TakeDamage(1);
            if (other.GetComponent<Info>().hazardName == "Crab")
            {
                crabSnap.Play();
            }
            if (gameHandler.stageName == "Beach")
            {
                Respawn();
                if (health != 0)
                {
                    gameHandler.uiHandler.hintText.text = gameHandler.currentLevelData.HazardHint(gameHandler.stageName, other.GetComponent<Info>().hazardName);
                    gameHandler.uiHandler.ShowHint();
                }
            }
            else
            {
                if (health != 0)
                {
                    gameHandler.uiHandler.hintText.text = gameHandler.currentLevelData.HazardHint(gameHandler.stageName, other.GetComponent<Info>().hazardName);
                    gameHandler.uiHandler.ShowHint();
                }
                if (other.GetComponent<Info>().hazardName == "Shark")
                {
                    sharkBite.Play();
                }
            }
            gotHit = true;
            StartCoroutine(Flash());
        }
        if (other.gameObject.tag == "CheckPoint")
        {
            SetSpawn(other.transform);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Oil" && !isSlowed)
        {
            SlowEffect();
        }
    }
    private IEnumerator Flash()
    {
        if (model != null && gotHit)
        {
            Color originalColour = model.material.GetColor("_Colour");
            float elapsedTime = 0f;
            gameHandler.uiHandler.UpdateHealth(health);

            while (elapsedTime < 2f)
            {
                if (!isRed)
                {
                    isRed = true;
                    model.material.SetColor("_Colour", Color.red);
                }
                else
                {
                    isRed = false;
                    model.material.SetColor("_Colour", originalColour);
                }
                // Wait for the next flash
                yield return new WaitForSeconds(0.2f);
                elapsedTime += 0.2f;
            }

            // Ensure the renderer is enabled at the end
            model.material.SetColor("_Colour", originalColour);
            gotHit = false;
        }
    }

    public void SetHat()
    {
        List<string> currentHatKeyList = gameHandler.gameData.CheckWhichHat();
        foreach (var hat in hatList)
        {
            hat.SetActive(false);
            foreach (var key in currentHatKeyList)
            {
                if (hat.name == key)
                {
                    hat.SetActive(true);
                }
            }
        }
    }
    private void SetSpawn(Transform point)
    {
        spawnPoint = point.position;
        spawnPoint.x = 0;
        spawnPoint.y = transform.position.y;
    }
    public abstract void PlayerInput();
    public abstract void MovePlayer();
    public abstract void Respawn();

    public abstract void SlowEffect();
    public abstract void SlowTimer();

}
