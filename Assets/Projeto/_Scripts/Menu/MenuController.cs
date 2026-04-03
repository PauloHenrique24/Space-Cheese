using EasyTransition;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public TransitionSettings transition;

    private string url = "https://linktr.ee/spacestudiosbr";
    private string gameUrl = "https://space-studios-oficial.itch.io/pedidos-do-alem";

    public void Play()
    {
        if (PlayerPrefs.HasKey("Dialogo"))
        {
            TransitionManager.Instance().Transition("Game", transition, 0);
        }
        else
        {
            TransitionManager.Instance().Transition("History", transition, 0);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void AvaliableNow()
    {
        Application.OpenURL(gameUrl);
    }
    
    public void LinkTree()
    {
        Application.OpenURL(url);
    }
}
