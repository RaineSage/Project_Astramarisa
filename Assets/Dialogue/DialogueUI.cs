using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    PlayerConversant m_playerConversant;
    [SerializeField]
    private TextMeshProUGUI m_AIText;
    [SerializeField]
    private Button m_nextButton;
    [SerializeField]
    private GameObject m_AIResponse;
    [SerializeField]
    private Transform m_choiceRoot;
    [SerializeField]
    private GameObject m_choicePrefab;

    // Start is called before the first frame update
    void Start()
    {
        m_playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        m_playerConversant.onConversationUpdated += UpdateUI;

        m_nextButton.onClick.AddListener(Next);

        UpdateUI();
    }

    private void Next()
    {
        m_playerConversant.Next();
    }

    void UpdateUI()
    {
        gameObject.SetActive(m_playerConversant.IsActive());

        if(!m_playerConversant.IsActive())
        {
            return;
        }

        // if you're choosing, show the Choice buttons!
        m_choiceRoot.gameObject.SetActive(m_playerConversant.IsChoosing());

        if(m_playerConversant.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            m_AIText.text = m_playerConversant.GetText();
            m_nextButton.gameObject.SetActive(m_playerConversant.HasNext());
        }

    }

    private void BuildChoiceList()
    {
        foreach (Transform item in m_choiceRoot)
        {
            Destroy(item.gameObject);
        }

        foreach (DialogueNode choice in m_playerConversant.GetChoices())
        {
            GameObject choiceInstance = Instantiate(m_choicePrefab, m_choiceRoot);
            var textComponent = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = choice.GetText();

            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => // Lambda Funktion || if the button is clicked, call the function for that specific Button
            {
                m_playerConversant.SelectChoice(choice);
            });
        }
    }
}
