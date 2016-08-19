# Cardboard-Cosmology-JLA
App to visualise the JLA supernova catalogue

Please email any questions to rjknapman@gmail.com.

## Adding a New Survey
1. Download [Unity](https://unity3d.com/) (Personal Edition is fine).
2. Download [GitHub Desktop](https://desktop.github.com/).
3. On the [2dF repository page](https://github.com/RossKnapman/Cardboard-Universe-2dF), select "Clone or download", "Open in Desktop", "Launch Application".
4. Clone to your desired location.
5. Replace the "2dF" in the project name with the name of the new survey.
6. In the project, delete the contents of the "Data" folder.
7. In Assets, rename "2dF Visualisation.unity" to "[Name of New Survey] Visualisation.unity".
8. Delete "2dFLogo.png".
7. Delete "2dFProcessed.txt" in Assets/Resources.
7. Add the raw data for the new survey, e.g. in the form of a .txt file.
8. Write a Python programme to convert the raw data to the table below, in a .txt file, with the values separated by commas in the format:

  x position, y position, z position, Red, Green, Blue, Radius of particles
  
  for each galaxy, with each galaxy separated by a new line.

  Note that you may choose to keep some or all of the RGB values and and radius of the particles constant, and hence would not need to include this in the file

  Output the data to Assets/Resources/[Name of New Survey]Processed.txt

  For example, the programme to convert the 2dF data includes the line:

  ascii.write(dataTable, "../Assets/Resources/2dFProcessed.txt", delimiter=",", format="no_header")

  Run this programme to place the data in the required location.

  While writing the programme, make a note of how many units in the Cartesian output correspond to 1 redshift unit. For example, for the 2dF survey, 1000 Cartesian output units correspond to 1 redshift unit.

9. In Assets/Runtime Scripts/Scale Grid/CreateGrid.cs, set redshiftScale on line 7 to the value noted down previously.
10. In the same script, set intervalLength on line 9 to the desired number of units in the Unity visualisation space between successive scale rings.
11. In Assets/Resources/Constants.txt, replace the 300 with the outer limit in Unity visualisation space units that the camera can reach in any dimension from the origin, which also corresponds to the radius of the outer ring of the scale grid (you can decided on this by playing around in Unity). Note that you may wish to override this in the script to draw the scale grid to prevent confusion with a distance in ly greater than the age of the universe, see the CreateGrid.cs script in the WiggleZ repository for an example of when this has been the case.
12. Also in CreateGrid.cs, you frustratingly must manually input the distance in Gly corresponding to each redshift, so write the code to do this in the format of lines 39-48. You can find the values for each redshift value using the calculator [here](http://www.astro.ucla.edu/~wright/CosmoCalc.html).
13. In Assets/Runtime Scripts/AddGalaxies.cs, for any colour values that are to remain constant for all galaxies set the values, e.g. 0.5f.
14. For any colour values which are to vary between galaxies, set them from the required index in the text file containing the processed data.

  For example, say the data contains the Cartesian coordinates, red values, and blue values. Each line would be in the format:
  
  x, y, z, R, B
  
  So the R value would be at index 3, hence set redValue = galaxyData [i][3], and blueValue = galaxyData [i][4]
15. If the particle size varies between galaxies, this can be set in a similar way to the colour.
  1. On line 22, remove the "=2f" part.
  2. Within the loop starting at line 52, add the line particleSize = galaxyData[i][x], where x is the index in a line in the processed data file that stores the radius of the particles. Ensure this is before line 57.
16. Change "2dFProcessed" in line 30 to [Name of New Survey]Processed.
17. In Assets/Runtime Scripts/, on line 13, change the speed that the camera flies around the survey if desired.
18. Change the logo:
  1. Create the logo as a .png file and resize it to 512x512px. Place it in "Assets".
  1. In Unity, select "File", "Build Settings...", "Player Settings" and "Select" under "Default Icon" in the Inspector panel. Select the new logo. (To open the project in Unity, select "Open", navigate the the repository, and click "Open". To open the scene, in the "Project" window, navigate to Assets, and double-click "[Name of New Survey] Visualisation.unity".)
  2. In "Icon", next to the 192x192 option, select the same logo.
20. Build the apk:
  1. Select "File", "Build Settings...".
  2. Under "Other Settings" in the Inspector panel, replace "TwodFSurvey" in the bundle identifier with the name of the new survey.
  3. If you plan on submitting an update to Google Play, increment the Version and the Bundle Version Code by 1.
  4. FOR THE FIRST TIME ONLY under "Publishing Settings", create a new keystore. Remember the password (for all of the projects so far it is "survey"). IF YOU CREATE A NEW KEYSTORE ONCE IT'S BEEN SUBMITTED TO GOOGLE PLAY, YOU WILL NOT BE ABLE TO SUBMIT A NEW UPDATE so always "Use Existing Keystore" after publishing.
21. Upload the new project to GitHub:
  1. In Unity, select "Edit", "Project Settings", "Editor".
  2. Ensure the version control mode is set to "Visible Meta Files".
  3. Ensure the Asset Serialisation is set to "Force Text".
  4. On the GitHub website, create a new repository named "Cardboard-Universe-[Name of New Survey]
  5. Add the .gitignore Unity template.
  6. Once the repository is created, clone it to the GitHub desktop like when this was downloaded.
  7. Open up the project in Finder.
  8. Copy and paste the Assets, Project Settings, and Data folders to the newly-created local repository.
  9. Click the icon in to top-right-hand corner in GitHub desktop to change to the local respository.
  10. Commit the changes to master.
  11. Select "Sync" to upload the new project to GitHub.
  12. Add this README file to the repository.

I'm pretty sure this is complete, though I have most likely missed out some minor steps along the way so feel free to email me with any questions if you run into problems.

## File Structure

### Assets

#### Google VR
The Google VR SDK for Unity, which contains the required scripts and prefabs, such as the camera.

#### Materials
Stores any custom-made materials, e.g. the material of the galaxy particles.

#### Plugins
Contains any plugins that extend Unity's features. I haven't added any, but I deleted the iOS one because there was a file that was greater than GitHub's 100 MB limit.

#### Resources
Contains any files that are to be accessed by the runtimes scripts.
##### [Name of Survey]Processed.txt
Contains the data that has been processed and is ready to be accessed by AddGalaxies.cs. Contains x, y, z coordinates, possible R, G, B values, and possibly the radius of each galaxy.
##### Constants.txt
Stores any constants that are to be accessed by more than one script. Currently only contains the outer boundary of the visualisation in Unity units in each dimension, used to determine when to stop the camera if it moves too far from the origin and the radius of the outer scale ring.

#### Runtime Scripts
Contains C# scripts to be run during runtime. Note that they must be attached to (often empty) game objects to be run automatically. The lines within the Start() function are run when the app opens, while those within the Update() function are called once per frame. See the comments for detailed information about each script.
##### AddGalaxies.cs
Takes information from the processed data .txt file about e.g. galaxy position, colour, and size, and places the particles in space. Attached to the game object "SpawnGalaxies".
##### Exit.cs
Controls the double-click-to-exit feature. Attached to the game object "Exit".
##### MoveCamera.cs
Controls the clicking to start/stop moving the gamera. Attached to the game object "GvrMain" (the camera).
##### CreateGrid.cs
For drawing the scale circles and text. Attached to the game object "SpawnGrid".
##### Circle.cs
For actually drawing the circle. Called by CreateGrid.cs, so not attached to any game object.

#### [Survey Name].unity
The scene file.

#### [Survey Name]logo.png
The app's logo.

### Project Settings
Stores information about the project settings. Unity generates these files; they do not need to be modified manually.

### Data
When uploading a Unity project to GitHub, generally only the "Assets" and "Project Settings" folders need to be uploaded as Unity generates the rest, but I have included the third folder "Data" which contains the raw data and a programme to process it.

#### The Data .txt File
This various a lot between surveys, so requires a Python programme to transfer it to the required format.

#### The Python Data Conversion File
Converts the raw data to the required format as previously specified and adds to to the folder Assets/Resources/. See the existing repositories for examples of such data conversion.
