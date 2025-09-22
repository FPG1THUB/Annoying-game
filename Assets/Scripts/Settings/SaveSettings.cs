using UnityEngine;

public class SaveSettings : MonoBehaviour
{
    private GameSettings gameSettings;

    void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();
        if (gameSettings == null) Debug.LogError("GameSettings missing!");
        else gameSettings.ResetToDefaults();
    }

    // For manual saving of current volumes
    public void SaveCurrentVolumes()
    {
        if (gameSettings == null) return;
        gameSettings.SetVolumes(
            gameSettings.GetVolume("Main"),
            gameSettings.GetVolume("Music"),
            gameSettings.GetVolume("Ads"),
            gameSettings.GetVolume("SFX")
        );
    }

    // For manual saving of current keybinds
    public void SaveCurrentKeybinds()
    {
        if (gameSettings == null) return;
        gameSettings.SetKeybinds(
            gameSettings.GetKeybind("MoveLeft"),
            gameSettings.GetKeybind("MoveRight"),
            gameSettings.GetKeybind("MoveUp"),
            gameSettings.GetKeybind("MoveDown"),
            gameSettings.GetKeybind("MoveExit")
        );
    }

    // For external calls with specific values
    public void SaveVolumes(int main, int music, int ads, int sfx)
    {
        if (gameSettings == null) return;
        gameSettings.SetVolumes(main, music, ads, sfx);
    }

    public void SaveKeybinds(string left, string right, string up, string down, string exit)
    {
        if (gameSettings == null) return;
        gameSettings.SetKeybinds(left, right, up, down, exit);
    }
}
