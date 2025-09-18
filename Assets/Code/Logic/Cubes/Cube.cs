using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("Cube Components")]
    [SerializeField] private Rigidbody rigidbody;
    
    public Rigidbody Rigidbody => rigidbody;
}
