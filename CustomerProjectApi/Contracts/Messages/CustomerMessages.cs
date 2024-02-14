namespace CustomerProjectApi.Contracts.Messages
{
    public class CustomerCreated
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string GithubUsername { get; set; }
    }

    public class CustomerUpdated
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string GithubUsername { get; set; }
    }

    public class CustomerDeletedId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}