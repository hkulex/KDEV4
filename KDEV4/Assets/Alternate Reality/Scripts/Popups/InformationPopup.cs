using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class InformationPopup : BasePopup
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _buttonConfirm;


        public void Initialize(string title, string description)
        {
            base.Initialize();

            _title.text = title;
            _description.text = description;
            _buttonConfirm.onClick.AddListener(OnButtonConfirmClicked);
        }


        private void OnButtonConfirmClicked()
        {
            PopupManagement.Instance.Remove(this);
        }


        public override void Dispose()
        {
            base.Dispose();

            _buttonConfirm.onClick.RemoveAllListeners();
        }
    }
}