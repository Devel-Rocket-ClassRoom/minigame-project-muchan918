using UnityEngine;

public class TutorialAutoShow : MonoBehaviour
{
    [SerializeField]
    private TutorialManager tutorialManager;

    private void Start()
    {
        if (PlayerPrefs.GetInt("ShowTutorialOnStart", 0) == 1)
        {
            PlayerPrefs.DeleteKey("ShowTutorialOnStart");
            TileMapGenerator.OnMapLoadComplete += ShowTutorial;
        }
    }

    private void ShowTutorial()
    {
        TileMapGenerator.OnMapLoadComplete -= ShowTutorial;
        tutorialManager.Show();
    }
}
