using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace JS.Sample.Application.Interfaces
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task UpdateRequest(Guid id, long contentId);

        Task AddRequestForCommandAsync(string Name, long contentId, string content);

        Task CreateRequestForCommandAsync<T>(Guid id, long contentId, string content);

    }
}
