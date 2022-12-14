
# Installation instructions

- Clone this repository.
- Open the project on Unity 2019.4.x
- To access the tool:
  - Select ***Assets -> Maps -> SampleScene1*** by double clicking it on the *Project* tab.
  - Double-click ***Terrain*** on the *Hierarchy* tab.
  - Click on ***Window -> BiomeGenerator***
  - If you get a black window, just increase its size by dragging the lower right corner.

# Basic usage

Select the desired Unity terrain from the Scene or the selection box and press the ***Select Terrain*** button.

To add a layer, deploy ***GeneralSettings -> Layers*** and press the ***+*** button. The ***X*** button deletes the layer and the ***O*** selects the layer. The toggle button sets the layer's visibility.

To paint, left click and drag over the *Draw Area Panel*. To erase, right click and drag over the *Draw Area Panel*. ***Intensity*** on the *Draw Setting Panel* indicates the element density in the painted zone. Painting and erasing increase and decrease density respectively, up to the intenisty value.  


Add elements to ***LayerSettings -> MapElements*** to select elements to populate each layer. Sample assets can be found in ***Assets -> MapElements***.

Add elements to ***LayerSettings -> FitnessFunctions*** to select fitness functions to target when providing suggestions. There is a limit of 8 functions per layer.

Press ***DisplayOptions -> RUN*** to start generating suggestions. clicking on a suggestion will replace your current map.

Press ***DrawSettings -> Generate*** to generate the 3D map.

# Basic Biome Testing Kit

The Biome Testing Kit has a group of objects marked with tags to define what properties it should provide (P) to the environment or require (R) from it. These tags are used by the AI to help the designer in the map creation process. The following table list the objects and tags present in the kit and their association. A P mark what the objects provides, R+ marks if the object require the pressence of the stat and R- if it requires it absence. Objects and tags can be modified by the user at will.

|:---:                |:---:  |:---:   |:---:    |:---:     |:---:       |:---:      |:---:   |:---:    |:---:   |:---:     |:---:     |:---:    |:---:         |
|                     |**Pet**|**Food**|**Shell**|**Danger**|**Collider**|**Mineral**|**Rock**|**Magic**|**Wood**|**Leaves**|**Shadow**|**Grass**|**Decorative**|  
|**Cat**              |P      |        |         |          |            |           |        |         |        |          |R+        |         |              |
|**Chicken**          |       |P       |         |          |            |           |        |         |        |          |          |R+       |              |
|**Crab**             |       |P       |P        |P         |            |           |R+      |         |        |          |          |         |              |
|**Dog**              |P      |        |         |          |            |           |        |         |        |          |          |         |              |
|**Lion**             |       |P       |         |P         |            |           |        |         |        |          |R+        |R+       |              |
|**Penguin**          |P      |        |         |          |            |           |        |R+       |        |          |          |         |              |
|**Mineral 1 - 12**   |       |        |         |          |P           |P          |P       |         |R-      |          |          |         |              |
|**Mineral 13**       |       |        |         |          |P           |P          |        |R+       |        |          |          |         |              |
|**Mineral 14 - 17**  |       |        |         |          |P           |           |P       |         |R-      |          |          |         |              |
|**Plant 1 - 3**      |       |P       |         |          |            |           |        |         |        |P         |P         |P        |              |
|**Plant 4 - 15**     |       |        |         |          |            |           |        |         |        |          |          |P        |              |
|**Plant 16 - 17**    |       |        |         |          |            |           |        |         |        |          |          |P        |P             |
|**Plant 18 - 19**    |       |        |         |          |            |           |        |R+       |        |          |          |P        |              |
|**Tree 1 - 9**       |       |        |         |          |P           |           |        |         |P       |P         |P         |         |              |
|**Tree 10**          |       |        |         |          |P           |           |        |P        |        |          |          |         |              |
|**Three Stump 1 - 7**|       |        |         |          |P           |           |        |         |P       |          |          |         |              |

## Caveats 

- BGT doesn't have an ***Undo*** feature.
- BGT doesn't have a ***Save*** feature. If you close the window, you will lose your work.
- BGT only works in the Editor, if you press ***Play*** you will lose your work.
