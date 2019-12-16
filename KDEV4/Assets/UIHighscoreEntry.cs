using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace alternatereality
{
    public class UIHighscoreEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text _position;
        [SerializeField] private TMP_Text _names;
        [SerializeField] private TMP_Text _score;


        public void Initialize(string position, string label, string score)
        {
            _position.text = position;
            _names.text = label;
            _score.text = score;
        }
    }
}