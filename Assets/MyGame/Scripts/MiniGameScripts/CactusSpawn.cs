using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    public bool hasntWonYet = true;

    private float elapsedTime = 0f;

    private Coroutine spawnRoutine;
    private Coroutine winRoutine;

    private float totalTime = 20f;
    private float timeLeft;

    private bool countdownActive = true;

    private void Start()
    {
        timeLeft = totalTime;

        spawnRoutine = StartCoroutine(SpawnCactusRoutine());
        winRoutine = StartCoroutine(WinCountdown());

        countdown.maxValue = 1f;
        countdown.value = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (!countdownActive)
            return;

        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Max(timeLeft, 0f);

            countdown.value = 1f - (timeLeft / totalTime);
        }
    }

    private IEnumerator SpawnCactusRoutine()
    {
        while (true)
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
        move.speed = speed;

        Destroy(cactus, cactusLifetime);
    }

    public void Death()
    {
        countdownActive = false;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        if (winRoutine != null)
            StopCoroutine(winRoutine);

        FindAnyObjectByType<TRexManager>().enabled = false;

        foreach (var cactus in FindObjectsByType<CactusMove>(FindObjectsSortMode.None))
            cactus.enabled = false;

        reloadSceneButton.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator WinCountdown()
    {
        yield return new WaitForSeconds(totalTime);

        countdownActive = false;
        hasntWonYet = false;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        FindAnyObjectByType<TRexManager>().enabled = false;

        foreach (var cactus in FindObjectsByType<CactusMove>(FindObjectsSortMode.None))
            cactus.enabled = false;

        winScreen.SetActive(true);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}