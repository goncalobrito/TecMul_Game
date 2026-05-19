using UnityEngine;

public class teste : MonoBehaviour , IInteragivel
{    
    public string TextoInteracao() =>  "Apanhar";

    public void Interagir()
    {
        ColorManager cm = FindObjectOfType<ColorManager>();
        cm.AlternarDisco();
    }
}