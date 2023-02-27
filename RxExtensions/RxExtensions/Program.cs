
namespace RxExtensions
{
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var subject = new Subject<Notification>();
            subject
                .Buffer(TimeSpan.FromSeconds(6))
                .Subscribe(notifications => Console.WriteLine($"Received {notifications?.Count} notifications"));

            for (int i = 0; i < 10; i++)
            {
                if (i != 0 && i % 4 == 0)
                {
                    Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                }

                int projectId = (i % 2 == 0) ? 1 : (i % 3 == 0) ? 2 : (i % 5 == 0) ? 3 : i;
                var notification = new Notification { ProjectId = projectId, Attributes = new NotificationAttributes { Id = i, Name = $"Name {i}" } };

                Console.WriteLine($"Raising notificaion: {notification}");

                if (notification != null)
                {
                    subject.OnNext(notification);
                }

                Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            }
        }
    }

    public class Notification
    {
        public int ProjectId { get; set; }
        public NotificationAttributes? Attributes { get; set; }

        public override string ToString()
        {
            return $"{DateTime.Now}: {ProjectId}, {Attributes?.Id}";
        }
    }

    public class NotificationAttributes
    {
        public int Id { get; set; } = 0;
        public string? Name { get; set; }
    }
}