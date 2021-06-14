using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    public List<User> users = new List<User>() 
    {
        new User("jatayu", "123456"),
        new User("faid", "123456")
    };

    private enum AuthError
    {
        None,
        UsernameAlreadyExist,
        UserNotFound,
        WrongPassword,
    }

    private AuthError error;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void LoginInput(int _clientId, string _username, string _password)
    {
        Authenticate(_username, _password);

        if(error != AuthError.None)
        {
            string _message = "Login Failed!";
            switch(error)
            {
                case AuthError.WrongPassword:
                    _message = "Wrong password";
                    break;
                case AuthError.UserNotFound:
                    _message = "Account does not exist";
                    break;
            }
            ServerSend.LoginFailed(_clientId, _message);
        }
        else
        {
            if(GameManager.instance.isGameSession)
            {
                ServerSend.LoginFailed(_clientId, "Can't join, the game is already playing");
                return;
            }
            
            if(LobbyManager.instance.IsLobbyFull())
            {
                ServerSend.LoginFailed(_clientId, "Can't join, lobby Is full");
                return;
            }
            
            ServerSend.LoginSuccess(_clientId);
            Debug.Log("Client " + _clientId + " is logged in, welcome " + _username);

            PlayerManager.instance.NewPlayer(_clientId, _username);
            PlayerManager.instance.SendIntoLobby(_clientId);
        }
    }

    public void SignUpInput(int _clientId, string _username, string _password)
    {
        CreateNewUser(_username, _password);
        
        if(error != AuthError.None)
        {
            string _message = "Sign Up Failed!";
            switch(error)
            {
                case AuthError.UsernameAlreadyExist:
                    _message = "Username already exist";
                    break;
            }
            ServerSend.SignUp(_clientId, false, _message);
        }
        else
        {
            string _message = "Sign Up Success";
            ServerSend.SignUp(_clientId, true, _message);
        }
        
    }

    public void Authenticate(string _username, string _password)
    {
        foreach(User _user in users)
        {
            if(_username == _user.username)
            {
                if(_password != _user.password)
                {
                    error = AuthError.WrongPassword;
                    return;
                }
                else
                {
                    error = AuthError.None;
                    return;
                }
            }
        }

        error = AuthError.UserNotFound;
    }

    public void CreateNewUser(string _username, string _password)
    {
        foreach(User _user in users)
        {
            if(_username == _user.username)
            {
                error = AuthError.UsernameAlreadyExist;
                return;
            }
        }

        User _newUser = new User(_username, _password);
        users.Add(_newUser);

        error = AuthError.None;
        return;
    }
}