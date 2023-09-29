using AutoMapper;
using documentDTO;
using fileDTO;
using fileEntities.fileModel;
using helpers.cipher;

using System.Text.Json;

namespace fileAPI.AsyncServices
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly IRabbitMQClient<userDocumentDTO> _rabbitMQFileInfoClient;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper,IRabbitMQClient<userDocumentDTO> rabbitMQFileInfoClient)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _rabbitMQFileInfoClient = rabbitMQFileInfoClient;
        }

        public void ProcessEvent(string message)
        {
            addFile(message);
        }


        private void addFile(string fileMessage)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var ctxt= scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var fMsg = JsonSerializer.Deserialize<rabbitMqFileTransferDTO>(fileMessage);

                byte[] arr = new byte[fMsg.size];
                
                if (fMsg.cipher)
                {
                    string b64 = Convert.ToBase64String(arr);
                    arr = Cipher.EncriptarValorBytes(AES.ClaveDocumentos("999"), b64);
                }
                else
                {
                    arr = Convert.FromBase64String(fMsg.b64File);
                }
                var file = new userFile() 
                {
                    fileName=fMsg.fileName,
                    size=fMsg.size,
                    contentType=fMsg.contentType,
                    content = arr,
                    cipher =fMsg.cipher,
                    userId=fMsg.userId
                };
                ctxt.userFile.Add(file);
                ctxt.SaveChanges();

                userDocumentDTO fileReference = new userDocumentDTO()
                {
                    id=fMsg.documentId,
                    userFileId=file.id,
                    userFileStorageDate=DateTime.Now
                };

                _rabbitMQFileInfoClient.sendMessage(fileReference);
            }

        }
    }
}

