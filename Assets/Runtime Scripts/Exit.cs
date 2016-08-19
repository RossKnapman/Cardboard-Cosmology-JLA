using UnityEngine;
using System.Collections;
using System.Threading;
using System.Diagnostics;

public class Exit : MonoBehaviour {

	public bool updated; // Prevents detectMoveType() from running until the update() that called it has passed
	public bool Triggered; // Makes GvrViewer.Instance.Triggered accessible from the detectMoveType() thread (by setting public)
	Thread detectDoubleClickThread; // Threads allow the camera to keep moving while detectMoveType() is running
	bool quit; // Application.Quit() can only be called from the main thread, so need a variable to carry this message
	public bool waitingForClick; // Stops the camera while detecting move type
	Stopwatch timer = new Stopwatch(); // Used to track time in detecting double clicks
	long clickTime = 500L; // Maximum allowed time between clicks of a double click - test gave ~600 ms
	bool clickedAgain; // The trigger has been pulled again after a single click (a double click)


	void Update () { // Called once per frame
		updated = true; // Previous update complete
		Triggered = GvrViewer.Instance.Triggered; // Note that GvrViewer.Instance.Triggered == True lasts for one call of Update(), i.e. one frame (see Unity documentation)
		if (Triggered) { // If the button is pressed
			if (detectDoubleClickThread == null) { // If there is currently no thread already processing a click
				updated = false; // The update function hasn't finished being called yet
				detectDoubleClickThread = new Thread (detectDoubleClick); // Create a new thread to process the click
				detectDoubleClickThread.Start (); // Start the new thread to process the click
			}
		}

		if (quit) { // Called if a double click has been detected
			Application.Quit ();
			quit = false;
		}
	}


	void detectDoubleClick() {
		waitingForClick = true;
		// Keeps the thread running until the time clickTime has passed

		while (!updated) {
			// Delay this function until the Update() the called it has passed
			// Otherwise, clickedAgain would pretty much always be true as GvrViewer.Instance.Triggered lasts for one Update() call,
			// enough for many cycles of this thread to have been called (the time of each loop of this thread is significantly less
			// than the time of one Update() call)
		}

		timer.Start (); // Only runs when Updated is true to prevent counting double clicks when there was no click
		clickedAgain = false;

		while (timer.ElapsedMilliseconds < clickTime) { // Detect whether or not a double-click occurs within 'clickTime'
			if (Triggered == true) {
				clickedAgain = true; // There has been a double click
			}
		}

		timer.Reset ();
		waitingForClick = false; // clickTime has now passed

		if (clickedAgain) { // If there has been a double click
			quit = true;
		}
		detectDoubleClickThread = null; // Stop the thread from running
	}
}