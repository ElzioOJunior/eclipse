
namespace EclipseApi.Domain.Entities
{
    public class Project: BaseEntity
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }

}
