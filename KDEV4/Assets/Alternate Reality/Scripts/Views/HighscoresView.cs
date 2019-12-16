using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace alternatereality
{
    public class HighscoresView : BaseView
    { 
        [SerializeField] private Button _buttonBack;
        [SerializeField] private Button _buttonRefresh;
        [SerializeField] private Button _buttonAll;
        [SerializeField] private Button _buttonWeekly;
        [SerializeField] private Button _buttonMonthly;
        [SerializeField] private Button _buttonPersonal;
        [SerializeField] private Transform _content;
        [SerializeField] private TMP_Text _labelInfo;
        
        private string _type;


        public override void SetVisible()
        {
            base.SetVisible();

            _type = "get_all.php";

            foreach (Transform entry in _content)
                Destroy(entry.gameObject);

            _buttonBack.onClick.AddListener(() => ViewManagement.Instance.SetView("MainView"));
            _buttonRefresh.onClick.AddListener(() => StartCoroutine(Get(_type)));
            _buttonAll.onClick.AddListener(() => StartCoroutine(Get("get_all.php")));
            _buttonWeekly.onClick.AddListener(() => StartCoroutine(Get("get_weekly.php")));
            _buttonMonthly.onClick.AddListener(() => StartCoroutine(Get("get_monthly.php")));
            _buttonPersonal.onClick.AddListener(() => StartCoroutine(Get("get_personal.php")));

            _buttonRefresh.interactable = true;
            _buttonAll.interactable = true;
            _buttonWeekly.interactable = true;
            _buttonMonthly.interactable = true;
            _buttonPersonal.interactable = true;

            _labelInfo.text = "";

            EventSystem.current.SetSelectedGameObject(_buttonAll.gameObject);

            StartCoroutine(Get("get_all.php"));
        }


        private IEnumerator Get(string type)
        {
            _type = type;

            _buttonRefresh.interactable = false;
            _buttonAll.interactable = false;
            _buttonWeekly.interactable = false;
            _buttonMonthly.interactable = false;
            _buttonPersonal.interactable = false;

            _labelInfo.text = "";

            UnityWebRequest request;

            if (type == "get_personal.php")
            {
                WWWForm form = new WWWForm();

                form.AddField("username", PhotonNetwork.LocalPlayer.NickName);
                request = UnityWebRequest.Post(Data.SERVER_URL + _type, form);
            }
            else
                request = UnityWebRequest.Get(Data.SERVER_URL + _type);

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                InformationPopup popup = PopupManagement.Instance.Add("PopupInformation") as InformationPopup;

                popup.Initialize("Oops!", "Unfortunately the highscores were unable to be retrieved from the server.\r\nError: " + request.error);
            }
            else
            {
                foreach (Transform entry in _content)
                    Destroy(entry.gameObject);

                if (request.downloadHandler.text != "-1")
                {
                    JSONNode json = JSON.Parse(request.downloadHandler.text);

                    for (int i = 0; i < json.Count; i++)
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("UIHighscoreEntry"), _content);
                        UIHighscoreEntry ui = go.GetComponent<UIHighscoreEntry>();

                        ui.Initialize(i + 1 + ".", json[i]["username"], json[i]["score"]);
                    }
                }
                else
                {
                    _labelInfo.text = "No records available.";
                }
            }

            _buttonRefresh.interactable = true;
            _buttonAll.interactable = true;
            _buttonWeekly.interactable = true;
            _buttonMonthly.interactable = true;
            _buttonPersonal.interactable = true;
        }


        public override void SetHidden()
        {
            base.SetHidden();

            _buttonBack.onClick.RemoveAllListeners();
            _buttonRefresh.onClick.RemoveAllListeners();
            _buttonAll.onClick.RemoveAllListeners();
            _buttonWeekly.onClick.RemoveAllListeners();
            _buttonMonthly.onClick.RemoveAllListeners();
            _buttonPersonal.onClick.RemoveAllListeners();

            StopAllCoroutines();
        }
    }
}