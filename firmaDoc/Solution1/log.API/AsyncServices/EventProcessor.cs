using AutoMapper;
using logDTO;
using logEntities.logModel;
using System.Text.Json;

namespace log.API.AsyncServices
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var messageType = DetermineMessageType(message);

            switch (messageType)
            {
                case "operationLog":
                    addOperationLog(message);
                    break;
                case "errorLog":
                    addErrorLog(message);
                break;
                default:
                    break;
            }
        }

        private string DetermineMessageType(string msg)
        {
            try
            {
                messageTypeDTO msgType= JsonSerializer.Deserialize<messageTypeDTO>(msg);
                return msgType.messageType;
            }
            catch (Exception ex)
            {
                return "";
            }
           
        }

        private void addErrorLog(string errorMessage)
        {
             using (var scope = _scopeFactory.CreateScope())
            {
                var ctxt= scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var error = JsonSerializer.Deserialize<errorDTO>(errorMessage);
                var err = _mapper.Map<errorLog>(error);
                ctxt.errorLog.Add(err);
                ctxt.SaveChanges();
            }

            
        }
        private void addOperationLog(string operationMessage)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var ctxt= scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var operation = JsonSerializer.Deserialize<operationDTO>(operationMessage);
                var op = _mapper.Map<operationLog>(operation);
                ctxt.operationLog.Add(op);
                ctxt.SaveChanges();
            }

            // using (var scope = _scopeFactory.CreateScope())
            // {
            //     var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            //     var operation = JsonSerializer.Deserialize<operationDTO>(operationMessage);

            //     try
            //     {
            //         var plat = _mapper.Map<Platform>(platformPublishedDto);
            //         if (!repo.ExternalPlatformExists(plat.ExternalID))
            //         {
            //             repo.CreatePlatform(plat);
            //             repo.SaveChanges();
            //             Console.WriteLine("--> Platform added!");
            //         }
            //         else
            //         {
            //             Console.WriteLine("--> Platform already exisits...");
            //         }

            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            //     }
            // }
        }
    }
}

