using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    [SerializeField]
    private GameObject textBox;
    [SerializeField]
    private Text theText;
    [SerializeField]
    private TextAsset textfile;
    [SerializeField]
    private string[] textLines;
    [SerializeField]
    private int currentLine;
    [SerializeField]
    private int endAtLine;
    [SerializeField]
    private Player player;

    private bool thisIsActive;

    // Use this for initialization
    void Start()
    {
        textBox.SetActive(false);
        player = FindObjectOfType<Player>();

        if (textfile != null)
        {
            textLines = (textfile.text.Split('\n'));
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }

    void Update()
    {
        if (!thisIsActive)
            return;
        theText.text = textLines[currentLine];
        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }

        if(currentLine > endAtLine)
        {
            textBox.SetActive(false);
            thisIsActive = false;
            Destroy(gameObject);
        }
    }

    public void enableTextBox()
    {
        currentLine = 0;
        textBox.SetActive(true);
        thisIsActive = true;
    }
}
