using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code.Logic.Cubes
{
    public class CubeView : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> _texts;
        [SerializeField] private MeshRenderer _meshRenderer;

        public void Initialize(int value, Color color)
        {
            foreach (TMP_Text tmpText in _texts) 
                tmpText.text = value.ToString();
            
            _meshRenderer.material.color = color;
        }
    }
}