import numpy as np
from astropy.coordinates import Angle
from astropy import units as u
from astropy import coordinates as coord
from astropy.table import Table
from astropy.io import ascii

# Define a function to convert stretch factor to radius
def stretchToRadius(StretchFactorValues):
    s_min = np.amin(StretchFactorValues) # Get the minimum stretch factor value
    s_max = np.amax(StretchFactorValues) # Get the maximum stretch factor value
    R_max = 10 # The maximum radius of the sphere - 0.5 (due to 0.5 offset so that the sphere with s = s_min is still shown, change R_max according to preference/ what looks good)
    R_max_squared = R_max ** 2
    s_norm = (StretchFactorValues - s_min)/(s_max - s_min) # Normalise stretch factor values to a 0 to 1 scale
    R_squared_values = (s_norm * R_max_squared) + 0.5 # Array of radius squared values
    return np.sqrt(R_squared_values)

# Load the supernova data from the .txt file
print("\nImporting data...")
data = np.genfromtxt("JLAData.txt", dtype="str", delimiter=" ")

# Slice the data array to obtain the values of interest
print("\nSlicing arrays...")
RAStrings = data[:,18]
DecStrings = data[:,19]
RedshiftStrings = data[:,1]
StretchFactorStrings = data[:,6]
ColourStrings = data[:,8]

# Convert the string values to values which Astropy can manage
print("\nConverting data...")
print("\tGetting RA values...")
RAValues = Angle(RAStrings, u.deg)
print("\tConverting RA values to radians...")
RAValues = RAValues.to(u.rad)
print("\tGetting Dec values...")
DecValues = Angle(DecStrings, u.deg)
print("\tConverting Dec values to radians...")
DecValues = DecValues.to(u.rad)
print("\Getting redshift values...")
RedshiftValues = RedshiftStrings.astype(np.float)
print("\tGetting stretch factor values...")
StretchFactorValues = StretchFactorStrings.astype(np.float)
print("\tGetting colour values...")
BVValues = -0.25 - ColourStrings.astype(np.float)

# I have used the above formula for BVValues as the formula for <B-V> is given by Colour = (B-V)_max - <B-V> (http://adsabs.harvard.edu/abs/2007A&A...466...11G, see Section 2)
# The value of (B-V)_max = -0.25 is from http://iopscience.iop.org/article/10.1086/133013/pdf

# Get the Cartesian coordinates of the supernovae
print("\nGetting cartesian values...")
multiplier = 1000 # The factor to spread it out in Unity visualisation space, equivalent to redshiftScale in CreateGrid.cs
XArray = RedshiftValues * np.cos(DecValues) * np.cos(RAValues) * multiplier
YArray = RedshiftValues * np.cos(DecValues) * np.sin(RAValues) * multiplier
ZArray = RedshiftValues * np.sin(DecValues) * multiplier
    
# Get the values for the radius of each sphere (stretch factors is proportional to brightness which is proportional to the square of the radius)
print("\nGetting radii of supernovae...")
RadiusValues = stretchToRadius(StretchFactorValues)

# Get the 'blue' values for each sphere (between 0 and 1)
gamma = 2.2
cutoff = 20
Intensities = 10 ** BVValues
Blue = Intensities ** gamma
BlueValues = np.interp(Blue, np.array([np.sort(Blue)[cutoff], np.sort(Blue)[len(Blue)-cutoff]]), np.array([0.4, 1.0]))

# Assign values to table
print("\nWriting data to table...")
dataTable = Table({'x' : XArray, 'y' : YArray, 'z' : ZArray, 'R' : RadiusValues, 'B' : BlueValues}, names=("x", "y", "z", 'R', 'B'))

# Write to file
print("\nWriting to file...")
ascii.write(dataTable, "../Assets/Resources/JLAProcessed.txt", delimiter=",", format="no_header")

print("\nDone!\n")