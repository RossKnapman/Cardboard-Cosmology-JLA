using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {

	// Declare the LineRenderer
	static LineRenderer lineRenderer;

	// The RGB colour of the circle
	static Color32 lineColour = new Color32 ((byte)100, (byte)100, (byte)100, (byte)200);

	// The width of the circle
	static float width = 0.5f;

	// Number of points on each circle for the line renderer (higher number -> less jagged)
	static int noPoints = 100;

	// Create the array to hold the Cartesian coordinates of each point on the circle
	static Vector3[] points = new Vector3 [noPoints];

	// Polar angle of each point
	static float theta;

	// Coordinates of each point on the circle
	static float x;
	static float z;


	public static void drawCircle (GameObject gridCircle, int radius) {

		// Create the LineRenderer object and assign relevant values to it
		lineRenderer = gridCircle.AddComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Mobile/Particles/Additive"));
		lineRenderer.SetColors (lineColour, lineColour);
		lineRenderer.SetWidth (width, width); // Set the width of the line renderer
		lineRenderer.SetVertexCount (noPoints); // Assign noPoints to the number of vertices on each circle

		// Add Cartesian to the coordiantes array
		for (int i = 0; i < noPoints; i++) {
			theta = (i / (float)(noPoints - 1)) * 2 * Mathf.PI; // The '-1' ensure the circle is closed
			x = radius * Mathf.Cos (theta); // Draw circle using polar -> Cartesian coordinate conversion
			z = radius * Mathf.Sin (theta);
			points [i] = new Vector3 (x, 0, z); // Add Cartesian coordinates to points array
		}
		lineRenderer.SetPositions (points); // Draw the circle
	}
}