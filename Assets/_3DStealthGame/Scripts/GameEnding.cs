using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public UIDocument uiDocument;
    public AudioSource exitAudio;
    public AudioSource caughtAudio;

    bool m_HasAudioPlayed;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;

    private VisualElement m_EndScreen;
    private VisualElement m_CaughtScreen;

    void Start()
    {
        m_EndScreen = uiDocument.rootVisualElement.Q<VisualElement>("EndScreen");
        m_CaughtScreen = uiDocument.rootVisualElement.Q<VisualElement>("CaughtScreen");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;

            // RESET AUDIO + TIMER WHEN END STARTS
            m_HasAudioPlayed = false;
            m_Timer = 0f;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;

        // RESET AUDIO + TIMER WHEN END STARTS
        m_HasAudioPlayed = false;
        m_Timer = 0f;
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(m_EndScreen, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(m_CaughtScreen, true, caughtAudio);
        }
    }

    void EndLevel(VisualElement element, bool doRestart, AudioSource audioSource)
    {
        // FIXED: Play audio only once, when it hasn't played yet
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        // Fade begins
        m_Timer += Time.deltaTime;
        element.style.opacity = m_Timer / fadeDuration;

        // End the game / restart
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Application.Quit();
                Time.timeScale = 0;
            }
        }
    }
}
