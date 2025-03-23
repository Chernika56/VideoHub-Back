namespace BackEnd.DB.Entities
{
    public class AgentsEntity
    {
        public uint Id { get; set; }

        public virtual ICollection<CamerasEntity>? Cameras { get; set; } = new List<CamerasEntity>();


        public string? Key { get; set; }

        public string? Model { get; set; }

        public string? Serial { get; set; }

        public string? Status { get; set; }
    }
}
