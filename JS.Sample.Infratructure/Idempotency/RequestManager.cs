using JS.Sample.Application.Interfaces;
using JS.Sample.Domain;
using JS.Sample.Infratructure.Exceptions;
using JS.Sample.Percistance;
using System;
using System.Threading.Tasks;

namespace JS.Sample.Infratructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly SampleDbContext _context;

        public RequestManager(SampleDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.
                FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task UpdateRequest(Guid id, long contentId)
        {
            try
            {
                var request = await _context.FindAsync<ClientRequest>(id).ConfigureAwait(false);
                if (request != null)
                {
                    request.ContentId = contentId;
                    await _context.SaveChangesAsync();
                }

            }
            catch
            {

            }

        }
        public async Task AddRequestForCommandAsync(string Name, long contentId, string content)
        {
            try
            {

                var request = new ClientRequest()
                {
                    Id = new Guid(),
                    Name = Name,
                    ContentId = contentId,
                    Content = content,
                    Time = DateTime.UtcNow
                };

                _context.Add(request);

                await _context.SaveChangesAsync();
            }
            catch
            {

            }

        }
        public async Task CreateRequestForCommandAsync<T>(Guid id, long contentId, string content)
        {
            try
            {
                var exists = await ExistAsync(id);

                var request = exists ?
                    throw new ApiException($"Request with {id} already exists") :
                    new ClientRequest()
                    {
                        Id = id,
                        Name = typeof(T).Name,
                        ContentId = contentId,
                        Content = content,
                        Time = DateTime.UtcNow
                    };

                _context.Add(request);

                await _context.SaveChangesAsync();
            }
            catch
            {

            }

        }


    }
}
