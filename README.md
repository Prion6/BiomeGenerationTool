
# Installation instructions

- Clone this repository.
- Open the project on Unity 2019.4.x
- To access the tool:
  - Click on ***Window -> BiomeGenerator***
  - If you get a black window, just increase its size by dragging the lower right corner.

# Basic usage

Select the desired Unity terrain from the Scene or the selection box and press the ***Select Terrain*** button.

To add a layer, deploy ***GeneralSettings -> Layers*** and press the ***+*** button. The ***X*** button deletes the layer and the ***O*** selects the layer.




Para agregar una capa despliegue GeneralSettings>Layers y presiones el botón +
El botón X elimina la capa, el botón O selecciona una capa. El toggle cambia 
la capa entre visible y no visible.

Para pintar presione el botón izquierdo del mouse sobre la vista del mapa
Para borrar presione el botón derecho del mouse sobre la vista del mapa

La Intensidad indica la concentración de elementos que habrán en la zona
designada al pintar. as acciones de pintar y borrar no sobrepasan el valor de
la Intensidad.

En cada capa se puede definir que elementos que poblaran esa capa en la 
Lista LayerSettings>MapElements. Se pueden encontrar Árboles, Animles, 
Plantas y Rocas a Disposición en la carpeta MapElements.

Se pueden definir funciones objetivos con las cuales optimizar la capa en la
Lista LayerSettings>FitnessFunctions. Hay un límite de 8 funciones por capa.
Para asignar una función objetivo presionar el botón en el extremo derecho del
campo y seleccionar una de la lista que aparece.

Para comenzar a generar sugerencias presionar DisplayOptions>RUN. Para usar 
una sugerencia presionarla. Al hacerlo el contenido de la capa cambiara por el
de la sugerencia.

Para generar la versión 3D presionar DrawSettings>Generate.

La herramienta no tiene la opción de deshacer acciones
La herramienta no tiene sistema de guardado. Si cierra la ventana perderá 
su avance.
La herramienta funciona en Editor si presiona el botón de Play de Unity perderá 
su avance.

En casos de uso más avanzados el usuario puede crear sus propios MapElements o 
Fitness Functions.