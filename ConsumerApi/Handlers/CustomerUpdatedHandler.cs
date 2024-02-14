using ConsumerApi.Messages;
using MediatR;

namespace ConsumerApi.Handlers
{
    public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
    {
        public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}