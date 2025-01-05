namespace BackEnd.Video.Utils
{
    public class VideoStorage
    {
        public static List<Video> Videos = new List<Video>
        {
            new Video { VideoName = "Never Gona Give You Up", VideoPath = "video"}
        };

        public static async Task<Video> GetVideoByName(string name)
        {
            return await Task.Run(() => Videos.FirstOrDefault(v => v.VideoName == name));
        }

        public static async Task<Video?> CreateVideo(string name, string path)
        {   
            return await Task.Run(async () =>
            {
                if (Videos.FirstOrDefault(v => v.VideoName == name) is not null)
                    return null;

                var newVideo = new Video { VideoName = name, VideoPath = path };
                Videos.Add(newVideo);
                // await TaskStorage.CreateUserStorage(username);
                return newVideo;
            });
        }
    }
}
