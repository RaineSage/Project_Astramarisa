using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;




   public class DialogueEditor : EditorWindow
   {
       private Dialogue m_selectedDialogue = null;
       [NonSerialized]
       private GUIStyle m_nodesStyle = null;
       [NonSerialized]
       private GUIStyle m_playerNodesStyle = null;
       [NonSerialized]                               // NonSeriallized to stop the editor from always spawning a default node at the start
       private DialogueNode m_draggingNode = null;
       [NonSerialized]
       private Vector2 m_draggingOffSet;
       [NonSerialized]
       private DialogueNode m_creatingNode = null;
       [NonSerialized]
       private DialogueNode m_deletingNode = null;
       [NonSerialized]
       private DialogueNode m_linkingParentNode = null;
       private Vector2 m_scrollPosition;
       [NonSerialized]
       private bool m_isDraggingCanvas = false;
       [NonSerialized]
       private Vector2 m_draggingCanvasOffset;

       private const float m_canvasSize = 4000f;
       private const float m_backgroundSize = 50f;

       // Open a Custom Editor Window in Unity
       [MenuItem("Window/Dialogue Editor")]
       public static void ShowEditorWindow()
       {
           GetWindow(typeof(DialogueEditor), false, "DialogueEditor");
       }

       [OnOpenAsset(1)]
       public static bool OnOpenAsset(int _instanceID, int _line)
       {
           // Checks the instance ID and returns the Object that it is. If it is NOT a Dialogue, it returns Null
           Dialogue dialogue = EditorUtility.InstanceIDToObject(_instanceID) as Dialogue;
           if(dialogue != null)
           {
               ShowEditorWindow();
               return true;
           }
           return false;
       }

       private void OnEnable()
       {
           Selection.selectionChanged += OnSelectionChanged;

           m_nodesStyle = new GUIStyle();

           // The string is the name of the Texture you wanna use! Change this later as you see fit!
           m_nodesStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
           m_nodesStyle.normal.textColor = Color.black; // change Text Color (but it doesn't seem to work??)
           m_nodesStyle.padding = new RectOffset(20, 20, 20, 20); // how far away should the border be from the node content
           m_nodesStyle.border = new RectOffset(12, 12, 12, 12); // look of the border, change numbers however it fits better

           m_playerNodesStyle = new GUIStyle();
           m_playerNodesStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
           m_playerNodesStyle.normal.textColor = Color.black; 
           m_playerNodesStyle.padding = new RectOffset(20, 20, 20, 20); 
           m_playerNodesStyle.border = new RectOffset(12, 12, 12, 12);
       }

       private void OnSelectionChanged()
       {
           // check if currently selected Object is a Dialogue
           Dialogue newDialogue = Selection.activeObject as Dialogue;
           if(newDialogue != null)
           {
               // update the variable
               m_selectedDialogue = newDialogue;
               Repaint(); // Triggers OnGui function
           }
       }

       private void OnGUI()
       {

           if(m_selectedDialogue == null)
           {
               EditorGUILayout.LabelField("No Dialogue Selected");
           }
           else
           {
               ProcessEvents();

               m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition); // Add Scrollfield

               Rect canvas = GUILayoutUtility.GetRect(m_canvasSize, m_canvasSize);
               Texture2D backgroundTex = Resources.Load("background") as Texture2D;
               Rect texCoords = new Rect(0, 0, m_canvasSize / m_backgroundSize, m_canvasSize / m_backgroundSize);
               GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

               // in two seperate foreach loops so that Node Lines go underneath the nodes itself in case of overlapping (looks prettier)
               foreach (DialogueNode node in m_selectedDialogue.GetAllNodes())
               {
                   DrawConnections(node);
               }

               foreach (DialogueNode node in m_selectedDialogue.GetAllNodes())
               {
                   DrawNode(node);
               }

               EditorGUILayout.EndScrollView();

               if(m_creatingNode != null)
               {
                   m_selectedDialogue.CreateNode(m_creatingNode);
                   m_creatingNode = null;
               }
               if(m_deletingNode != null)
               {
                   m_selectedDialogue.DeleteNode(m_deletingNode);
                   m_deletingNode = null;
               }
           }
           
       }

       private void ProcessEvents()
       {
           // track if left Mouse Button was pressen, while it's standing still!
           if(Event.current.type == EventType.MouseDown && m_draggingNode == null)
           {
               m_draggingNode = GetNodeAtPoint(Event.current.mousePosition + m_scrollPosition);

               if(m_draggingNode != null)
               {
                  m_draggingOffSet = m_draggingNode.GetRect().position - Event.current.mousePosition;
                  Selection.activeObject = m_draggingNode; // show's only the Node in the Inspector
               }
               else
               {
                   // record dragOffset and dragging
                   m_isDraggingCanvas = true;
                   m_draggingCanvasOffset = Event.current.mousePosition + m_scrollPosition;
                   Selection.activeObject = m_selectedDialogue; // shows the whole Dialogue-Asset in the Inspector
               }
           }
           else if(Event.current.type == EventType.MouseDrag && m_draggingNode != null)
           {
               m_draggingNode.SetPosition(Event.current.mousePosition + m_draggingOffSet);
               GUI.changed = true; // change of Input data
           }
           else if (Event.current.type == EventType.MouseDrag && m_isDraggingCanvas)
           {
               // Update scrollPosition
               m_scrollPosition = m_draggingCanvasOffset - Event.current.mousePosition;

               GUI.changed = true; // change of Input data (redraw)
           }
           else if(Event.current.type == EventType.MouseUp && m_draggingNode != null)
           {
               m_draggingNode = null;
           }
           else if (Event.current.type == EventType.MouseUp && m_isDraggingCanvas)
           {
               m_isDraggingCanvas = false;
           }
       }

       private void DrawNode(DialogueNode _node)
       {
           GUIStyle style = m_nodesStyle;
           if(_node.IsPlayerSpeaking())
           {
               style = m_playerNodesStyle;
           }

           // Draw a Rectangle with a certain position and size around the following content
           GUILayout.BeginArea(_node.GetRect(), style); // takes the position and the style

           // EditorStyle can be switched, feel free to experiment
           _node.SetText(EditorGUILayout.TextField(_node.GetText()));

           GUILayout.BeginHorizontal();

           if (GUILayout.Button("Delete"))
           {
               m_deletingNode = _node;
           }

           DrawLinkButtons(_node);

           if (GUILayout.Button("Add"))
           {
               m_creatingNode = _node;
           }

           GUILayout.EndHorizontal();

           // End the Rectangle after the content above
           GUILayout.EndArea();
       }

       private void DrawLinkButtons(DialogueNode _node)
       {
           if (m_linkingParentNode == null)
           {
               if (GUILayout.Button("link"))
               {
                   m_linkingParentNode = _node;
               }
           }
           else if(m_linkingParentNode == _node)
           {
               if (GUILayout.Button("cancel"))
               {
                   m_linkingParentNode = null;
               }
           }
           else if(m_linkingParentNode.GetChildren().Contains(_node.name))
           {
               if (GUILayout.Button("unlink"))
               {
                   m_linkingParentNode.RemoveChild(_node.name);
                   m_linkingParentNode = null;
               }
           }
           else
           {
               if (GUILayout.Button("child"))
               {
                   Undo.RecordObject(m_selectedDialogue, "Add Dialogue Link");
                   m_linkingParentNode.AddChild(_node.name);
                   m_linkingParentNode = null;
               }
           }
       }

       private void DrawConnections(DialogueNode _node)
       {
           Vector3 startPosition = new Vector2(_node.GetRect().xMax, _node.GetRect().center.y);
           
           foreach (DialogueNode childNode in m_selectedDialogue.GetAllChildren(_node))
           {
               Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
               Vector3 controlPointOffset = endPosition - startPosition;
               controlPointOffset.y = 0;
               controlPointOffset.x *= 0.8f; // float can get changed, based on your liking! smoothes out the curves for the node lines
               Handles.DrawBezier(startPosition, endPosition, 
                                  startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 4f);
           }
       }

       // check if Mouse is hovering over any of the Nodes in the Editor
       private DialogueNode GetNodeAtPoint(Vector2 _point)
       {
           DialogueNode foundNode = null;
           foreach (DialogueNode node in m_selectedDialogue.GetAllNodes())
           {
               if(node.GetRect().Contains(_point))
               {
                   // takes the top Node (in case they're overlapping)
                   foundNode = node;
               }
           }
           return foundNode;
       }
   }
