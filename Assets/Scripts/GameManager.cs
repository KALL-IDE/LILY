using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Explosion and reset")]
    public ParticleSystem explosionParticles;
    public AudioClip explodeClip;
    public float resetDelay = 1.0f;

    [Header("Win UI")]
    public GameObject winPanel;

    private AudioSource audioSource;
    private Vector3 playerStartPos;
    private Vector3[] crateStartPositions;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerStartPos = player.transform.position;

        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        crateStartPositions = new Vector3[crates.Length];
        for (int i = 0; i < crates.Length; i++) crateStartPositions[i] = crates[i].transform.position;

        if (winPanel != null) winPanel.SetActive(false);

        // Background music placeholder: if an AudioSource component is present, play its clip
        AudioSource bgm = GetComponent<AudioSource>();
        if (bgm != null && bgm.clip != null) { bgm.loop = true; bgm.Play(); }
    }

    public void TriggerExplosion()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            if (explosionParticles != null)
            {
                ParticleSystem ps = Instantiate(explosionParticles, player.transform.position, Quaternion.identity);
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration + 0.5f);
            }
            if (explodeClip != null) audioSource.PlayOneShot(explodeClip);

            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        ResetLevel();
    }

    public void ResetLevel()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerStartPos;
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;
        }

        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        for (int i = 0; i < crates.Length; i++)
        {
            if (i < crateStartPositions.Length)
                crates[i].transform.position = crateStartPositions[i];
        }

        if (winPanel != null) winPanel.SetActive(false);
    }

    public void CheckWinCondition()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        foreach (var g in goals)
        {
            Collider2D hit = Physics2D.OverlapPoint(g.transform.position);
            if (hit == null) return;
            if (!hit.CompareTag("Crate")) return;
        }

        WinLevel();
    }

    void WinLevel()
    {
        if (winPanel != null) winPanel.SetActive(true);
        // optionally pause the game
    }

    // UI buttons
    public void OnRestartButton() => ResetLevel();
    public void OnNextLevelButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void OnQuitToMenu() => SceneManager.LoadScene("MainMenu");
}
