using fileDTO;

namespace documentAPI.SyncServices
{
    public interface IHttpLocalClient
    {
        Task<bool> checkUserPermit(int userId,int permitId);
        Task<userFileDTO?> getFile(int fileId);
    }
}
