namespace BackEnd.Users.Utils
{
    public class UserStorage
    {
        public static List<User> Users = new List<User>
        {
            new User { Login = "user1", Password = "password1"},
            new User { Login = "user2", Password = "password2" }
        };

        public static async Task<User> GetUser(string login, string password)
        {
            return await Task.Run(() => Users.FirstOrDefault(u => u.Login == login && u.Password == password));
        }

        public static async Task<User?> CreateUser(string login, string password)
        {
            return await Task.Run(async () =>
            {
                if (Users.FirstOrDefault(u => u.Login == login) is not null)
                    return null;

                var newUser = new User { Login = login, Password = password };
                Users.Add(newUser);
                // await TaskStorage.CreateUserStorage(username);
                return newUser;
            });
        }
    }
}
