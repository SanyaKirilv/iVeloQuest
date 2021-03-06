using UnityEngine;
using UnityEngine.SceneManagement;

public class Playfield : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    public GameObject scoretxt;
    public Spawner _spawner;
    public WinGame _wingame;
    static int score;
    string gameState;
    
    GameObject[] tetr;

    public static Transform[,] grid = new Transform[w, h];
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),Mathf.Round(v.y));
    }
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0);
    }
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }
    public static void decreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    public static void decreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }
    public static void deleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                score += 1;
                --y;
                PlayerPrefs.SetInt("ScoreTetris", score);
            }
        }
    }
    void Update()
    {
        tetr = GameObject.FindGameObjectsWithTag("tetr");
        gameState = PlayerPrefs.GetString("Game");
        if(score >= 10)
        {
            _wingame.FinishGame();
        }
        if(gameState == "GO")
        {
            for (int i = 0; i < tetr.Length; ++i)
                Destroy(tetr[i]);
            SceneManager.LoadScene("Tetris");
            PlayerPrefs.SetString("Game", "PL");
        }
    }
}
