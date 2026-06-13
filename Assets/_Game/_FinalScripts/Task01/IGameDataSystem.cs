// This interface defines the structural contract for managing player accounts and storage updates in a database system
public interface IGameDataSystem
{
    // Method layout that requires implementing classes to handle initial profile generation using tracking strings
    void CreateUserRecord(string userID, string email, string name);

    // Method layout that requires implementing classes to look up a player account and overwrite its numerical value
    void UpdateScore(string userID, int newScore);
}