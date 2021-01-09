namespace LogFrog.Telegram.Users
{
    public interface IUserSettingRepository
    {
        UserSettings GetSettings(int userId);
    }
}