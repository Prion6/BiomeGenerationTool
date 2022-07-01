
# Installation instructions

- Clone this repository.
- Open the project on Unity 2019.4.x
- To access the tool:
  - Select ***Assets -> Maps -> SampleScene1*** by double clicking it on the * *Project* * tab.
  - Double-click ***Terrain** on the * *Hierarchy* * tab.
  - Click on ***Window -> BiomeGenerator***
  - If you get a black window, just increase its size by dragging the lower right corner.

# Basic usage

Select the desired Unity terrain from the Scene or the selection box and press the ***Select Terrain*** button.

To add a layer, deploy ***GeneralSettings -> Layers*** and press the ***+*** button. The ***X*** button deletes the layer and the ***O*** selects the layer. The toggle button sets the layer's visibility.

To paint, left click and drag over the * *Draw Area Panel* * . To erase, right click and drag over the * *Draw Area Panel* * . ***Intensity*** on the * *Draw Setting Panel* * indicates the element density in the painted zone. Painting and erasing increase and decrease density respectively, up to the intenisty value.  


Add elements to ***LayerSettings -> MapElements*** to select elements to populate each layer. Sample assets can be found in ***Assets -> MapElements***.

Add elements to ***LayerSettings>FitnessFunctions*** to select fitness functions to target when providing suggestions. There is a limit of 8 functions per layer.

Press ***DisplayOptions -> RUN*** to start generating suggestions. clicking on a suggestion will replace your current map.

Press ***DrawSettings -> Generate*** to generate the 3D map.

## Caveats 

- BGT doesn't have an ***undo*** feature.
- BGT doesn't have a ***save*** feature. If you close the window, you will lose your work.
- BGT only works in the Editor, if you press ***Play*** you will lose your work.
