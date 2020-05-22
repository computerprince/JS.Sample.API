using JS.Sample.Common.Models;
using MediatR;

namespace JS.Sample.CommandStack
{
    public class StatusProductCommand : IRequest<CommonResponse>
    {
        public long Id { get; set; }
        public bool IsAvailable { get; set; }
    }
}
