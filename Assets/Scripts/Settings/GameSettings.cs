using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    // UI References
    public Text mainVolumeText, musicVolumeText, adsVolumeText, sfxVolumeText;
    public Text moveLeftValueText, moveRightValueText, moveUpValueText, moveDownValueText, moveExitValueText;

    // Current Settings
    private int mainVolume = 100, musicVolume = 100, adsVolume = 100, sfxVolume = 100;
    private string moveLeft = "NONE", moveRight = "NONE", moveUp = "NONE", moveDown = "NONE", moveExit = "J";

    // Default Settings
    private int defaultMainVolume, defaultMusicVolume, defaultAdsVolume, defaultSfxVolume;
    private string defaultMoveLeft, defaultMoveRight, defaultMoveUp, defaultMoveDown, defaultMoveExit;

    // Audio Source References
    private AudioSource[] musicAudioPlayers; // Array for multiple music players
    private AudioSource adsAudioPlayer;
    private AudioSource sfxAudioPlayer;

    void Awake()
    {
        // Initialize default values
        defaultMainVolume = mainVolume;
        defaultMusicVolume = musicVolume;
        defaultAdsVolume = adsVolume;
        defaultSfxVolume = sfxVolume;
        defaultMoveLeft = moveLeft;
        defaultMoveRight = moveRight;
        defaultMoveUp = moveUp;
        defaultMoveDown = moveDown;
        defaultMoveExit = moveExit;
    }

    void Start()
    {
        // Initialize Music Audio (multiple players)
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("music");
        if (musicObjects.Length > 0)
        {
            musicAudioPlayers = new AudioSource[musicObjects.Length];
            for (int i = 0; i < musicObjects.Length; i++)
            {
                musicAudioPlayers[i] = musicObjects[i].GetComponent<AudioSource>();
                if (musicAudioPlayers[i] != null)
                {
                    musicAudioPlayers[i].volume = (musicVolume / 100f) * (mainVolume / 100f);
                }
                else
                {
                    Debug.LogError($"No AudioSource found on music object: {musicObjects[i].name}");
                }
            }
        }
        else
        {
            Debug.LogError("No GameObjects found with tag 'music'.");
        }

        // Initialize Ads Audio
        var adsObject = GameObject.FindWithTag("ads");
        if (adsObject != null)
        {
            adsAudioPlayer = adsObject.GetComponent<AudioSource>();
            if (adsAudioPlayer != null) adsAudioPlayer.volume = (adsVolume / 100f) * (mainVolume / 100f);
            else Debug.LogError("No AudioSource found on object tagged 'ads'.");
        }
        else Debug.LogError("No GameObject found with tag 'ads'.");

        // Initialize SFX Audio
        var sfxObject = GameObject.FindWithTag("sfx");
        if (sfxObject != null)
        {
            sfxAudioPlayer = sfxObject.GetComponent<AudioSource>();
            if (sfxAudioPlayer != null) sfxAudioPlayer.volume = (sfxVolume / 100f) * (mainVolume / 100f);
            else Debug.LogError("No AudioSource found on object tagged 'sfx'.");
        }
        else Debug.LogError("No GameObject found with tag 'sfx'.");

        UpdateUIText();
    }

    // Public Methods
    public void ResetToDefaults()
    {
        mainVolume = defaultMainVolume;
        musicVolume = defaultMusicVolume;
        adsVolume = defaultAdsVolume;
        sfxVolume = defaultSfxVolume;
        moveLeft = defaultMoveLeft;
        moveRight = defaultMoveRight;
        moveUp = defaultMoveUp;
        moveDown = defaultMoveDown;
        moveExit = defaultMoveExit;

        UpdateAllVolumes();
        UpdateUIText();
    }

    public void RollDiceForVolume(string volumeType)
    {
        float chance = Random.Range(0f, 100f);
        int newVolume = chance <= 1f ? Random.Range(0, 16) : Random.Range(16, 101);

        switch (volumeType)
        {
            case "Main":
                mainVolume = newVolume;
                break;
            case "Music":
                musicVolume = newVolume;
                break;
            case "Ads":
                adsVolume = newVolume;
                break;
            case "SFX":
                sfxVolume = newVolume;
                break;
        }

        UpdateAllVolumes();
        UpdateUIText();
    }

    public void RollDiceForKeybind(string action)
    {
        string[] keys = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                        "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "SPACE", "ESC" };

        string randomKey = keys[Random.Range(0, keys.Length)];
        float chance = Random.Range(0f, 100f);

        if (chance <= 30f)
        {
            switch (action)
            {
                case "MoveLeft": moveLeft = "D"; break;
                case "MoveRight": moveRight = "A"; break;
                case "MoveUp": moveUp = Random.Range(0, 2) == 0 ? "W" : "SPACE"; break;
                case "MoveDown": moveDown = "S"; break;
                case "MoveExit": moveExit = "ESC"; break;
            }
        }
        else
        {
            switch (action)
            {
                case "MoveLeft": moveLeft = randomKey; break;
                case "MoveRight": moveRight = randomKey; break;
                case "MoveUp": moveUp = randomKey; break;
                case "MoveDown": moveDown = randomKey; break;
                case "MoveExit": moveExit = randomKey; break;
            }
        }

        UpdateUIText();
    }

    public void SetKeybinds(string left, string right, string up, string down, string exit)
    {
        moveLeft = left;
        moveRight = right;
        moveUp = up;
        moveDown = down;
        moveExit = exit;
        UpdateUIText();
    }

    public void SetVolumes(int main, int music, int ads, int sfx)
    {
        mainVolume = main;
        musicVolume = music;
        adsVolume = ads;
        sfxVolume = sfx;

        UpdateAllVolumes();
        UpdateUIText();
    }

    public string GetKeybind(string action)
    {
        switch (action)
        {
            case "MoveLeft": return moveLeft;
            case "MoveRight": return moveRight;
            case "MoveUp": return moveUp;
            case "MoveDown": return moveDown;
            default: return moveExit;
        }
    }

    public int GetVolume(string volumeType)
    {
        switch (volumeType)
        {
            case "Main": return mainVolume;
            case "Music": return musicVolume;
            case "Ads": return adsVolume;
            case "SFX": return sfxVolume;
            default: return 100;
        }
    }

    public void UpdateUIText()
    {
        UpdateVolumeText(mainVolumeText, "MAIN VOLUME", mainVolume);
        UpdateVolumeText(musicVolumeText, "MUSIC VOLUME", musicVolume);
        UpdateVolumeText(adsVolumeText, "ADS VOLUME", adsVolume);
        UpdateVolumeText(sfxVolumeText, "SFX VOLUME", sfxVolume);

        if (moveLeftValueText != null) moveLeftValueText.text = moveLeft;
        if (moveRightValueText != null) moveRightValueText.text = moveRight;
        if (moveUpValueText != null) moveUpValueText.text = moveUp;
        if (moveDownValueText != null) moveDownValueText.text = moveDown;
        if (moveExitValueText != null) moveExitValueText.text = moveExit;
    }

    private void UpdateVolumeText(Text textComponent, string label, int value)
    {
        if (textComponent == null)
        {
            Debug.LogWarning($"{label} Text component is not assigned!");
            return;
        }

        textComponent.text = $"{label}: {value}";

        if (!textComponent.gameObject.activeInHierarchy)
            Debug.LogWarning($"{label} Text GameObject is not active!");
    }

    // Helper method to update all audio volumes based on main volume
    private void UpdateAllVolumes()
    {
        if (musicAudioPlayers != null)
        {
            foreach (AudioSource player in musicAudioPlayers)
            {
                if (player != null) player.volume = (musicVolume / 100f) * (mainVolume / 100f);
            }
        }
        if (adsAudioPlayer != null) adsAudioPlayer.volume = (adsVolume / 100f) * (mainVolume / 100f);
        if (sfxAudioPlayer != null) sfxAudioPlayer.volume = (sfxVolume / 100f) * (mainVolume / 100f);
    }
}