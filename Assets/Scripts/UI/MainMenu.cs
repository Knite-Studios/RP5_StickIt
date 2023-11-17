using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip _menuClip;
    [SerializeField]
    private GameObject muteIcon;
    [SerializeField]
    private GameObject settings;

    private bool isMuted = false;
    private bool settingIsOn = false;

    private AudioSource _menuSpeaker;

    private void Awake()
    {  

        muteIcon.SetActive(isMuted);

        settings.SetActive(false);
        muteIcon.SetActive(isMuted);

        _menuSpeaker = GetComponent<AudioSource>();
        _menuSpeaker.loop = true;
    }
    void Start()
    {
       
        PlayMainMusic();
        Cursor.visible = true;

    }
    private void PlayMainMusic()
    {
        if (_menuSpeaker != null && _menuClip != null)
        {
            _menuSpeaker.clip = _menuClip;
            _menuSpeaker.Play();
        }
    }
    public void ToggleSettings()
    {
        settingIsOn = !settingIsOn;
        settings.SetActive(settingIsOn);
    }
    public void ToggleMute()
    {

        isMuted = !isMuted;
        muteIcon.SetActive(isMuted);
        _menuSpeaker.mute = isMuted;
    }
    public void Play()
    {
        SceneManager.LoadScene("Loading");
    }
    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
}
