using UnityEngine;
using UnityEngine.UI;

public class MesajUI : MonoBehaviour
{
    public Text textMesaj;

    void Start()
    {
        // Exemplu de mesaj pe ecran când începe jocul
        AfiseazaMesaj("Jocul a început!");
    }

    void OnTriggerEnter(Collider other)
    {
        // Exemplu de mesaj pe ecran la intrarea într-un trigger
        AfiseazaMesaj("Ai intrat în trigger!");
    }

    public void AfiseazaMesaj(string mesaj)
    {
        // Setează textul mesajului pe ecran
        textMesaj.text = mesaj;

        // Oprește afișarea mesajului după 2 secunde (opțional)
        Invoke("StergeMesaj", 2f);
    }

    void StergeMesaj()
    {
        // Șterge textul mesajului după 2 secunde
        textMesaj.text = "";
    }
}
