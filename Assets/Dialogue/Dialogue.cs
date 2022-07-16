using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<DialogueNode> m_nodes = new List<DialogueNode>();
    [SerializeField]
    private Vector2 m_newNodeOffset = new Vector2(250, 0);

    private Dictionary<string, DialogueNode> m_nodeLookup = new Dictionary<string, DialogueNode>();


    private void OnValidate()
    {
        m_nodeLookup.Clear();
        foreach(DialogueNode node in GetAllNodes())
        {
            m_nodeLookup[node.name] = node;
        }
    }

    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return m_nodes;
    }

    public DialogueNode GetRootNode()
    {
        return m_nodes[0];
    }

    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode _parentNode)
    {
        foreach(string childID in _parentNode.GetChildren())
        {
            if(m_nodeLookup.ContainsKey(childID)) // Check if the Key exists first
            {
                yield return m_nodeLookup[childID];
            }
        }
    }

    public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode m_currentNode)
    {
        foreach(DialogueNode node in GetAllChildren(m_currentNode))
        {
            if (node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }

    public IEnumerable<DialogueNode> GetAIChildren(DialogueNode m_currentNode)
    {
        foreach (DialogueNode node in GetAllChildren(m_currentNode))
        {
            if(!node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }

#if UNITY_EDITOR
    public void CreateNode(DialogueNode _parent)
    {
        DialogueNode newNode = MakeNode(_parent);
        if(AssetDatabase.GetAssetPath(this) != "")
        {
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
        }
        Undo.RecordObject(this, "Added Dialogue Node"); // records the action, so you can undo it again

        AddNode(newNode);
    }

       

    public void DeleteNode(DialogueNode _nodeToDelete)
    {
        Undo.RecordObject(this, "Deleted Dialogue Node");

        m_nodes.Remove(_nodeToDelete);
        OnValidate();       // need to be called to update the Lookup (the lines)

        DeleteAllChildren(_nodeToDelete);
        
        Undo.DestroyObjectImmediate(_nodeToDelete);
    }

    private DialogueNode MakeNode(DialogueNode _parent)
    {
        DialogueNode newNode = CreateInstance<DialogueNode>();
        newNode.name = Guid.NewGuid().ToString();

        if (_parent != null)
        {
            _parent.AddChild(newNode.name);
            newNode.SetPlayerSpeaking(!_parent.IsPlayerSpeaking());
            newNode.SetPosition(_parent.GetRect().position + m_newNodeOffset);

        }

        return newNode;
    }
    private void AddNode(DialogueNode newNode)
    {
        m_nodes.Add(newNode);
        OnValidate();         // need to be called to update the Lookup (the lines)
    }

    private void DeleteAllChildren(DialogueNode _nodeToDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
        {
            node.RemoveChild(_nodeToDelete.name);  // deletes all children nodes, in case the node had one
        }
    }

#endif

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
         if (m_nodes.Count == 0)
         {
             DialogueNode newNode = MakeNode(null);

             AddNode(newNode);
         }

         // happens when you save to the hard drive
         if ( AssetDatabase.GetAssetPath(this) != "")
         {
             foreach(DialogueNode node in GetAllNodes())
             {
                 if(AssetDatabase.GetAssetPath(node) == "")
                 {
                     AssetDatabase.AddObjectToAsset(node, this); // Nodes will be Sub-Assets for the actual Dialogue object
                 }
             }
         }
#endif
    }

     public void OnAfterDeserialize()
     {
         // only used when you load something from the hard drive 
         // no need to use this
     }
}

