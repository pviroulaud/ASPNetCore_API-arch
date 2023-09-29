using AutoMapper;
using documentDTO;
using documentEntities.documentModel;
using System.Text.Json;

namespace documentAPI.AsyncServices
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
            completeDocumentInfo(message);
        }


        private void completeDocumentInfo(string fileMessage)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var ctxt= scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var fMsg = JsonSerializer.Deserialize<userDocumentDTO>(fileMessage);

                var doc = (from d in ctxt.document where d.id == fMsg.id select d).FirstOrDefault();
                if (doc!=null)
                {
                    if (fMsg.userFileId!= null)
                    {
                        doc.userFileId = fMsg.userFileId;
                        doc.userFileStorageDate = fMsg.userFileStorageDate;
                    }
                    

                    if (fMsg.signedUserFileId!= null)
                    {
                        doc.signedUserFileId = fMsg.signedUserFileId;
                        doc.userFileSignDate = fMsg.userFileSignDate;
                    }
                    ctxt.document.Update(doc);
                    ctxt.SaveChanges();
                }
            }

        }
    }
}

