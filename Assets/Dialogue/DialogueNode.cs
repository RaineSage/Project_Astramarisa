using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;



public class DialogueNode : ScriptableObject
{
    [SerializeField]
    private bool m_isPlayerSpeaking = false; // if you need more than one character to talk to, use an Enum!!
    
    [TextArea (5, 15)]
    [SerializeField]
    private string m_text;
    [SerializeField]
    private List<string> m_children = new List<string>();
    [SerializeField]
    private Rect m_rect = new Rect(0, 0, 200, 100);
    [SerializeField]
    private string m_onEnterAction;
    [SerializeField]
    private string m_onExitAction;


    public Rect GetRect ()
    {
        return m_rect;
    }

    public string GetText()
    {
        return m_text;
    }

    public List<string> GetChildren()
    {
        return m_children;
    }
    public bool IsPlayerSpeaking()
    {
        return m_isPlayerSpeaking;
    }

    public string GetOnEnterAction()
    {
        return m_onEnterAction;
    }
    public string GetOnExitAction()
    {
        return m_onExitAction;
    }

#if UNITY_EDITOR
    public void SetPosition (Vector2 _newPosition)
    {
       Undo.RecordObject(this, "Move Dialogue Node");

       m_rect.position = _newPosition;
       EditorUtility.SetDirty(this);
    }

    public void SetText(string _newText)
    {
        if(_newText != m_text)
        {
            Undo.RecordObject(this, "Update Dialogue Text");

            m_text = _newText;
            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild (string _childID)
    {
        Undo.RecordObject(this, "Add Dialogue Link");

        m_children.Add(_childID);
        EditorUtility.SetDirty(this);
    }

    public void RemoveChild (string _childID)
    {
        Undo.RecordObject(this, "Remove Dialogue Link");

        m_children.Remove(_childID);
        EditorUtility.SetDirty(this);
    }

    public void SetPlayerSpeaking(bool _newIsPlayerSpeaking)
    {
        Undo.RecordObject(this, "Change Dialogue Speaker");
        m_isPlayerSpeaking = _newIsPlayerSpeaking;
        EditorUtility.SetDirty(this);
    }


#endif

}

