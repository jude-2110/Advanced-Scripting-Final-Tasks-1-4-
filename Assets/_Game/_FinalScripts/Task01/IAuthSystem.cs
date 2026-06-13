using System;

// This interface defines the mandatory blueprint contract that any authentication system must follow
public interface IAuthSystem
{
    // Method layout that requires implementing classes to handle account registration with tracking notifications
    void SignUp(string email, string password, string displayName, System.Action<string> onSuccess);

    // Method layout that requires implementing classes to verify existing account access keys
    void Login(string email, string password);

    // Method layout that requires implementing classes to pass back the active account identifier text
    string GetUserID();
}