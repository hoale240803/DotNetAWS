namespace CustomerProjectApi.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
