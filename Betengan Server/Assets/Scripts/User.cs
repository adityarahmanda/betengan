[System.Serializable]
public class User
{
   public string username;
   public string password;

   public User(string _username, string _password)
   {
       username = _username;
       password = _password;
   }
}