import os
import sys
import timeit
import glob
import shutil
from distutils.dir_util import copy_tree
from paraview.simple import *

#start2 = timeit.default_timer()
#data format to export visual CFD data from ParaView: x3d, vrml, svg and other supported formats
#(optional) exportParaview = input ('Enter the export format from ParaView: ')
exportParaview = 'x3d'
export_format_paraview = '.' + exportParaview

import_format_blender = export_format_paraview
#data format to export visual CFD data from Blender: fbx, obj, 3ds, ply, stl and other supported formats

exportBlender = 'glb'  # Define the export format for Blender
export_format_blender = '.' + exportBlender
#create a directory to collect processed data and metadata
statefile = "T.py"
path_metadata = 'process/metadata/'
os.makedirs(path_metadata, exist_ok=True)
path_paraview = os.getcwd() + '/' + path_metadata
path_blender = path_paraview
path_unityfor = os.getcwd() + '/' + 'process/'

# Logging of operations from terminal and save in a text file
class Tee(object):
    def __init__(self, *files):
        self.files = files

    def write(self, obj):
        for f in self.files:
            f.write(obj)
            f.flush()  # If you want the output to be visible immediately

    def flush(self):
        for f in self.files:
            f.flush()

f = open('logfile_tessellate.txt', 'w')
original = sys.stdout
sys.stdout = Tee(sys.stdout, f)
print("Data processing is in progress...")

# ParaView state file operation
# Timer for each section - data processing speed
start1 = timeit.default_timer()
# Import user-defined Paraview state
exec(open(statefile).read())

# Update the view to ensure updated data information
renderView1 = GetActiveViewOrCreate('RenderView')
renderView1.Update()
stop1 = timeit.default_timer()

# Data processing via ParaView pipeline
start2 = timeit.default_timer()
# Create a directory to collect processed data and metadata
os.makedirs(path_paraview, exist_ok=True)

# Define the total number of timesteps (1 for steady-state solutions)
timestep_sim = 2

# Obtain a list of timesteps with values
animationScene1 = GetAnimationScene()
tsteps = animationScene1.TimeKeeper.TimestepValues
# Check if tsteps is a single float value or a list/array
if isinstance(tsteps, float):
    # If it's a float, make it a list with one element
    tsteps = [tsteps]

# Now you can safely access the last element
animationScene1.AnimationTime = tsteps[-1]
animationScene1.AnimationTime = tsteps[-1]
# ParaView metadata export
ExportView(path_paraview + str(tsteps[-1]) + export_format_paraview, view=renderView1, ExportColorLegends=1)


# for x in range(0, timestep_sim):
#     # ParaView metadata export
#     ExportView(path_paraview + str(x) + export_format_paraview, view=renderView1, ExportColorLegends=1)
#     # Check out the data processed
#     SaveScreenshot(path_paraview + str(x) + '.png', renderView1, ImageResolution=[1025, 782])
#     # Switch to the next timestep
#     animationScene1 = GetAnimationScene()
#     animationScene1.AnimationTime = tsteps[-1]
stop2 = timeit.default_timer()

"""
Data processing analytics for qualitative studies (3)
"""
        
#b_x3d = os.path.getsize(path_blender + tsteps[-1]+'.x3d')
print('*****Data processing performance & analytics*****')
print('Time_ParaView: ', stop1 - start1)
#print('X3D file size in bytes:',b_x3d)
print('Time_ParaView_Blender: ', stop2 - start2)
print('Time_Integration (sec): ', stop2 - start2 + stop1 - start1)