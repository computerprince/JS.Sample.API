using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.CommandStack
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api
    /// This is implemented by wrapping the business command (in this case CreateCouponCommand) 
    /// and embeding it into a generic IdentifiedCommand which is tracked by an ID of every message coming through the network that has to be idempotent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    public class IdentifiedCommand<T, R> : IRequest<R>
       where T : IRequest<R>
    {
        /// <summary>
        /// Command Object
        /// </summary>
        public T Command { get; }
        /// <summary>
        /// ID of every message coming through
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// User Id, who is making request
        /// </summary>
        //public long UserId { get; set; }

        /// <summary>
        /// Content Id, of the request
        /// </summary>
        public long ContentId { get; set; }

        /// <summary>
        /// Content of the request
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// Constructor for update
        /// </summary>
        /// <param name="command"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="contentId"></param>
        /// <param name="content"></param>
        public IdentifiedCommand(T command, Guid id, long contentId)
        {
            Command = command;
            Id = id;
            Content = JsonConvert.SerializeObject(command);
            ContentId = contentId;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedCommand{T, R}"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="actionby">The actionby.</param>
        /// <param name="actionNameBy">The action name by.</param>
        public IdentifiedCommand(T command, Guid id)
        {
            Command = command;
            Id = id;
            Content = JsonConvert.SerializeObject(command);

        }


    }
}
