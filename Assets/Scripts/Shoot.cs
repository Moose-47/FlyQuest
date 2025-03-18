using UnityEngine;

public class Shoot : MonoBehaviour
{
    
    [SerializeField] private Spore sporePrefab;
    [SerializeField] private Transform sporeSpawn;
    [SerializeField] private BoxCollider2D squish;
    AudioSource audioSource;
    public AudioClip sporeShot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //squish = GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    public void Fire()
    {
        squish.enabled = true;
        Spore curSpore;
        curSpore = Instantiate(sporePrefab, sporeSpawn.position, sporeSpawn.rotation);
        audioSource.PlayOneShot(sporeShot);
    }
}
