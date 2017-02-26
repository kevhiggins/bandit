/**
 * @file WaypointClusterEditor.cs
 * @author Eric Mourin
 * @date 12 Aug 2015
 * @brief Custom editor to handle waypoint placement.
 */

using System;
using UnityEngine;
using UnityEditor;
using Bandit;

[CustomEditor(typeof(WaypointCluster))]
public class WaypointClusterEditor : Editor
{
    /**< Editing state, changed with buttons */

    enum EditingState
    {
        None, /**< Not currently editing */
        Adding, /**< Adding principal link arrows (red) */
        Removing /**< Removing links between waypoints */
    };

    private WaypointCluster clusterobject; //Object being edited.
    private bool dragging = false; //True if the user is dragging the mouse.
    private WayPoint waypointClicked; //waypoint clicked, if any.
    private WayPoint waypointDestiny; //waypoint the mouse is over, if any.
    private EditingState state = EditingState.None; //Current editing state.
    private string currentState = "Not editing."; //Text shown in the inspector label.
    private GameObject previewSphere; //The small white preview sphere.
    private Plane plane;
    private MouseRayHit mouseRayHit = new MouseRayHit();

    //private static GameObject cluster = null;		//The cluster gameObject where all waypoints are packed, only one.
    int hash = "ClusterControl".GetHashCode(); //hash used to keep the focus on the current object

    void OnEnable()
    {
        clusterobject = (WaypointCluster) target;
        plane = new Plane(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0));
    }

    /* 	GUI showed on the inspector, default.
	* 	Some buttons were added to change the editing state and a label to show the current state.
	*/

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Add waypoints / edges"))
        {
            state = EditingState.Adding;
            currentState = "Adding principal links.";
        }
        else if (GUILayout.Button("Remove edges"))
        {
            state = EditingState.Removing;
            currentState = "Removing links";
        }
        else if (GUILayout.Button("Turn off edit mode"))
        {
            state = EditingState.None;
            currentState = "Not editing.";
        }
        EditorGUILayout.LabelField("Current state: " + currentState);
        /*if(GUILayout.Button("LISTALL")){ KEPT FOR DEBUG PUPOSES
		cluster = GameObject.Find("Waypoint Cluster");
		clusterobject.sources =new List<WayPoint>();
		clusterobject.sinks = new List<WayPoint>();
		clusterobject.waypoints = new List<WayPoint>();
		foreach(Transform child in cluster.transform) {
		if (child.gameObject.GetComponent<WayPointSource>() != null) clusterobject.sources.Add(child.gameObject.GetComponent<WayPointSource>());
		if (child.gameObject.GetComponent<WayPointSink>() != null) clusterobject.sinks.Add(child.gameObject.GetComponent<WayPointSink>());
		child.gameObject.name = clusterobject.waypoints.Count.ToString();
		child.gameObject.GetComponent<WayPoint>().setParent(clusterobject);
		clusterobject.waypoints.Add(child.gameObject.GetComponent<WayPoint>());
		}
		}*/
    }

    void OnSceneGUI()
    {
        // Clear the previously calculated mouseRayHit, since it's a new "frame".
        mouseRayHit.Clear();

        Event current = Event.current;
        //note: current.button == 0 works only the frame you press it.
        if (state != EditingState.None)
        {
            if (dragging) MovePreview();
            int ID = GUIUtility.GetControlID(hash, FocusType.Passive);

            switch (current.type)
            {
                //For focus purposes
                case EventType.Layout:
                    HandleUtility.AddDefaultControl(ID);
                    break;

                //White sphere creation and dragging = true
                case EventType.mouseDown:
                    if (current.button == 1) //ignore right clicks, use them to cancel the dragging
                    {
                        dragging = false;
                        if (previewSphere != null) DestroyImmediate(previewSphere);
                    }
                    else CreatePreview();
                    break;

                //Point creation if necessary
                case EventType.mouseUp:

                    if (current.button == 1) break;
                    if (!dragging) break;

                    dragging = false;
                    if (previewSphere != null) DestroyImmediate(previewSphere);

                    var mousePoint = GetMousePoint();


                    if (state == EditingState.Adding) CreateLink(mousePoint);
                    else if (state == EditingState.Removing) RemoveLink(mousePoint);

                    waypointClicked = null;
                    waypointDestiny = null;
                    break;
            }
        }
    }

    /**
     * Returns the point where the mouse ray intersects are node drawing canvas.
     */

    private Vector3 GetMousePoint()
    {
        var worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float rayDistance;
        if (plane.Raycast(worldRay, out rayDistance))
        {
            return worldRay.GetPoint(rayDistance);
        }
        throw new Exception("Mouse ray does not intersect plane. Camera might be pointed in the opposite direction.");
    }


    /**************************WAYPOINT AND LINK CREATION***********************************/

    /** Finds the cluster object.
	*	Adds the desired waypoint, depending on the current state.
	*	Places it in the hitpoint, as a cluster object child.
	*	Gives it a unique number.
	*	Adds it to the cluster object lists.
	*	If it is source/sink is added to the pertinent special list too.
	*	The cluster object is referenced in the waypoint.
	*/

    private WayPoint CreatePoint(Vector3 point)
    {       
        if (clusterobject.cluster == null)
        {
            clusterobject.cluster = clusterobject.gameObject;
        }       

        GameObject waypointAux;
        Undo.RecordObject(clusterobject, "Created waypoint");
        waypointAux = Resources.Load("Waypoint") as GameObject;
        GameObject waypointInstance = Instantiate(waypointAux) as GameObject;
        waypointInstance.transform.position = point;
        waypointInstance.transform.parent = clusterobject.cluster.transform;
        waypointInstance.name = clusterobject.waypoints.Count.ToString();
        clusterobject.waypoints.Add(waypointInstance.GetComponent<WayPoint>());
        waypointInstance.GetComponent<WayPoint>().setParent(clusterobject);
        Undo.RegisterCreatedObjectUndo(waypointInstance, "Created waypoint");
        return waypointInstance.GetComponent<WayPoint>();
    }

    /**	a)Creates a link between two existing waypoints.
	*	b)Creates and links a new waypoint to an existing waypoint.
	*	c)Creates a new waypoiny with no links.
	*/

    private void CreateLink(Vector3 point)
    {
        Debug.Log(waypointClicked);
        Debug.Log(waypointDestiny);

        // If we started by clicking a waypoint, and the end mouse position on release was a different waypoint, then link them together.
        if (waypointClicked != null && waypointDestiny != null && waypointClicked != waypointDestiny)
        {
            Link(waypointClicked, waypointDestiny);
        }
        // If we started by clicking a waypoint, and did not select another waypoint to link, then create a new waypoint instead.
        else if (waypointClicked != null && waypointDestiny == null)
        {
            Link(waypointClicked, CreatePoint(point));
        }
        // If we did not click an initial waypoint, and there is not destination waypoint, then create a new waypoint.
        else if (waypointClicked == null && waypointDestiny == null)
        {
            CreatePoint(point);
        }
    }

    /**	a)Removes a link between two existing waypoints.
    */

    private void RemoveLink(Vector3 point)
    {
        if (waypointClicked != null && waypointDestiny != null && waypointClicked != waypointDestiny)
            UnLink(waypointClicked, waypointDestiny);
    }

    /* Creates a link between source and destiny */

    private static void Link(WayPoint source, WayPoint destiny)
    {
        Undo.RecordObject(source, "waypointadd");
        Undo.RecordObject(destiny, "waypointadd");
        source.addOutWayPoint(destiny);
        destiny.addInWayPoint(source);
    }

    /* Removes a link between source and destiny */

    private void UnLink(WayPoint source, WayPoint destiny)
    {
        Undo.RecordObject(source, "waypointremove");
        Undo.RecordObject(destiny, "waypointremove");
        source.removeOutWayPoint(destiny);
        destiny.removeInWayPoint(source);
    }


    /**************************PREVIEW CREATIONG/MOVEMENT***********************************/

    /**	Creates the white sphere preview.
	*	If a waypoint is clicked then waypointClicked is set.
	*/

    private void CreatePreview()
    {
        dragging = true;
        previewSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        DestroyImmediate(previewSphere.GetComponent<SphereCollider>());

        var mousePoint = GetMousePoint();
        previewSphere.transform.position = mousePoint;
        if (mouseRayHit.HasRayHit())
        {
            waypointClicked = mouseRayHit.GetRayHit().transform.gameObject.GetComponent<WayPoint>();
        }
    }

    /**	Moves the white sphere preview.
	*	If the previewsphere is null it is created again.
	*	If a waypointClicked is set then an arrow is created.
	*	If there is hovering over another wayopint waypointDestiny is set and pointed with the preview arrow.
	*	If there was no object hitted then the white sphete preview is destroyed.
	*/

    private void MovePreview()
    {
        var mousePoint = GetMousePoint();

        if (previewSphere == null)
        {
            previewSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            DestroyImmediate(previewSphere.GetComponent<SphereCollider>());
        }
        previewSphere.transform.position = mousePoint;

        if (mouseRayHit.HasRayHit())
        {
            waypointDestiny = mouseRayHit.GetRayHit().transform.gameObject.GetComponent<WayPoint>();
            if (waypointDestiny != null) waypointDestiny.setColor(Color.green);
            if (waypointClicked != null && waypointClicked != waypointDestiny)
            {
                if (waypointDestiny != null)
                {
                    DrawArrow.ForDebug(waypointClicked.transform.position,
                        waypointDestiny.transform.position - waypointClicked.transform.position,
                        (state == EditingState.Adding ? Color.red : Color.blue));
                }
                else if (previewSphere != null && waypointClicked != null)
                {
                    DrawArrow.ForDebug(waypointClicked.transform.position,
                        previewSphere.transform.position - waypointClicked.transform.position,
                        (state == EditingState.Adding ? Color.red : Color.blue));
                }
            }
        }
        else
        {
            waypointDestiny = null;
        }
    }
}