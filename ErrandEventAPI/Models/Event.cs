using System.ComponentModel.DataAnnotations;

namespace ErrandEventAPI.Models
{
    public enum EVENT_TYPE
    {
        WEDDING,
        PARTY,
        CONCERT,
        FESTIVAL,
        MEETING,
        PARADE
    }
    public class UserMapping
    {
        public int Id { get; set; }
    }
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public EVENT_TYPE EventType { get; set; }
        public int EventDescription { get; set; }
        public List<UserMapping> Users { get; set; }

    }
}
