namespace AppWithS3.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
