using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountManagerScript : MonoBehaviour
{
    #region UI objects

    // Login panel objs
    public InputField inputUsername;
    public InputField inputPassword;

    // Status texts
    public Text statusMessage;
    public Text statusMessageNewAccount;

    // New account panel objs
    public GameObject pnlNewAccount;
    public InputField inputNewUsername;
    public InputField inputNewPassword;

    #endregion

    // This dictionary stores all login info and should be updating to server
    private Dictionary<string, string> loginInfo = new Dictionary<string, string>();

    // Keep this object to load other scenes with correct account info
    private void Awake()
    {
        DontDestroyOnLoad(transform);
    }

    // Use this for initialization
    void Start()
    {
        statusMessage.text = "";
        pnlNewAccount.SetActive(false);
        inputUsername.text = "";
        inputPassword.text = "";
    }

    #region OnGUIs

    // Show new account panel
    public void OnBtnNewAcccountPressed()
    {
        // Show panel and set inputs empty
        inputUsername.text = "";
        inputPassword.text = "";
        pnlNewAccount.SetActive(true);
    }

    // Create new account if possible. If not show error.
    public void OnBtnCreateAccountPressed()
    {
        bool isAbleToCreateNewAccount = false;

        // Check if username isn't blank
        if (inputNewUsername.text.Equals(""))
        {
            statusMessageNewAccount.text = "Enter your new username!";
            isAbleToCreateNewAccount = false;
            Debug.Log("1");
        }
        // Check if password isn't blank
        else if (inputNewPassword.text.Equals(""))
        {
            statusMessageNewAccount.text = "Enter your new password";
            isAbleToCreateNewAccount = false;
            Debug.Log("2");
        }
        // Check if username is already taken
        else if (loginInfo.ContainsKey(inputNewUsername.text))
        {
            statusMessageNewAccount.text = "That username is already taken.";
            isAbleToCreateNewAccount = false;
            Debug.Log("3");
        }
        else
        {
            isAbleToCreateNewAccount = true;
        }

        // If login info is valid add new account
        if (isAbleToCreateNewAccount)
        {
            CreateNewAccount(inputNewUsername.text, inputNewPassword.text);
            statusMessageNewAccount.text = "Account created!";
        }
    }

    // Hide new account panel
    public void OnCloseNewAccountPanel()
    {
        pnlNewAccount.SetActive(false);
    }

    // If login info is valid, log in and open lobby scene
    public void OnPlayButtonPressed()
    {
        bool isAbleToPlay = false;
        string thisUsername = inputUsername.text;
        string thisPassword = inputPassword.text;

        // Check if username is blank
        if (thisUsername.Equals("") || thisUsername.Equals(null))
        {
            statusMessage.text = "Enter your username!";
            isAbleToPlay = false;
        }
        // Check if password is blank
        else if (thisPassword.Equals("") || thisPassword.Equals(null))
        {
            statusMessage.text = "Enter your password!";
            isAbleToPlay = false;

        }
        // Check if username and its corresponding password exists
        else if (loginInfo.TryGetValue(thisUsername, out thisPassword)) // Verify account
        {
            // Account exists
            isAbleToPlay = true;
        }
        // Incorrect username/password
        else
        {
            statusMessage.text = "Account not found";
            isAbleToPlay = false;
        }

        if (isAbleToPlay) // Load lobby scene
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }

    #endregion 

    // Create new account with given params
    private void CreateNewAccount(string username, string password)
    {
        loginInfo.Add(username, password);
    }


}
