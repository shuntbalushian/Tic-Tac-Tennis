using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridSpace : MonoBehaviour
{

    public Button button;
    public Text buttonText;
    public Sprite[] sprites;
    public AudioSource[] sounds;
    private GameController gameController;

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

    public void SetSpace()
    {
        buttonText.text = gameController.GetPlayerSide();
        if (gameController.GetPlayerSide() == "X")
        {
            this.GetComponent<Image>().sprite = sprites[0];
            ColorBlock c = button.colors;
            c.disabledColor = Color.white;
            button.colors = c;
        }
        else
        {
            this.GetComponent<Image>().sprite = sprites[1];
            ColorBlock c = button.colors;
            c.disabledColor = Color.white;
            button.colors = c;
        }
        
        button.interactable = false;
        gameController.SetBoardSpace(button.name);
    }

    public void choosePlayer()
    {
        gameController.SetPlayers(buttonText.text);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }
}