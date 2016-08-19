using UnityEngine;
using System;
using System.Collections;

public class AddGalaxies : MonoBehaviour {

	// Note that in the comments, I represent arrays using curly brackets "{}"

	// Declare the variables specific to the JLA visualisation
	static float redValue = 0.2f; // The red value of each particle, on a scale from 0 to 1
	static float greenValue = 0.2f; // The blue value of each particle, on a scale from 0 to 1
	static float blueValue; // Loaded in script from data
	static int sizeMultiplier = 7; // For making quick changes to the sizes of the points

	// Declare the general variables
	static TextAsset file; // Stores all the unprocessed data when it is loaded
	static string[] lines; // Stores an array of the lines in the unprocessed data
	static int noGalaxiesInSurvey; // Number of galaxies in the survey
	static float[][] galaxyData; // Holds array of values for each galaxy, with each galaxy having an array of values (e.g. {{x1, y1, z1, redValue1, blueValue1}, {x2, y2, z2, redValue2, blueValue2}, ... }
	static int lineNumber = 0; // Used to keep track of which line is being read in the foreach loop to access and write to the relevant line in galaxyData
	static String[] parts_of_line; // Array to hold each line i.e. [x, y, z, redValue, blueValue] in an array {x, y, z, redValue, blueValue}
	static float[] floatArray; // Same as parts_of_line, but the values have been converted from string to float
	static float particleSize; // The size of the particles, loaded from data
	static float alphaValue = 1.0f; // The red value of each particle, on a scale from 0 to 1
	static ParticleSystem galaxyParticleSystem; // The particle system
	static ParticleSystem.Particle[] galaxyParticles; // Array containing the particles


	void Start () {

		file = Resources.Load<TextAsset> ("JLAProcessed"); // Load galaxies from text file

		lines = file.text.Split ('\n'); // Convert lines of text file to array i.e. {Information about first galaxy, information about second galaxy, ... }

		noGalaxiesInSurvey = lines.Length - 1; // Number of galaxies (the '-1' is because there is always a blank line at the end of the processed data file)

		galaxyData = new float[noGalaxiesInSurvey][]; // Array to hold galaxy data read from file (see description in variable declaration)

		// Take galaxies from the text file and put them into a jagged array galaxyData
		foreach (string line in lines) {
			try {
				parts_of_line = line.Split (','); // Split each line into a string array of {x, y, z, redValue, blueValue}
				floatArray = Array.ConvertAll<string, float> (parts_of_line, Convert.ToSingle); // Convert parts_of_line values to floating-point values
				galaxyData [lineNumber] = floatArray; // Add the floatArray of form {x, y, z, redValue, blueValue} to each line in galaxyData
				lineNumber++;
			} catch (FormatException) {
				// Do nothing (for the blank line at the end of the processed data file, which causes this exception to be raised)
			}
		}

		// Create particles representing the galaxies
		galaxyParticles = new ParticleSystem.Particle[noGalaxiesInSurvey]; // Array to hold the positions of the particles
		for (int i = 0; i < noGalaxiesInSurvey; i++) {
			galaxyParticles [i].position = new Vector3 (galaxyData [i][0], galaxyData [i][1], galaxyData [i][2]); // Set positions of particles
			particleSize = galaxyData [i][3];
			galaxyParticles [i].startSize = particleSize * sizeMultiplier; // Set sizes of particles, multiplied by 
			blueValue = galaxyData [i][4];
			galaxyParticles [i].startColor = new Color (redValue, greenValue, blueValue, alphaValue);
		}
		galaxyParticleSystem = gameObject.GetComponent<ParticleSystem> ();
		galaxyParticleSystem.SetParticles (galaxyParticles, galaxyParticles.Length); // Place the particles in space
	}
}