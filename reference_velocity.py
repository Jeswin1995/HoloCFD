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
renderView1.ViewSize = [1330, 672]
renderView1.AxesGrid = 'GridAxes3DActor'
renderView1.CenterOfRotation = [0.09620902873575687, 0.0024532824754714966, -0.5929342149756849]
renderView1.StereoType = 'Crystal Eyes'
renderView1.CameraPosition = [-2.14955878160498, -1.3519416419375174, -1.8835644830532754]
renderView1.CameraFocalPoint = [0.08231110045938624, 0.2008766013722368, -0.3446036669935883]
renderView1.CameraViewUp = [0.24112483062630624, 0.4859818731110196, -0.8400478766492658]
renderView1.CameraFocalDisk = 1.0
renderView1.CameraParallelScale = 0.6682754797769143
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

# ----------------------------------------------------------------
# setup the visualization in view 'renderView1'
# ----------------------------------------------------------------

# show data from jeswin_test1foam
jeswin_test1foamDisplay = Show(jeswin_test1foam, renderView1, 'UnstructuredGridRepresentation')

# get color transfer function/color map for 'T'
tLUT = GetColorTransferFunction('T')
tLUT.AutomaticRescaleRangeMode = 'Never'
tLUT.RGBPoints = [299.999755859375, 0.231373, 0.298039, 0.752941, 309.9998779296875, 0.865003, 0.865003, 0.865003, 320.0, 0.705882, 0.0156863, 0.14902]
tLUT.ScalarRangeInitialized = 1.0

# get opacity transfer function/opacity map for 'T'
tPWF = GetOpacityTransferFunction('T')
tPWF.Points = [299.999755859375, 0.0, 0.5, 0.0, 320.0, 1.0, 0.5, 0.0]
tPWF.ScalarRangeInitialized = 1

# trace defaults for the display properties.
jeswin_test1foamDisplay.Representation = 'Surface'
jeswin_test1foamDisplay.ColorArrayName = ['CELLS', 'T']
jeswin_test1foamDisplay.LookupTable = tLUT
jeswin_test1foamDisplay.OSPRayScaleArray = 'p'
jeswin_test1foamDisplay.OSPRayScaleFunction = 'PiecewiseFunction'
jeswin_test1foamDisplay.SelectOrientationVectors = 'U'
jeswin_test1foamDisplay.ScaleFactor = 0.0991985181812197
jeswin_test1foamDisplay.SelectScaleArray = 'p'
jeswin_test1foamDisplay.GlyphType = 'Arrow'
jeswin_test1foamDisplay.GlyphTableIndexArray = 'p'
jeswin_test1foamDisplay.GaussianRadius = 0.004959925909060985
jeswin_test1foamDisplay.SetScaleArray = ['POINTS', 'p']
jeswin_test1foamDisplay.ScaleTransferFunction = 'PiecewiseFunction'
jeswin_test1foamDisplay.OpacityArray = ['POINTS', 'p']
jeswin_test1foamDisplay.OpacityTransferFunction = 'PiecewiseFunction'
jeswin_test1foamDisplay.DataAxesGrid = 'GridAxesRepresentation'
jeswin_test1foamDisplay.PolarAxes = 'PolarAxesRepresentation'
jeswin_test1foamDisplay.ScalarOpacityFunction = tPWF
jeswin_test1foamDisplay.ScalarOpacityUnitDistance = 0.0187427567609613
jeswin_test1foamDisplay.ExtractedBlockIndex = 1

# init the 'PiecewiseFunction' selected for 'ScaleTransferFunction'
jeswin_test1foamDisplay.ScaleTransferFunction.Points = [-5.269542694091797, 0.0, 0.5, 0.0, 0.5422264933586121, 1.0, 0.5, 0.0]

# init the 'PiecewiseFunction' selected for 'OpacityTransferFunction'
jeswin_test1foamDisplay.OpacityTransferFunction.Points = [-5.269542694091797, 0.0, 0.5, 0.0, 0.5422264933586121, 1.0, 0.5, 0.0]

# setup the color legend parameters for each legend in this view

# get color legend/bar for tLUT in view renderView1
tLUTColorBar = GetScalarBar(tLUT, renderView1)
tLUTColorBar.Title = 'T'
tLUTColorBar.ComponentTitle = ''

# set color bar visibility
tLUTColorBar.Visibility = 1

# show color legend
jeswin_test1foamDisplay.SetScalarBarVisibility(renderView1, True)

# ----------------------------------------------------------------
# setup color maps and opacity mapes used in the visualization
# note: the Get..() functions create a new object, if needed
# ----------------------------------------------------------------

# ----------------------------------------------------------------
# finally, restore active source
SetActiveSource(jeswin_test1foam)
# ----------------------------------------------------------------