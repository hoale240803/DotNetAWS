using ConsumerApi.Messages;
using MediatR;

namespace ConsumerApi.Handlers
{
    public class CustomerDeletedIdHandler : IRequestHandler<CustomerDeletedId>
    {
        public Task Handle(CustomerDeletedId request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}