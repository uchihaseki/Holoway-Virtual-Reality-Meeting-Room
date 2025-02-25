using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CastRay;
using static LoginSession;
using TMPro;
using static ID;
using holowayapi;
public class AccessDrive : MonoBehaviour
{

    [SerializeField]
    private Material InterfaceMaterial;
    [SerializeField]
    private GameObject ButtonPrefab;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject Content;

    public static string userSharingToken;
    public static string userSharingID;

    public static List<string> filesName;
    public static List<string> filesID;

    private string api_key;
    private string priv_key;

    // Update is called once per frame
    void Update()
    {
        // Check if the player has hit the magic sphere to share them all.
        if (Input.GetMouseButtonDown(0) && CastRay.hasHit("ShareFileInterface", Camera.main))
        {
            // Get the API parameters.
            this.api_key = "N7e9vMq3BMmD84XxwUI4Vhq1snt7iBe8";
            this.priv_key = Credentials.creds[api_key];

            // Create the api instance and pass the parameters.
            GameObject APIManager = GameObject.Find("APIManager");
            HolowayAPI api = APIManager.AddComponent<HolowayAPI>();
            api.add_params(priv_key, api_key);

            // Get the current player token.
            string player_token = PlayerPrefs.GetString("player_token");

            // Get User who clicked
            userSharingToken = LoginSession.token;
            userSharingID = LoginSession.user_id;

            // Enable Canvas
            canvas.SetActive(true);

            // Change Material
            GameObject.Find("ShareFileInterface").GetComponent<MeshRenderer>().material = InterfaceMaterial;

            // Make the actual API call, althou the result will be received by ListFiles.
            StartCoroutine(api.list_files("", userSharingToken, ListFiles));
        }
    }

    void ListFiles(string status, List<HoloFile> files, string msg)
    {
        // Check if the call succeeded.
        if (status != "success")
        {
            Debug.Log("ERROR");
            return;
        }

        // Init the lists of ids and names.
        List<string> ids = new List<string>() {};
        List<string> names = new List<string>() {};

        // Iterate through files.
        foreach (HoloFile file in files) 
        {   
            Debug.Log(file.id);
            ids.Add(file.id);
            names.Add(file.name);
        }
        
        for(int i = 0; i < names.Count; i++) {
            GameObject fileButton = Instantiate(ButtonPrefab, new Vector3(0.0f,0.0f, 0.0f), Quaternion.identity);
            fileButton.GetComponent<ID>().id = ids[i];
            fileButton.transform.GetChild(0).GetComponent<TMP_Text>().text = names[i];
            fileButton.transform.SetParent(Content.transform, false);
        }
        //AddItemsToList(names, ids);
    }

    private void AddItemsToList(List<string> _filesName, List<string> _filesID)
    {
        GameObject Content = GameObject.Find("Content");
        for(int i = 0; i < _filesName.Count; i++) {
            GameObject fileButton = Instantiate(ButtonPrefab, new Vector3(0.0f,0.0f, 0.0f), Quaternion.identity);
            fileButton.GetComponent<ID>().id = _filesID[i];
            fileButton.transform.GetChild(0).GetComponent<TMP_Text>().text = _filesName[i];
            fileButton.transform.SetParent(Content.transform, false);
        }
    }
    
}
