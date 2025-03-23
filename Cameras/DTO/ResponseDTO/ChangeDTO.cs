namespace BackEnd.Cameras.DTO.ResponseDTO
{
    public class ChangeDTO
    {
        public string User { get; set; } = null!;

        public string ObjectId { get; set; } = null!;

        public long CreatedAt { get; set; }

        // event_data: changed_data: 

        public string ActionType { get; set; } = null!; // change | delete | create

        public string ObjectType { get; set; } = null!; // название класса

        public RequestData RequestData { get; set; } = null!;
    }

    public class RequestData
    {
        public string Ip { get; set; } = null!;

        public string UserAgent { get; set; } = null!;
    }
}
