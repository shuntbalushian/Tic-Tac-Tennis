using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public Button restart;
    public AudioSource grunt1;
    public AudioSource grunt2;

    private string[] originBoard;
    private string playerSide;
    private int moveCount;
    private string humanPlayer;
    private string aiPlayer;

    public class Move
    {
        public int index;
        public int score;

        public Move()
        {
            index = -1;
            score = -1;
        }
    }
    void Awake()

    {
        SetGameControllerReferenceOnButtons();
        for (int i = 0; i < buttonList.Length - 2; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
        playerSide = "X";
        humanPlayer = "";
        aiPlayer = "";
        SetGameOverText("Choose who goes first.");
        moveCount = 0;
        originBoard = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        restart.gameObject.SetActive(false);
    }
    public void SetPlayers(string player1)
    {
        buttonList[9].GetComponentInParent<Button>().interactable = false;
        buttonList[10].GetComponentInParent<Button>().interactable = false;

        if (string.Compare(player1, "Player") == 0)
        {
            humanPlayer = "X";
            aiPlayer = "O";
            for (int i = 0; i < buttonList.Length - 2; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = true;
            }
            gameOverPanel.SetActive(false);
        }
        else
        {
            gameOverPanel.SetActive(false);
            humanPlayer = "O";
            aiPlayer = "X";
            Move indexOfMove = Minimax(originBoard, aiPlayer, 0);
            originBoard[indexOfMove.index] = aiPlayer;
            buttonList[indexOfMove.index].text = aiPlayer;
            
            if (aiPlayer == "X")
            {
                buttonList[indexOfMove.index].GetComponentInParent<Image>().sprite = buttonList[0].GetComponentInParent<GridSpace>().sprites[0];
                ColorBlock c = buttonList[indexOfMove.index].GetComponentInParent<Button>().colors;
                c.normalColor = Color.white;
                buttonList[indexOfMove.index].GetComponentInParent<Button>().colors = c;
            }
            else
            {
                buttonList[indexOfMove.index].GetComponentInParent<Image>().sprite = buttonList[1].GetComponentInParent<GridSpace>().sprites[0];
                ColorBlock c = buttonList[indexOfMove.index].GetComponentInParent<Button>().colors;
                c.normalColor = Color.white;
                buttonList[indexOfMove.index].GetComponentInParent<Button>().colors = c;
            }
            grunt2.Play();
            EndTurn();
            for (int i = 0; i < buttonList.Length - 2; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = true;
            }
        }

    }
    public Move Minimax(string[] board, string player, int depth)
    {
        depth++;
        List<int> spots = AvailableSpots(board);
        if (GameWon(board, aiPlayer) || depth == 0)
        {
            Move m = new Move();
            m.score = 10 - depth;
            return m;
        }
        else if (GameWon(board, humanPlayer))
        {
            Move m = new Move();
            m.score = depth - 10;
            return m;
        }
        else if(spots.Count == 0)
        {
            Move m = new Move();
            m.score = 0;
            return m;
        }
        List<Move> moves = new List<Move>();
        foreach (int i in spots)
        {
            Move m = new Move();
            m.index = i;
            board[i] = player;
            if (player == aiPlayer)
            {
                Move result = Minimax(board, humanPlayer, depth);
                m.score = result.score;
            }
            else
            {
                Move result = Minimax(board, aiPlayer,depth);
                m.score = result.score;
            }
            board[i] = m.index.ToString();
         
            
            moves.Add(m);
        }
        int bestMove = 0;

        if (player == aiPlayer)
        {
            int bestScore = -10000;
            int i = 0;
            foreach (Move m in moves)
            {
               
                if (m.score > bestScore)
                {
                    bestScore = m.score;
                    bestMove = i;
                }
                i++;
            }
        }
        else
        {

            
            int bestScore = 10000;
            int i = 0;
            foreach (Move m in moves)
            {
                
                if (m.score < bestScore)
                {
                    bestScore = m.score;
                    bestMove = i;
                }
                i++;
            }
        }

        return moves[bestMove];

    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }
    public List<int> AvailableSpots(string[] board)
    {
        List<int> spots = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (string.Compare(board[i], "X") == 0 || string.Compare(board[i], "O") == 0)
            {
                
            }
            else
            {
                spots.Add(i);
            }
        }
        return spots;
    }
    public bool GameWon(string[] board, string player)
    {
        if (board[0] == player && board[1] == player && board[2] == player)
        {
            return true;

        }

        else if (board[3] == player && board[4] == player && board[5] == player)
        {
            return true;

        }

        else if (board[6] == player && board[7] == player && board[8] == player)
        {
            return true;

        }

        else if (board[0] == player && board[3] == player && board[6] == player)
        {
            return true;

        }

        else if (board[1] == player && board[4] == player && board[7] == player)
        {
            return true;

        }

        else if (board[2] == player && board[5] == player && board[8] == player)
        {
            return true;

        }

        else if (board[0] == player && board[4] == player && board[8] == player)
        {
            return true;

        }

        else if (board[2] == player && board[4] == player && board[6] == player)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    public void EndTurn()
    {
        moveCount++;

        if (GameWon(originBoard,playerSide))
        {
            GameOver();
            return;
        }
        if (moveCount >= 9)
        {
            SetGameOverText("It's a draw!");
            restart.gameObject.SetActive(true);
            return;
        }

        StartCoroutine(PauseGame(0.8f));
    }
    void PrintBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            Debug.Log(originBoard[i]);
        }
    }
    public void SetBoardSpace(string index)
    {
        int indexToAdd = int.Parse(index);
        originBoard[indexToAdd] = humanPlayer;
        grunt1.Play();
        EndTurn();
    }
    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        if (playerSide == aiPlayer)
        {
            Move indexOfMove = Minimax(originBoard, aiPlayer,0);
            originBoard[indexOfMove.index] = aiPlayer;
            buttonList[indexOfMove.index].text = aiPlayer;
            if (aiPlayer == "X")
            {
                buttonList[indexOfMove.index].GetComponentInParent<Image>().sprite = buttonList[indexOfMove.index].GetComponentInParent<GridSpace>().sprites[0];
                ColorBlock c = buttonList[indexOfMove.index].GetComponentInParent<Button>().colors;
                c.normalColor = Color.white;
                buttonList[indexOfMove.index].GetComponentInParent<Button>().colors = c;
            }
            else
            {
                buttonList[indexOfMove.index].GetComponentInParent<Image>().sprite = buttonList[indexOfMove.index].GetComponentInParent<GridSpace>().sprites[1];
                ColorBlock c = buttonList[indexOfMove.index].GetComponentInParent<Button>().colors;
                c.normalColor = Color.white;
                buttonList[indexOfMove.index].GetComponentInParent<Button>().colors = c;
            }
            grunt2.Play();
            EndTurn();
        }
    }

    void GameOver()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
        SetGameOverText(playerSide + " Wins!");
        restart.gameObject.SetActive(true);
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    IEnumerator PauseGame(float x)
    {
        yield return new WaitForSeconds(x);
        ChangeSides();
    }
}