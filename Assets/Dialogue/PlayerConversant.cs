using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerConversant : MonoBehaviour
{
    [SerializeField]
    private Dialogue m_hardDialog;
    [SerializeField]
    private Dialogue m_easyDialog;
    private Dialogue m_currentDialogue;
    DialogueNode m_currentNode = null;
    private bool m_isChoosing = false;


    public event Action onConversationUpdated;

    private void Awake()
    {
       
    }

    public void StartDialog(Dialogue _newDialog)
    {
        m_currentDialogue = _newDialog;
        m_currentNode = m_currentDialogue.GetRootNode();
        TriggerEnterAction();
        onConversationUpdated();
    }

    public bool IsActive()
    {
        return m_currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return m_isChoosing;
    }

    public string GetText()
    {
         if(m_currentNode == null)
         {
            return "";
         }

        return m_currentNode.GetText();
    }
   
    public IEnumerable<DialogueNode> GetChoices()
    {
        return m_currentDialogue.GetPlayerChildren(m_currentNode);
    }

    public void SelectChoice(DialogueNode _chosenNode)
    {
        m_currentNode = _chosenNode;
        TriggerEnterAction();
        m_isChoosing = false;
        Next();
    }

    public void Next()
    {
        int numPlayerResponses = m_currentDialogue.GetPlayerChildren(m_currentNode).Count();
        if(numPlayerResponses > 0)
        {
            m_isChoosing = true;
            TriggerExitAction();
            onConversationUpdated();

            return;
        }

        DialogueNode[] children = m_currentDialogue.GetAIChildren(m_currentNode).ToArray();
        int randomIndex = UnityEngine.Random.Range(0, children.Count());
        TriggerExitAction();
        m_currentNode = children[randomIndex];
        TriggerEnterAction();
        onConversationUpdated();
    }

    public bool HasNext()
    {
        // if we have children, the Next Button appears, if not, screw it

        return m_currentDialogue.GetAllChildren(m_currentNode).Count() > 0;
    }

    private void TriggerEnterAction()
    {
        if(m_currentNode != null && m_currentNode.GetOnEnterAction() != "")
        {
            Debug.Log(m_currentNode.GetOnEnterAction());
        }
    }

    private void TriggerExitAction()
    {
        if (m_currentNode != null && m_currentNode.GetOnExitAction() != "")
        {
            Debug.Log(m_currentNode.GetOnExitAction());
        }
    }
}

