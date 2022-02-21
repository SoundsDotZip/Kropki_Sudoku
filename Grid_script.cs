using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid_script : MonoBehaviour
{
    // Custom functions
    public void Shuffle(int[] a) { for (int i = a.Length - 1; i > 0; i--) { int rnd = Random.Range(0, i); int temp = a[i]; a[i] = a[rnd]; a[rnd] = temp; } }
    public string bo(bool condition, string iftrue, string iffalse) { if (condition) return iftrue; return iffalse; }
    public bool isdone()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (user_grid[i, j].Count != 1 || user_grid[i, j][0] != base_grid[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    // Variables
    public Transform selection_transform;
    public SpriteRenderer grid_spriterenderer;
    public TextMeshPro BigGrid_text;
    public TextMeshPro SmallGrid_text;
    public Sprite[] grid_sprites;
    public GameObject Bsep_h;
    public GameObject Wsep_h;
    public GameObject Bsep_v;
    public GameObject Wsep_v;
    static int size = 5;
    int[,] base_grid = new int[size, size];
    int[] grid_selection = new int[2] { 0, 0 };
    int[] timer = new int[2] { -15, -15 };
    List<int>[,] user_grid = new List<int>[size, size];
    bool done = false;

    // Start function
    void Start()
    {
        Application.targetFrameRate = 60;
        grid_spriterenderer.sprite = grid_sprites[size - 4];
        BigGrid_text.transform.position = new Vector3((9 - size) * 0.46875f, 0, -2);
        SmallGrid_text.transform.position = new Vector3((9 - size) * 0.46875f + 0.04f, 0, -2);
        int[] numbers = new int[size];
        for (int i = 0; i < size; i++)
        {
            numbers[i] = i + 1;
        }
        Shuffle(numbers);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                base_grid[i, j] = numbers[(j + 1 + i) % size];
                user_grid[i, j] = new List<int>();
            }
        }
        for (int i = 0; i < size-1; i++)
        {
            int rnd = Random.Range(i+1, size);
            for (int j = 0; j < size; j++)
            {
                int temp = base_grid[i, j];
                base_grid[i, j] = base_grid[rnd, j];
                base_grid[rnd, j] = temp;
            }
        }
        for (int i = 0; i < size - 1; i++)
        {
            int rnd = Random.Range(i + 1, size);
            for (int j = 0; j < size; j++)
            {
                int temp = base_grid[j, i];
                base_grid[j, i] = base_grid[j, rnd];
                base_grid[j, rnd] = temp;
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size-1; j++)
            {
                if (base_grid[i, j] + 1 == base_grid[i, j + 1] || base_grid[i, j] - 1 == base_grid[i, j + 1])
                {
                    Instantiate(Wsep_h, new Vector3(-0.46875f * (size - 2) + 0.9375f * j, 0.46875f * (size - 1) - 0.9375f * i, -1), Quaternion.identity);
                }
                if (base_grid[i, j] * 2 == base_grid[i, j + 1] || base_grid[i, j + 1] * 2 == base_grid[i, j])
                {
                    Instantiate(Bsep_h, new Vector3(-0.46875f * (size - 2) + 0.9375f * j, 0.46875f * (size - 1) - 0.9375f * i, -1), Quaternion.identity);
                }
                if (base_grid[j, i] + 1 == base_grid[j + 1, i] || base_grid[j, i] - 1 == base_grid[j + 1, i])
                {
                    Instantiate(Wsep_v, new Vector3(-0.46875f * (size - 1) + 0.9375f * i, 0.46875f * (size - 2) - 0.9375f * j, -1), Quaternion.identity);
                }
                if (base_grid[j, i] * 2 == base_grid[j + 1, i] || base_grid[j + 1, i] * 2 == base_grid[j, i])
                {
                    Instantiate(Bsep_v, new Vector3(-0.46875f * (size - 1) + 0.9375f * i, 0.46875f * (size - 2) - 0.9375f * j, -1), Quaternion.identity);
                }
            }
        }
    }

    // Update function
    void Update()
    {
        if (!done)
        {
            if (Input.GetKey("right") && (timer[0] == -15 || timer[0] == 0)) grid_selection[0] = Mathf.Clamp(++grid_selection[0], 0, size - 1);
            if (Input.GetKey("left") && (timer[0] == -15 || timer[0] == 0)) grid_selection[0] = Mathf.Clamp(--grid_selection[0], 0, size - 1);
            if (Input.GetKey("down") && (timer[1] == -15 || timer[1] == 0)) grid_selection[1] = Mathf.Clamp(++grid_selection[1], 0, size - 1);
            if (Input.GetKey("up") && (timer[1] == -15 || timer[1] == 0)) grid_selection[1] = Mathf.Clamp(--grid_selection[1], 0, size - 1);
            if (Input.GetKey("right") || Input.GetKey("left"))
            {
                if (++timer[0] == 3) timer[0] = 0;
            }
            else timer[0] = -15;
            if (Input.GetKey("down") || Input.GetKey("up"))
            {
                if (++timer[1] == 3) timer[1] = 0;
            }
            else timer[1] = -15;
            selection_transform.position = new Vector3(grid_selection[0] * 0.9375f - 0.46875f * (size - 1), -grid_selection[1] * 0.9375f + 0.46875f * (size - 1), -1);
            for (int i = 1; i <= size; i++)
            {
                if (Input.GetKeyDown("" + i))
                {
                    if (user_grid[grid_selection[1], grid_selection[0]].Contains(i)) user_grid[grid_selection[1], grid_selection[0]].Remove(i);
                    else user_grid[grid_selection[1], grid_selection[0]].Add(i);
                    if (isdone()) done = true;
                }
            }
            string biggrid = "";
            for (int i = 0; i < size; i++)
            {
                string bgs = "";
                for (int j = 0; j < size; j++)
                {
                    if (user_grid[i, j].Count == 1)
                    {
                        bgs += user_grid[i, j][0];
                    }
                    else
                    {
                        bgs += " ";
                    }
                }
                biggrid += bgs + "\n";
            }
            BigGrid_text.text = biggrid;
            string smallgrid = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string sgs = "";
                    for (int k = 0; k < size; k++)
                    {
                        for (int l = 1; l <= 3; l++)
                        {
                            if (user_grid[i, k].Contains(l + j * 3) && user_grid[i, k].Count != 1)
                            {
                                sgs += (l + j * 3).ToString();
                            }
                            else
                            {
                                sgs += " ";
                            }
                        }
                    }
                    smallgrid += sgs + "\n";
                }
            }
            SmallGrid_text.text = smallgrid;
        }
    }
}
