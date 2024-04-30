# Gramola

## Descripción del Juego

<p align="justify">La Gramola Musical es un juego desarrollado en Unity que simula la experiencia de utilizar un reproductor de música o una gramola. Los jugadores pueden interactuar con una interfaz intuitiva para seleccionar entre una variedad de canciones, cada una acompañada de información detallada sobre la pista en reproducción. El juego ofrece controles completos sobre la reproducción de la música, incluyendo reproducción, pausa, cambio de velocidad, selección aleatoria de canciones, y más.</p>

&nbsp;

## Características Principales

1. **Interfaz de Gramola:**
   - La interfaz se asemeja a una gramola o reproductor de música clásico.
   - Los jugadores pueden elegir entre cinco canciones distintas utilizando botones visuales con las portadas de los álbumes.

&nbsp; 


2. **Información de la Canción:**
   - <p align="justify">Muestra un menú informativo con los datos de la canción en reproducción, incluyendo el nombre de la canción, el artista y la fecha de lanzamiento.</p>

```sh
// Introduce los datos correpondientes de texto para cada canción.
infoSong.text = "Nombre: " + Nombre[id] + "     " + "Artista: " + Artista + "     " + "Fecha Lanzamiento: " + Fecha[id];
```
&nbsp; 


3. **Controles de Reproducción:**
   - <p align="justify">Controles completos sobre la reproducción de la música, incluyendo Play, Stop, Pause, Speed, Doble Speed, Slow, Half Speed, Prev Song y Next Song.</p>

```sh
// Pausa la canción.
public void Pause()
{musica.Pause();}

// Continua la canción y la deja a velocidad normal.
public void Play()
{musica.Play();
musica.pitch = 1;}

// Para la canción y la devuelve al segundo 0.
public void Stop()
{musica.Stop();}

// Reproduce la canción a velodidad media.
public void Avanzar()
{musica.pitch = 0.5f;}

// Reproduce la canción a doble velodidad.
public void Avanzarx2()
{musica.pitch = 2;}

// Rebobina lento.
public void Rebobinar()
{musica.pitch = -0.5f;}

// Rebobina rápido.
public void Rebobinarx2()
{musica.pitch = -1;}
```
&nbsp; 


4. **Selección Aleatoria:**
   - <p align="justify">El botón "Random" permite a los jugadores elegir si la siguiente canción reproducida será seleccionada aleatoriamente de la lista o seguirá un orden predefinido.</p>

```sh
 // Cambia el color del boton random cuando es pulsado.
 public void ColorBotonRandom() 
 {
     if (Aleatorio)
     {
         BotonRandom.GetComponent<Image>().color = Color.green;
     }
     else
     {
         BotonRandom.GetComponent<Image>().color = Color.white;
     }
 }
```
&nbsp; 


5. **Reproducción Automática:**
   - <p align="justify">Sistema que detecta cuando una canción ha finalizado y automáticamente comienza la siguiente canción después de un intervalo de tiempo ajustable. La siguiente canción puede ser influenciada por la opción de selección aleatoria.</p>

```sh
// Pasa de cancion automaticamente cuando llega al final.
public void NextAutomatico()
{
    if (musica.clip != null)
    {
        TimeLine.value = musica.time / musica.clip.length;
        if (musica.isPlaying && musica.time >= musica.clip.length - 0.1f) // 0.1f es un peque�o margen para asegurarse de que hemos llegado al final.
        {
            Next();
        }
    }
}
```
&nbsp; 


6. **Historial de Reproducción:**
   - <p align="justify">Sistema que registra las últimas canciones reproducidas, permitiendo a los jugadores volver a reproducir la canción anterior utilizando el botón "Prev". Este sistema se adapta incluso cuando se utiliza la selección aleatoria de canciones.</p>

```sh
// Coge la cancion anterior 
public void CogerNumAle() 
{
    if (historial.Count >= 2) { NumAle = historial[historial.Count - 2]; }
}

// Pasa a la anterior canción detectando si esta en normal o en aleatorio.
public void Pre()
{
    if (!Aleatorio)
    {
        cancionsonando--;
        historial.RemoveAt(historial.Count - 1);
        if (historial.Count == 0)
        {
            historial.Clear();
            musica.Stop();
            musica = GetComponent<AudioSource>();
            infoSong.text = "Nombre:" + "                      " + "Artista:" + "                          " + "Fecha Lanzamiento";
            cancionsonando++;
            historial.Add(cancionsonando);
            if (historial.Count == 0)
            {
                cancionsonando = 0;
            }
        }
        if (cancionsonando < 0)
        {
            cancionsonando = 4;
        }
        Reproducir(cancionsonando);
    }
    else
    {
        historial.RemoveAt(historial.Count - 1);
        if (historial.Count == 0)
        {
            historial.Clear();
            musica.Stop();
            musica = GetComponent<AudioSource>();
            infoSong.text = "Nombre:" + "                      " + "Artista:" + "                          " + "Fecha Lanzamiento";
            historial.Add(cancionsonando);
            if (historial.Count == 0)
            {
                cancionsonando = 0;
            }
        }
        Reproducir(NumAle);
    }
    historial.RemoveAt(historial.Count - 1);


    infoSong.text = "Nombre: " + Nombre[cancionsonando] + "     " + "Artista: " + Artista + "     " + "Fecha Lanzamiento: " + Fecha[cancionsonando];
}
```
&nbsp; 


7. **Espectro de Onda:**
   - <p align="justify">Agrega un espectro de onda visual que se mueve al ritmo de la canción en reproducción, proporcionando una experiencia visual dinámica.</p>

```sh
// Espectro de onda.
float[] spectrum = new float[1024];
AudioListener.GetOutputData(spectrum, 0);
for (int i = 0; i < NumBar; i++)
{
    Vector3 prevScale = Barras[i].transform.localScale;
    prevScale.y = spectrum[i] * 50;
    Barras[i].transform.localScale = prevScale;
}
```
&nbsp; 


8. **Controles de Audio:**
   - <p align="justify">Sliders para ajustar el volumen y el tiempo de la canción, permitiendo al usuario controlar el volumen y la posición de reproducción de forma precisa.</p>

```sh
// Posiciona el slider a la par del momento de la cancion.
public void PosicionSlider()
{
    float nuevaPosicion = TimeLine.value * musica.clip.length;
    musica.time = nuevaPosicion;
}

// Referencia el volumen con la barra slider.
public void Volumen()
{
    musica.volume = volumen.value;
}
```
&nbsp; 


9. **Implementación en Unity:**
   - <p align="justify">Utiliza la interfaz de usuario (UI) de Unity para los controles de selección de canciones.</p>
   - <p align="justify">Todos los elementos están ajustados al canvas para una adaptación óptima a diferentes tamaños de pantalla.</p>

&nbsp; 


10. **Script en C#:**
    - Se ha creado un script en C# para gestionar la reproducción de la música.
    - <p align="justify">Utiliza los componentes AudioSource y AudioListener, que se agregan a un GameObject llamado ControlManager para controlar la reproducción de las canciones.</p>

&nbsp;
## Captura de Pantalla
![1](https://github.com/valen28030/Gramola/assets/167770750/1e1e7112-4cca-44bc-8dc1-df22d5a7e8d5)


&nbsp;
## Conclusión

<p align="justify">La Gramola Musical es una experiencia interactiva que combina la nostalgia de un reproductor de música clásico con la conveniencia de los controles modernos. Con una interfaz intuitiva, controles completos y características avanzadas como la selección aleatoria de canciones y el historial de reproducción, este juego ofrece una experiencia musical inmersiva para todos los jugadores.</p>


&nbsp;
## Créditos

- **Desarrollador**: Carlos Valencia Sánchez
- **Diseñador de Juego**: Carlos Valencia Sánchez
- **Artista Gráfico**: Carlos Valencia Sánchez

&nbsp;
## Contacto

Para obtener soporte técnico, reportar errores o proporcionar comentarios, no dudes en contactar.

¡Prepárate para disfrutar de tus canciones favoritas en un entorno virtual único con la Gramola!

---
<p align="justify">Este documento proporciona una visión general del juego, incluyendo sus características, tecnología utilizada, requisitos del sistema, instrucciones de juego y créditos. Para obtener más información detallada sobre el desarrollo y funcionamiento del juego, consulta la documentación interna o comunícate con el desarrollador.</p>



