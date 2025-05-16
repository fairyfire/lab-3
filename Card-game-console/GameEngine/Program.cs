using System;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace ForJohnny
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller game = new Controller();

            Console.WriteLine("Нажмите 'N' - Начать новую игру");
            Console.WriteLine("Нажмите 'L' - Загрузить незавершенную игру");
            Console.WriteLine("Нажмите 'R' - Показать правила игры");

            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.R)
            {
                Console.WriteLine("\nВы встречаетесь на поле боя с противником. У вас обоих есть здоровье,\r\nу кого оно опустится до нуля - тот проиграет, и очки маны, за которые Вы\r\nи враг разыгрываете существ и заклинания. Вначале раунда у вас одинаковое\r\nколичество очков маны, мана восстанавливается в начале нового раунда и\r\nсчетчик увеличивается на 1 до 10. Также вы вытягиваете из колоды по\r\nодной карте.\r\n\r\nИгровое поле состоит из 5 линий, где сражаются по одному существу в\r\nкаждой линии, если в момент удара на противоположной стороне у\r\nсущества не будет противника, то это существо ударяет врага/Вас.\r\nКогда номер раунда четный Вы защищаетесь, когда раунд нечетный \r\n- атакуете.\r\n\r\nРаунд состоит из 3-х этапов:\r\n1. Выставление существ\r\n2. Срабатывание эффектов, если таковые имеются, также можно применять заклинания\r\n3. Совершаются удары\r\nВраг может выставлять существ и играть заклинания и на 2 этапе.\r\n\r\nВиды карт:\r\n[Существо] - единственный тип карт, который можно разместить на\r\nбоевом поле, обладает очками атаки и здоровья. А также может иметь \r\nодин из эффектов.\r\n[Заклинание лечения] - имеет характеристику количество здоровья,\r\nлечит свое существо на выбранной линии, не может дать здоровья\r\nбольше, чем максимальное.\r\n[Заклинание урона] - имеет характеристику атаки, наносит урон врагу\r\nна выбранной линии.\r\n[Заклинание улучшения] - имеет характеристики атаки, здоровья,\r\nэффекта, если он имеется. Увеличивает характеристики своего существа\r\nна выбранной линии и дает ему соответствующий эффект, если у \r\nсущества уже имеется эффект, то он заменяется на эффект заклинания.");
                Console.WriteLine("\nНажмите 'N' - Начать новую игру");
                Console.WriteLine("Нажмите 'L' - Загрузить незавершенную игру");
                key = Console.ReadKey().Key;
            }

            switch (key)
            {
                case ConsoleKey.N: game = new Controller(); break;
                case ConsoleKey.L:

                    if (File.Exists("GameSave.json"))
                    {

                        JsonSerializerSettings settings = new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        };
                        DataContractSerializer jsonF = new DataContractSerializer(typeof(Controller));
                        using (FileStream fileStream = new FileStream("GameSave.json", FileMode.Open))
                            game = (Controller)jsonF.ReadObject(fileStream);

                        game.Load();
                    }
                    else
                    {
                        Console.WriteLine("Нет сохраненной игры");
                        Console.WriteLine("Будет запущена новая игра. Нажмите любую клавишу для продолжения или клавину ESC для выхода");
                        if (Console.ReadKey().Key == ConsoleKey.Escape)
                            Environment.Exit(0);
                    }
                    break;

                default: break;
            }


            for (; ; )
            {
                Console.Clear();
                Console.WriteLine(game.GameOutPut());

                Console.WriteLine("1 - Сыграть карту из руки");
                Console.WriteLine("2 - Пропуск");
                Console.WriteLine("I - Вывод сведений");
                Console.WriteLine("ESC - Прервать игру и выйти");


                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1: game.PlayCard(); break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        game.Skip();
                        break;
                    case ConsoleKey.I: Console.Clear(); Console.WriteLine(Debug.GetLogs()); Console.ReadKey(); break;
                    case ConsoleKey.Escape:
                        DataContractSerializer jsonF = new DataContractSerializer(typeof(Controller));

                        game.Save();
                        using (FileStream fileStream = new FileStream("GameSave.json", FileMode.Create))
                            jsonF.WriteObject(fileStream, game);

                        Environment.Exit(0);
                        break;

                    default: break;
                }
            }
        }

    }
}
