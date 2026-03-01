using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class CactusSpawn : MonoBehaviour
{
    [Header("Prefabs & Canvas")]
    [SerializeField] GameObject[] cactusPrefabs;
    [SerializeField] Transform canvasTransform;
    [SerializeField] Transform spawnPoint;

    [Header("Speed Ramp")]
    [SerializeField] float startSpeed;
    [SerializeField] float endSpeed;
    [SerializeField] float rampDuration;

    [Header("Spawn & Lifetime")]
    [SerializeField] float spawnIntervalMin;
    [SerializeField] float spawnIntervalMax;
    [SerializeField] float cactusLifetime;
    [SerializeField] GameObject reloadSceneButton;

    [Header("UI")]
    [SerializeField] Slider countdown;
    [SerializeField] GameObject winScreen;

    [Header("Game Settings")]
    [SerializeField] float totalTime = 20f;
    [SerializeField] AudioSource dyingSound;

    public DesktopManager dma;
    public TRexManager trex;

    [SerializeField] float elapsedTime = 0f;
    [SerializeField] float timeLeft;

    [SerializeField] Coroutine spawnRoutine;

    [SerializeField] bool gameActive = true;
    public bool GameActive => gameActive;

    [SerializeField] AudioSource backgroundMusic;
    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            Win();
        }
        if (!gameActive)
            return;

        elapsedTime += Time.deltaTime;

        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(timeLeft, 0f);

        countdown.value = 1f - (timeLeft / totalTime);

        if (timeLeft <= 0f)
        {
            Win();
        }
    }

    void StartGame()
    {
        gameActive = true;
        elapsedTime = 0f;
        timeLeft = totalTime;

        countdown.maxValue = 1f;
        countdown.value = 0f;

        reloadSceneButton.SetActive(false);
        winScreen.SetActive(false);

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = StartCoroutine(SpawnCactusRoutine());
    }

    private IEnumerator SpawnCactusRoutine()
    {
        while (gameActive)
        {
            SpawnCactus();
            float interval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnCactus()
    {
        if (!gameActive) return;
        if (cactusPrefabs.Length == 0) return;

        int index = Random.Range(0, cactusPrefabs.Length);
        GameObject cactus = Instantiate(cactusPrefabs[index], canvasTransform);

        RectTransform rt = cactus.GetComponent<RectTransform>();
        rt.anchoredPosition = spawnPoint.GetComponent<RectTransform>().anchoredPosition;

        float speed = Mathf.Lerp(startSpeed, endSpeed, Mathf.Clamp01(elapsedTime / rampDuration));

        var move = cactus.AddComponent<CactusMove>();
        move.Initialize(this, speed);

        Destroy(cactus, cactusLifetime);
    }

    public void Death()
    {
        if (!gameActive) return;

        gameActive = false;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        if (trex != null)
        {
            trex.dinoAnimator.speed = 0f;
            trex.enabled = false;
        }

        foreach (var cactus in FindObjectsByType<CactusMove>(FindObjectsSortMode.None))
            cactus.enabled = false;

        dyingSound.Play();
        reloadSceneButton.SetActive(true);
    }

    void Win()
    {
        if (!gameActive) return;

        gameActive = false;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        if (trex != null)
            trex.enabled = false;

        foreach (var cactus in FindObjectsByType<CactusMove>(FindObjectsSortMode.None))
            cactus.enabled = false;

        winScreen.SetActive(true);

        StartCoroutine(WinDelay());
    }

    IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(3);
        dma.CallEvent(dma.gayIndex);
        dma.gayIndex++;
        backgroundMusic.mute = false;
        canvasTransform.gameObject.SetActive(false);
        dma.apps[0].SetActive(true);
        gameObject.SetActive(false);
    }

    public void ReloadSceneManual()
    {
        StopAllCoroutines();

        foreach (var cactus in FindObjectsByType<CactusMove>(FindObjectsSortMode.None))
            Destroy(cactus.gameObject);

        StartGame();
    }
}