namespace SQSPublisher
{
    public class CustomerCreated
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string GithubUsername { get; set; }
    }
}