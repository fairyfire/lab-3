
namespace ForJohnny
{
    /// <summary> Сбор и вывод статиски о ходе игры </summary>
    static class Debug
    {
        private static GameManager game_status;
        private static string logs = string.Empty;

        public static void DebugInit(GameManager game) {
            game_status = game;
        }

        /// <summary> Лог в статистику </summary>
        public static void Log(string log) {
            logs += $"Round {game_status.RoundNumber} {log} \n";
        }

        /// <summary> Вывод всех сведений о ходе игры </summary>
        public static string GetLogs() {
            return "Сведения об игре: \n" + logs;
        }


        /// <summary> Считывание сведений для сохранения при сериализации </summary>
        public static string SavingLogs() {
            return logs;
        }

        /// <summary> Загрузка сведений после десериализации </summary>
        public static void LoadLogs(string load_logs) {
            logs = load_logs;
        }
    }
}
