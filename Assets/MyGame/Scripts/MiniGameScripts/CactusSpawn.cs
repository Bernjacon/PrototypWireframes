using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CactusSpawn : MonoBehaviour
{
    [Header("Prefabs & Canvas")]
    public GameObject[] cactusPrefabs;
    public Transform canvasTransform;
    public Transform spawnPoint;

    [Header("Speed Ramp")]
    public float startSpeed;
    public float endSpeed;
    public float rampDuration;

    [Header("Spawn & Lifetime")]
    public float spawnIntervalMin;
    public float spawnIntervalMax;
    public float cactusLifetime;
    public GameObject reloadSceneButton;

    [Header("UI")]
    public Slider countdown;
    public GameObject winScreen;

    [Header("Game Settings")]
    public float totalTime = 20f;

    public DesktopManager dma;
    public TRexManager trex;

    private float elapsedTime = 0f;
    private float timeLeft;

    private Coroutine spawnRoutine;

    private bool gameActive = true;
    public bool GameActive => gameActive;

    bool firstTime = true;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
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

        if (trex != null)
        {
            trex.enabled = true;
            trex.dinoAnimator.speed = 1f;
            trex.dinoAnimator.Play("Run", 0, 0f);
        }

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
        if (firstTime)
        {
            firstTime = !firstTime;
            dma.CallEvent(5);
        }


        if (!firstTime)
            dma.CallEvent(7);
    }

    public void ReloadSceneManual()
    {
        StopAllCoroutines();

        foreach (var cactus in FindObjectsByType<CactusMove>(FindObjectsSortMode.None))
            Destroy(cactus.gameObject);

        StartGame();
    }
}