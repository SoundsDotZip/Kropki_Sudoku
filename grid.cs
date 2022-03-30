using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class grid : MonoBehaviour
{
    static void intShuffle(int[] a)
    {
        for (int i = a.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i);
            int temp = a[i];
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }
    static bool finished()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (usernumbers[i, j].Count != 1 || basenumbers[i, j]!=usernumbers[i, j][0])
                {
                    return false;
                }
            }
        }
        return true;
    }
    static bool error_check_row(int r, int v)
    {
        List<int> row = new List<int>();
        for (int i = 0; i < size; i++)
        {
            if (usernumbers[r, i].Count == 1)
            {
                row.Add(usernumbers[r, i][0]);
            }
        }
        int amount = 0;
        foreach (int item in row)
        {
            if (item == v) amount++;
        }
        return (amount > 1);
    }
    static bool error_check_col(int c, int v)
    {
        List<int> col = new List<int>();
        for (int i = 0; i < size; i++)
        {
            if (usernumbers[i, c].Count == 1)
            {
                col.Add(usernumbers[i, c][0]);
            }
        }
        int amount = 0;
        foreach (int item in col)
        {
            if (item == v) amount++;
        }
        return (amount > 1);
    }
    static bool error_check_separators(int y, int x)
    {
        int[] x_directions = new int[] { 0, 0, -1, 1 };
        int[] y_directions = new int[] { -1, 1, 0, 0 };
        bool error = false;
        if (usernumbers[y, x].Count == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (usernumbers[y + y_directions[i], x + x_directions[i]].Count == 1)
                    {
                        bool v1 = basenumbers[y, x]!=1 && basenumbers[y + y_directions[i], x + x_directions[i]]!=1 && Mathf.Abs(basenumbers[y, x] - basenumbers[y + y_directions[i], x + x_directions[i]]) == 1;
                        bool v2 = Mathf.Max((float)basenumbers[y, x], (float)basenumbers[y + y_directions[i], x + x_directions[i]]) / Mathf.Min((float)basenumbers[y, x], (float)basenumbers[y + y_directions[i], x + x_directions[i]]) == 2f;
                        bool v3 = Mathf.Abs(usernumbers[y, x][0] - usernumbers[y + y_directions[i], x + x_directions[i]][0]) == 1;
                        bool v4 = Mathf.Max((float)usernumbers[y, x][0], (float)usernumbers[y + y_directions[i], x + x_directions[i]][0]) / Mathf.Min((float)usernumbers[y, x][0], (float)usernumbers[y + y_directions[i], x + x_directions[i]][0]) == 2f;
                        if ((v2 && !v4) || (v1 && !v3) || (!v2 && !v1 && (v3 || v4))) error = true;
                    }
                } catch { }
            }
        }
        return error;
    }
    static string bo_string(bool condition, string iftrue, string iffalse) { if (condition) return iftrue; return iffalse; }
    static int bo_int(bool condition, int iftrue, int iffalse) { if (condition) return iftrue; return iffalse; }
    static void GenerateGrid(GameObject sep1, GameObject sep2, GameObject sep3, GameObject sep4, SpriteRenderer sprr, Sprite[] spr)
    {
        int[] random_num = new int[size];
        for (int i = 0; i < size; i++)
        {
            random_num[i] = i + 1;
        }
        intShuffle(random_num);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                basenumbers[i, j] = random_num[(j + i + 1) % size];
            }
        }
        for (int i = 0; i < size - 1; i++)
        {
            int rnd = Random.Range(i + 1, size);
            for (int j = 0; j < size; j++)
            {
                int temp = basenumbers[i, j];
                basenumbers[i, j] = basenumbers[rnd, j];
                basenumbers[rnd, j] = temp;
            }
        }
        for (int i = 0; i < size - 1; i++)
        {
            int rnd = Random.Range(i + 1, size);
            for (int j = 0; j < size; j++)
            {
                int temp = basenumbers[j, i];
                basenumbers[j, i] = basenumbers[j, rnd];
                basenumbers[j, rnd] = temp;
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                usernumbers[i, j] = new List<int>();
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size - 1; j++)
            {
                if (basenumbers[i, j] + 1 == basenumbers[i, j + 1] || basenumbers[i, j] - 1 == basenumbers[i, j + 1])
                {
                    Instantiate(sep1, new Vector3(-0.9375f / 2 * (size - 2) + 0.9375f * j, 0.46875f * (size - 1) - 0.9375f * i, -1), Quaternion.identity);
                }
                if (basenumbers[i, j] * 2 == basenumbers[i, j + 1] || basenumbers[i, j + 1] * 2 == basenumbers[i, j])
                {
                    Instantiate(sep2, new Vector3(-0.9375f / 2 * (size - 2) + 0.9375f * j, 0.46875f * (size - 1) - 0.9375f * i, -1), Quaternion.identity);
                }
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size - 1; j++)
            {
                if (basenumbers[j, i] + 1 == basenumbers[j + 1, i] || basenumbers[j, i] - 1 == basenumbers[j + 1, i])
                {
                    Instantiate(sep3, new Vector3(-0.46875f * (size - 1) + 0.9375f * i, 0.9375f / 2 * (size - 2) - 0.9375f * j, -1), Quaternion.identity);
                }
                if (basenumbers[j, i] * 2 == basenumbers[j + 1, i] || basenumbers[j + 1, i] * 2 == basenumbers[j, i])
                {
                    Instantiate(sep4, new Vector3(-0.46875f * (size - 1) + 0.9375f * i, 0.9375f / 2 * (size - 2) - 0.9375f * j, -1), Quaternion.identity);
                }
            }
        }
        sprr.sprite = spr[size - 4];
    }
    static void UpdateText(TMP_Text Main, TMP_Text PencilMark)
    {
        string main = "";
        for (int i = 0; i < size; i++)
        {
            string s = "";
            for (int j = 0; j < size; j++)
            {
                if (usernumbers[i, j].Count == 1)
                {
                    if (error_check_row(i, usernumbers[i, j][0]) || error_check_col(j, usernumbers[i, j][0]) || error_check_separators(i, j))
                        s += "<color=#b13e53>";
                    s += usernumbers[i, j][0].ToString();
                    if (error_check_row(i, usernumbers[i, j][0]) || error_check_col(j, usernumbers[i, j][0]) || error_check_separators(i, j))
                        s += "</color>";
                }
                else
                {
                    s += " ";
                }
            }
            main += (s + "\n");
        }
        Main.text = main;
        main = "";
        for (int i = 0; i < size * 3; i++)
        {
            string s = "";
            for (int j = 0; j < size; j++)
            {
                for (int k = i % 3 * 3 + 1; k <= i % 3 * 3 + 3; k++)
                {
                    if (usernumbers[i / 3, j].Contains(k) && usernumbers[i / 3, j].Count != 1)
                    {
                        s += k.ToString();
                    }
                    else
                    {
                        s += " ";
                    }
                }
            }
            main += s + "\n";
        }
        PencilMark.text = main;
    }
    public Transform thingy;
    public SpriteRenderer bongus;
    public Sprite[] sprites = new Sprite[6];
    public Transform seltrans;
    public TMP_Text Main;
    public TMP_Text PencilMark;
    public TMP_Text Finished_Text;
    public GameObject Bsep_v;
    public GameObject Wsep_v;
    public GameObject Bsep_h;
    public GameObject Wsep_h;
    static int size = menu.msize;
    static int[] grid_selection = new int[2] { 0, 0 };
    int timer = -15;

    static int[,] basenumbers = new int[size, size];
    static List<int>[,] usernumbers = new List<int>[size, size];

    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Main.transform.position = new Vector3((9 - size) * 0.46875f, 0, -2);
        PencilMark.transform.position = new Vector3((9 - size) * 0.46875f + 0.04f, 0, -2);
        Finished_Text.enabled = false;
        GenerateGrid(Wsep_v, Bsep_v, Wsep_h, Bsep_h, bongus, sprites);
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            if (Input.GetKey("right") && (timer == 0 || timer == -15)) { grid_selection[0] = Mathf.Clamp(++grid_selection[0], 0, size - 1); }
            if (Input.GetKey("left") && (timer == 0 || timer == -15)) { grid_selection[0] = Mathf.Clamp(--grid_selection[0], 0, size - 1); }
            if (Input.GetKey("down") && (timer == 0 || timer == -15)) { grid_selection[1] = Mathf.Clamp(++grid_selection[1], 0, size - 1); }
            if (Input.GetKey("up") && (timer == 0 || timer == -15)) { grid_selection[1] = Mathf.Clamp(--grid_selection[1], 0, size - 1); }
            if (Input.GetKey("right") || Input.GetKey("left") || Input.GetKey("up") || Input.GetKey("down"))
            {
                timer += 1;
                if (timer == 3)
                {
                    timer = 0;
                }
            }
            else
            {
                timer = -15;
            }
            for (int i = 1; i <= size; i++)
            {
                if (Input.GetKeyDown("" + i))
                {
                    if (usernumbers[grid_selection[1], grid_selection[0]].Contains(i))
                    {
                        usernumbers[grid_selection[1], grid_selection[0]].Remove(i);
                    }
                    else
                    {
                        usernumbers[grid_selection[1], grid_selection[0]].Add(i);
                    }
                    if (finished())
                    {
                        Finished_Text.enabled = true;
                        done = true;
                    }
                }
            }
            if (Input.GetKeyDown("delete")) usernumbers[grid_selection[1], grid_selection[0]] = new List<int>();
            UpdateText(Main, PencilMark);
            seltrans.position = new Vector3((grid_selection[0] * 0.9375f) - 0.46875f * (size - 1), (-grid_selection[1] * 0.9375f) + 0.46875f * (size - 1), -1);
        }
    }
}