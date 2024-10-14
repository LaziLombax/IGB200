using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DialogueController : MonoBehaviour
{
    public CanvasGroup dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueTextUI;
    [SerializeField] private TextMeshProUGUI dialogueTextBackUI;
    [SerializeField] private float typeSpeed;
    [SerializeField] private float wobbleSpeed = 1f;

    private Queue<string> paragraphs = new Queue<string>();

    private bool conversationEnded;
    private bool isTyping;

    private string p;

    private const string HTML_ALPHA = "<color=#00000000>";

    private const float MAX_TYPE_TIME = 0.1f;

    private Coroutine typeDialogueCoroutine;

    private Mesh mesh;
    private List<int> wordIndexes;
    private List<int> wordLengths;
    Vector3[] vertices = null;
    private void Start()
    {
    }

    private void LateUpdate()
    {
        if(!conversationEnded && dialogueBox.gameObject.activeSelf)
        {
            dialogueTextUI.ForceMeshUpdate();
            mesh = dialogueTextUI.mesh;
            vertices = mesh.vertices;
            #region TextEffect

            wordIndexes = new List<int> { 0 };
            wordLengths = new List<int>();

            string s = p;
            for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
            {
                wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
                wordIndexes.Add(index + 1);
            }
            wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
            #endregion

            for (int w = 0; w < wordIndexes.Count; w++)
            {
                int wordIndex = wordIndexes[w];
                Vector3 offset = Wobble(Time.time + w);

                for (int i = 0; i < wordLengths[w]; i++)
                {
                    TMP_CharacterInfo c = dialogueTextUI.textInfo.characterInfo[wordIndex + i];

                    int index = c.vertexIndex;

                    vertices[index] += offset;
                    vertices[index + 1] += offset;
                    vertices[index + 2] += offset;
                    vertices[index + 3] += offset;
                }
            }

            mesh.vertices = vertices;
            dialogueTextUI.canvasRenderer.SetMesh(mesh);
        }
    }
    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3f * wobbleSpeed), Mathf.Cos(time * 3f * wobbleSpeed));
    }
    public void DisplayNextParagraph(GameData.DialogueShelldon dialogueText)
    {
        if(paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                //start
                StartConversation(dialogueText);
            } else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }
        if (!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }
        else
        {
            FinishEarly();
        }

        dialogueTextUI.text = p;

        if (paragraphs.Count == 0) 
        {
            conversationEnded = true;
        }
    }

    private void StartConversation(GameData.DialogueShelldon dialogueText)
    {
        if (!dialogueBox.gameObject.activeSelf)
        {
            GameHandler.Instance.timerOn = false;
            dialogueBox.gameObject.SetActive(true);
            CanvasGroup dialogueCanvasGroup = dialogueBox.gameObject.GetComponent<CanvasGroup>();
            dialogueCanvasGroup.alpha = 0;
            dialogueCanvasGroup.DOFade(1, 0.5f);
        }

        for (int i = 0; i < dialogueText.dialogueList.Count; i++)
        {
            paragraphs.Enqueue(dialogueText.dialogueList[i]);
        }
    }
    private void EndConversation()
    {
        GameHandler.Instance.timerOn = true;
        conversationEnded = false;
        if (dialogueBox.gameObject.activeSelf)
        {
            CanvasGroup dialogueCanvasGroup = dialogueBox.gameObject.GetComponent<CanvasGroup>();
            dialogueCanvasGroup.alpha = 0;
            dialogueCanvasGroup.DOFade(0, 0.5f).OnComplete(() => dialogueBox.gameObject.SetActive(false));
        }
    }
    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;
        int maxVisibleChars = 0;

        dialogueTextUI.text = p;
        dialogueTextUI.maxVisibleCharacters = maxVisibleChars;

        string originalText = p;
        //string displayedText = "";
        //int alphaIndex = 0;
        foreach (char c in p.ToCharArray())
        {
            maxVisibleChars++;
            dialogueTextUI.maxVisibleCharacters = maxVisibleChars;
            
            //alphaIndex++;
            //dialogueTextUI.text = originalText;

            // = dialogueTextUI.text.Insert(alphaIndex, HTML_ALPHA);
            
            //dialogueTextUI.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishEarly()
    {
        StopCoroutine(typeDialogueCoroutine);

        //dialogueTextUI.text = p;
        dialogueTextUI.maxVisibleCharacters = p.Length;

        isTyping = false;
    }
}
