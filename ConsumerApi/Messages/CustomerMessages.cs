namespace ConsumerApi.Messages
{
    public class CustomerCreated : ISqsMessage
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string GithubUsername { get; set; }
    }

    public class CustomerUpdated : ISqsMessage
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string GithubUsername { get; set; }
    }

    public class CustomerDeletedId : ISqsMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}