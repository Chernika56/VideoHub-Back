using System.Runtime.CompilerServices;

namespace MovieApi.DTO
{
    public class MSRespone
    {
        public int? Code { get; set; }
        public string? Message { get; set; }
        public Object? Data { get; set; }

        public MSRespone() { }
        public MSRespone(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
