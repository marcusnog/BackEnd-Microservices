namespace Ms.Api.Utilities.Models
{
    public abstract class ControlledEntity<T> 
    {
        public abstract T Id { get; set; }
        public abstract double CreationDate { get; set; }
        public abstract T CreationUserId { get; set; }
        public abstract double? DeletionDate { get; set; }
        public abstract T DeletionUserId { get; set; }
        public abstract bool Active { get; set; }
    }
}
