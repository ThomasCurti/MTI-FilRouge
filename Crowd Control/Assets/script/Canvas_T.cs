using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_T : MonoBehaviour
{
    public GameObject txt;

    public GameObject TiboInShape;
    public GameObject Roger;
    // Start is called before the first frame update

    public int Total_manif;
    public int Total_policier;

    //Thread safe
    public static Object b = new Object();
    public static int score = 0;
    
    public int nb_manif_escape;

    public GameObject End_Canvas;

    private int canvas_count;

    //lethal mode
    private bool lethalMode;
    private float timeLethalMode;
    private float limitTimeLethalMode;
    private float timeBeforeNextLethalMode;

    void Start()
    {
        End_Canvas.SetActive(false);
        score = 0;
        lethalMode = false;
        timeLethalMode = 0;
        limitTimeLethalMode = 10;
        timeBeforeNextLethalMode = 60;
        Dialog();
    }

    // Update is called once per frame
    void Update()
    {
        End_Canvas.SetActive(false);

        if (lethalMode && Time.fixedTime >= timeLethalMode + limitTimeLethalMode)
        {
            lethalMode = false;
            timeLethalMode = Time.fixedTime;
            agent.ViolenceScale = 0;
            StartCoroutine(parole(Roger, "On n'a plus de balle ! Attendez le ravitaillement", "Roger", 2));
        }

        setscore();
        End_condition();
 ;    }

    private  void setscore()
    {
        txt.GetComponent<UnityEngine.UI.Text>().text = "Compteur de dommages collatéraux: " + score;
        
        if (score <= 1 && score > 25 && canvas_count == 1)
        {
            StartCoroutine(parole(TiboInShape, "Rien de tel pour impressioner les petites", "TiboInShape"));
            canvas_count++;
        }
        if (score <= 25 && score > 40 && canvas_count == 2)
        {
            StartCoroutine(parole(Roger, "N'hésite pas à viser entre les deux yeux Tibo", "Roger"));
            canvas_count++;
        }
        if (score <= 40 && canvas_count == 3)
        {
            StartCoroutine(parole(TiboInShape, "N'oubliez pas, la team Shape, de laisser un max de pouce bleu", "TiboInShape"));
            canvas_count++;
        }

    }

    private void Dialog()
    {
        TiboInShape.SetActive(false);
        Roger.SetActive(false);
        StartCoroutine(dialog());
    }

    private IEnumerator dialog()
    {
        yield return new WaitForSeconds(1f);
        TiboInShape.SetActive(true);
        TiboInShape.GetComponentInChildren< UnityEngine.UI.Text>().text = "TiboInShape : \n Damn les gens, aujourd'hui je suis avec la police Nationale pour vidéo très spéciale. Dans cette vidéo, je serai accompagné du sergent Roger. Damn sergent !"; yield return new WaitForSeconds(5f);
        TiboInShape.SetActive(false);
        Roger.SetActive(true);
        Roger.GetComponentInChildren<UnityEngine.UI.Text>().text = "Roger :\n Damn Tibo, aujourd'hui journée très spéciale car nous allons \" encadrer\" une manifestation.";
        yield return new WaitForSeconds(4f);
        TiboInShape.SetActive(true);
        Roger.SetActive(false);
        TiboInShape.GetComponentInChildren<UnityEngine.UI.Text>().text = "TiboInShape :\n Alors c'est parti !";
        yield return new WaitForSeconds(4f);
        TiboInShape.SetActive(false);
        Roger.SetActive(false);
        canvas_count = 1;
    }

    private IEnumerator parole(GameObject text, string d , string name, float duration = 4f)
    {
        text.SetActive(true);
        TiboInShape.GetComponentInChildren<UnityEngine.UI.Text>().text = name + "\n" + d;
        yield return new WaitForSeconds(duration);
        text.SetActive(false);
    }

    private void End_condition()
    {
        if(Total_manif <= nb_manif_escape + score)
        {
            End_Canvas.SetActive(true);
            End_Canvas.GetComponentInChildren<UnityEngine.UI.Text>().text = "Victoire \n Score : " + score;
        }

        if (Total_policier <= 0)
        {
            End_Canvas.SetActive(true);
            End_Canvas.GetComponentInChildren<UnityEngine.UI.Text>().text = "Defaite \n Score : " + score;
        }


    }


    public void LethalMode()
    {
        if (lethalMode)
            return;

        if (Time.fixedTime <= timeLethalMode + timeBeforeNextLethalMode)
        {
            StartCoroutine(parole(TiboInShape, "On n'a pas de balle chef, on doit attendre !", "Soldat", 2));
            return;
        }
        lethalMode = true;
        timeLethalMode = Time.fixedTime;
        agent.ViolenceScale = (agent.ViolenceScale + 1) % 2;
        StartCoroutine(parole(Roger, "Autorisation de tuer, je répète autorisation de tuer.", "Roger", 2));
    }
}
