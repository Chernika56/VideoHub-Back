namespace BackEnd.Streamers.DTO
{
    public class StreamerResponseDTO
    {
        public uint Id { get; set; }

        public string HostName { get; set; } = null!;

        public string ApiUrl { get; set; } = null!;

        public uint Port { get; set; }

        public bool IsLocal { get; set; }

        public string DVRPath { get; set; } = null!;
    }
}
