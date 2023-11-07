# state file generated using paraview version 5.8.0

# ----------------------------------------------------------------
# setup views used in the visualization
# ----------------------------------------------------------------

# trace generated using paraview version 5.8.0
#
# To ensure correct image size when batch processing, please search 
# for and uncomment the line `# renderView*.ViewSize = [*,*]`

#### import the simple module from the paraview
from paraview.simple import *
#### disable automatic camera reset on 'Show'
paraview.simple._DisableFirstRenderCameraReset()

# get the material library
materialLibrary1 = GetMaterialLibrary()

# Create a new 'Render View'
renderView1 = CreateView('RenderView')
renderView1.ViewSize = [1336, 676]
renderView1.AxesGrid = 'GridAxes3DActor'
renderView1.OrientationAxesVisibility = 0
renderView1.CenterOfRotation = [-8.717179298400879e-07, 0.0, -0.49999908248719294]
renderView1.StereoType = 'Crystal Eyes'
renderView1.CameraPosition = [-8.717179298400879e-07, -2.433399528358085, -0.49999908248719294]
renderView1.CameraFocalPoint = [-8.717179298400879e-07, 0.0, -0.49999908248719294]
renderView1.CameraViewUp = [0.0, 0.0, 1.0]
renderView1.CameraFocalDisk = 1.0
renderView1.CameraParallelScale = 0.6298101422825638
renderView1.BackEnd = 'OSPRay raycaster'
renderView1.OSPRayMaterialLibrary = materialLibrary1

SetActiveView(None)

# ----------------------------------------------------------------
# setup view layouts
# ----------------------------------------------------------------

# create new layout object 'Layout #1'
layout1 = CreateLayout(name='Layout #1')
layout1.AssignView(0, renderView1)

# ----------------------------------------------------------------
# restore active view
SetActiveView(renderView1)
# ----------------------------------------------------------------

# ----------------------------------------------------------------
# setup the data processing pipelines
# ----------------------------------------------------------------

# create a new 'OpenFOAMReader'
jeswin_test1foam = OpenFOAMReader(FileName='C:\\Users\\XR-Lab\\Documents\\Jeswin Files\\Project HoloCFD\\openfoamdocker\\jeswin_test1\\jeswin_test1.foam')
jeswin_test1foam.MeshRegions = ['internalMesh']
jeswin_test1foam.CellArrays = ['T', 'U', 'alphat', 'p', 'p_rgh']

# create a new 'Reflect'
reflect1 = Reflect(Input=jeswin_test1foam)

# ----------------------------------------------------------------
# setup the visualization in view 'renderView1'
# ----------------------------------------------------------------

# show data from reflect1
reflect1Display = Show(reflect1, renderView1, 'UnstructuredGridRepresentation')

# get color transfer function/color map for 'T'
tLUT = GetColorTransferFunction('T')
tLUT.AutomaticRescaleRangeMode = 'Never'
tLUT.RGBPoints = [300.0, 0.231373, 0.298039, 0.752941, 310.0, 0.865003, 0.865003, 0.865003, 320.0, 0.705882, 0.0156863, 0.14902]
tLUT.ScalarRangeInitialized = 1.0

# get opacity transfer function/opacity map for 'T'
tPWF = GetOpacityTransferFunction('T')
tPWF.Points = [300.0, 0.0, 0.5, 0.0, 320.0, 1.0, 0.5, 0.0]
tPWF.ScalarRangeInitialized = 1

# trace defaults for the display properties.
reflect1Display.Representation = 'Surface'
reflect1Display.ColorArrayName = ['CELLS', 'T']
reflect1Display.LookupTable = tLUT
reflect1Display.Opacity = 0.38
reflect1Display.OSPRayScaleArray = 'p'
reflect1Display.OSPRayScaleFunction = 'PiecewiseFunction'
reflect1Display.SelectOrientationVectors = 'U'
reflect1Display.ScaleFactor = 0.10001217595709022
reflect1Display.SelectScaleArray = 'p'
reflect1Display.GlyphType = 'Arrow'
reflect1Display.GlyphTableIndexArray = 'p'
reflect1Display.GaussianRadius = 0.005000608797854511
reflect1Display.SetScaleArray = ['POINTS', 'p']
reflect1Display.ScaleTransferFunction = 'PiecewiseFunction'
reflect1Display.OpacityArray = ['POINTS', 'p']
reflect1Display.OpacityTransferFunction = 'PiecewiseFunction'
reflect1Display.DataAxesGrid = 'GridAxesRepresentation'
reflect1Display.PolarAxes = 'PolarAxesRepresentation'
reflect1Display.ScalarOpacityFunction = tPWF
reflect1Display.ScalarOpacityUnitDistance = 0.016323514184224466
reflect1Display.ExtractedBlockIndex = 1

# init the 'PiecewiseFunction' selected for 'ScaleTransferFunction'
reflect1Display.ScaleTransferFunction.Points = [-2.985599994659424, 0.0, 0.5, 0.0, 2.689850091934204, 1.0, 0.5, 0.0]

# init the 'PiecewiseFunction' selected for 'OpacityTransferFunction'
reflect1Display.OpacityTransferFunction.Points = [-2.985599994659424, 0.0, 0.5, 0.0, 2.689850091934204, 1.0, 0.5, 0.0]

# setup the color legend parameters for each legend in this view

# get color legend/bar for tLUT in view renderView1
tLUTColorBar = GetScalarBar(tLUT, renderView1)
tLUTColorBar.WindowLocation = 'AnyLocation'
tLUTColorBar.Position = [0.7186564371257484, 0.2559171597633137]
tLUTColorBar.Title = 'T'
tLUTColorBar.ComponentTitle = ''

# set color bar visibility
tLUTColorBar.Visibility = 1

# show color legend
reflect1Display.SetScalarBarVisibility(renderView1, True)

# ----------------------------------------------------------------
# setup color maps and opacity mapes used in the visualization
# note: the Get..() functions create a new object, if needed
# ----------------------------------------------------------------

# ----------------------------------------------------------------
# finally, restore active source
SetActiveSource(reflect1)
# ----------------------------------------------------------------