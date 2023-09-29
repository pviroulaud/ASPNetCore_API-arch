using logEntities.logModel;

namespace log.API.Repositories
{
    public interface ILogRepository
    {
        public void addLog(operationLog op);
        public void addErrorLog(errorLog op);
    }
}
