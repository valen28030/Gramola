using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Musica : MonoBehaviour
{
    public Button BotonRandom;
    public AudioSource musica;
    public AudioClip[] canciones;
    public Slider volumen;
    public Slider TimeLine;
    public int cancionsonando;
    public TextMeshProUGUI infoSong;
    public string[] Nombre;
    public string Artista = "Valen Lauren";
    public string[] Fecha;
    public bool Aleatorio = false;
    public int NumBar;
    public GameObject Barra;
    public GameObject[] Barras;
    public List<int> historial;
    public int NumAle;

    // Coloca la barra del volumen, aparece un texto con vacios al iniciar el juego, establece el tiempo de la canción en una barra y trabaja el espectro de onda.
    void Start()
    {
        musica = GetComponent<AudioSource>();
        musica.volume = volumen.value;

        infoSong.text = "Nombre:" + "                      " + "Artista:" + "                          " + "Fecha Lanzamiento";

        TimeLine.minValue = 0;
        TimeLine.maxValue = 1;

        // Espectro de onda.
        float posX = 0;
        for (int i = 0; i < NumBar; i++)
        {
            Instantiate(Barra, new Vector3(posX, 0, 0), Quaternion.identity);
            posX += 0.3f;
        }
        Barras = GameObject.FindGameObjectsWithTag("Player");
    }
   
    // Recoge algunas funciones y trabaja el espectro de onda.
    void Update()
    {
        NextAutomatico();

        ColorBotonRandom();

        CogerNumAle();

        // Espectro de onda.
        float[] spectrum = new float[1024];
        AudioListener.GetOutputData(spectrum, 0);
        for (int i = 0; i < NumBar; i++)
        {
            Vector3 prevScale = Barras[i].transform.localScale;
            prevScale.y = spectrum[i] * 50;
            Barras[i].transform.localScale = prevScale;
        }
    }

    // Para la canción que esta sonando en ese momento y reproduce la nueva canción pulsada.
    public void Reproducir(int id)
    {
        musica.Stop();
        musica.clip = canciones[id];
        cancionsonando = id;
        TimeLine.value = 0;
        historial.Add(cancionsonando);
        musica.Play();
    }

    // Pausa la canción.
    public void Pause()
    {
        musica.Pause();
    }

    // Continua la canción y la deja a velocidad normal.
    public void Play()
    {
        musica.Play();
        musica.pitch = 1; 
    }

    // Para la canción y la devuelve al segundo 0.
    public void Stop()
    {
       musica.Stop();  
    }

    // Referencia el volumen con la barra slider.
    public void Volumen()
    {
        musica.volume = volumen.value;
    }

    // Reproduce la canción a velodidad media.
    public void Avanzar()
    {
        musica.pitch = 0.5f;
    }

    // Reproduce la canción a doble velodidad.
    public void Avanzarx2()
    {
        musica.pitch = 2;
    }

    // Rebobina lento.
    public void Rebobinar()
    {
        musica.pitch = -0.5f;
    }

    // Rebobina rápido.
    public void Rebobinarx2()
    {
        musica.pitch = -1;
    }

    // Pasa de cancion automaticamente cuando llega al final.
    public void NextAutomatico()
    {
        if (musica.clip != null)
        {
            TimeLine.value = musica.time / musica.clip.length;
            if (musica.isPlaying && musica.time >= musica.clip.length - 0.1f) // 0.1f es un pequeño margen para asegurarse de que hemos llegado al final.
            {
                Next();
            }
        }
    }

    // Pasa a la siguiente canción detectando si esta en normal o en aleatorio.
    public void Next()
    {
        if (!Aleatorio)
        {
            cancionsonando++;
            if (cancionsonando >= canciones.Length)
            {
                cancionsonando = 0;
            }
        }
        else 
        {
            cancionsonando = Random.Range(0, canciones.Length);
        }
        Reproducir(cancionsonando);
        infoSong.text = "Nombre: " + Nombre[cancionsonando] + "     " + "Artista: " + Artista + "     " + "Fecha Lanzamiento: " + Fecha[cancionsonando];
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

    // Introduce los datos correpondientes de texto para cada canción.
    public void CancionSeleccionada(int id)
    {
        infoSong.text = "Nombre: " + Nombre[id] + "     " + "Artista: " + Artista + "     " + "Fecha Lanzamiento: " + Fecha[id];
    }

    // Activa y desactiva el aleatorio para el botón de random.
    public void ActivarDesactivarAleatorio()
    {
        Aleatorio = !Aleatorio;
    }

    // Posiciona el slider a la par del momento de la cancion.
    public void PosicionSlider()
    {
        float nuevaPosicion = TimeLine.value * musica.clip.length;
        musica.time = nuevaPosicion;
    }

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

    // Coge la cancion anterior 
    public void CogerNumAle() 
    {
        if (historial.Count >= 2) { NumAle = historial[historial.Count - 2]; }
    }
}


