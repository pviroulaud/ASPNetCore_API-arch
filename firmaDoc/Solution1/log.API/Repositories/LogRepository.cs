using logDTO;
using logEntities.logModel;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace log.API.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LogRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

        }
        public void addLog(operationLog op)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _context.operationLog.Add(op);
                _context.SaveChanges();
            }

            
        }
        public void addErrorLog(errorLog err)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _context.errorLog.Add(err);
                _context.SaveChanges();
            }
            
        }
    }
}
