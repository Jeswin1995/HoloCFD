import bpy
import os
from distutils.dir_util import copy_tree
import timeit
import glob
import shutil

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
path_metadata = 'process/metadata/'
os.makedirs(path_metadata, exist_ok=True)
path_paraview = os.getcwd() + '/' + path_metadata
path_blender = path_paraview
path_unityfor = os.getcwd() + '/' + 'process/'
# Loop through all objects in the scene
#blender starts metadata import & export
path_to_obj_dir = os.path.join(path_blender)
#get list of all files in directory
file_list = sorted(os.listdir(path_to_obj_dir))
#get a list of files ending in 'obj' in Blender
obj_list = [item for item in file_list if item.endswith(import_format_blender)]
#loop through the strings in obj_list and add the files to the scene
for item in obj_list:
    path_to_file = os.path.join(path_to_obj_dir, item)
    bpy.ops.import_scene.x3d(filepath = path_to_file)
    #get the current path and make a new folder for the exported meshes
    path_blender = bpy.path.abspath(path_blender)
    # Get a reference to the current active scene
    scene = bpy.context.scene
    for obj in scene.objects:
        bpy.ops.object.select_all(action='DESELECT') #to deselect all meshes
        # Select the current object

        # Check if the object name starts with "Shape_IndexedFaceSet"
        #if obj.name.startswith("Shape_IndexedFaceSet"):
        if obj.name != "Cube":
            # Select the object
            obj.select_set(True)
            print(obj.name)
        
    
    bpy.ops.export_scene.gltf(filepath=path_blender + '1000+0.10+HE24+T' + export_format_blender, export_format='GLB', use_selection=True,export_tangents=True, export_attributes=True,) 
    print("Done")   

"""
Data processing analytics for qualitative studies (3)
"""
        
path_to_obj_dir = os.path.join(path_blender)
file_list = sorted(os.listdir(path_to_obj_dir))
obj_list = [item for item in file_list if item.endswith('.glb')]
size_file_blender = []
for item in obj_list:
    path_to_file = os.path.join(path_to_obj_dir, item)
    size_processing_blender=os.path.getsize(path_to_file)
    print('GLB file sizes in bytes:',size_processing_blender)
    size_file_blender.append(size_processing_blender)
    shutil.copy(path_to_file, os.getcwd() + '/' + 'ExpressServer/ABs/')
    cloud_path = r"Z:\TestCFD"
    shutil.copy(path_to_file, cloud_path)

print('Data processing has successfully been completed!')

#clean up metadata 
#metadata = input ('Clean all metadata? [y or n]: ')
metadata = "y"
mypath = path_paraview
if metadata == 'y':
    for mydata in glob.glob(mypath + "Shape*"):
        shutil.copy(mydata, os.getcwd() + '/' + 'process/')
    for metadata in glob.glob(mypath + "*"):
        os.remove(metadata)
    print ('Metadata is cleaned.')
else:
    print ('Metadata is available under the process directory.')
#self.text_comments.insert('end', open(filename,'r').read())




"""
The end
"""