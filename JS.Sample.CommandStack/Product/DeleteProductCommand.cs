using JS.Sample.Common.Models;
using MediatR;

namespace JS.Sample.CommandStack
{
    public class DeleteProductCommand : IRequest<CommonResponse>
    {
        public long Id { get; set; }

    }
}
