using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wt_lab2_2oop.Interfaces;
using wt_lab2_2oop.Models;

namespace wt_lab2_2oop.Services
{
    public class PlayerService : IItemService<Player>
    {
        private readonly List<Player> _players = new List<Player>();

        public void Add(Player item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _players.Add(item);
            Console.WriteLine($"[PlayerService] Добавлен игрок #{item.Id}: '{item.Nickname}'");
        }

        public void Add(string nickname, string role, int rating)
        {
            int newId = _players.Count > 0 ? _players.Max(p => p.Id) + 1 : 1;
            Player newPlayer = new Player(newId, nickname, role, rating);
            Add(newPlayer);
        }

        public List<Player> GetAll()
        {
            return new List<Player>(_players);
        }

        public Player GetById(int id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public bool Delete(int id)
        {
            var player = GetById(id);
            if (player != null)
            {
                _players.Remove(player);
                Console.WriteLine($"[PlayerService] Игрок с ID {id} успешно удален.");
                return true;
            }
            return false;
        }

        public async Task<List<Player>> FetchDataAsync()
        {
            Console.WriteLine("[Async Processing] Начало загрузки данных игроков из внешнего API...");
            await Task.Delay(1000);
            Console.WriteLine("[Async Processing] Данные успешно получены после задержки в 1000 мс.");

            return new List<Player>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Player(2, "ZywOo", "AWPer", 3150),
                new Player(3, "b1t", "Rifler", 2900),
                new Player(4, "m0NESY", "AWPer", 3100)
            };
        }

        public async Task<List<Player>> GetCollectionFromDbAsync()
        {
            Console.WriteLine("[Database Service] Инициализация подключения к СУБД PostgreSQL / API...");
            await Task.Delay(1200);
            Console.WriteLine("[Database Service] Запрос успешно выполнен. Формирование коллекции List<Player>...");

            return new List<Player>
            {
                new Player(101, "donk", "Rifler", 3350),
                new Player(102, "sh1ro", "AWPer", 3180),
                new Player(103, "chopper", "IGL", 2750),
                new Player(104, "magixx", "Support", 2800),
                new Player(105, "zok3r", "Rifler", 2950)
            };
        }

        public async Task<List<Player>> FetchDataWithExceptionHandlingAsync(bool shouldFail)
        {
            Console.WriteLine("[Async Try-Catch] Подключение к удаленному серверу...");

            try
            {
                await Task.Delay(1000);

                if (shouldFail)
                {
                    throw new InvalidOperationException("Ошибка подключения: Внешний API недоступен или превышен таймаут.");
                }

                Console.WriteLine("[Async Try-Catch] Данные успешно получены.");
                return new List<Player>
                {
                    new Player(1, "s1mple", "AWPer", 3200),
                    new Player(2, "ZywOo", "AWPer", 3150)
                };
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[CATCH] Перехвачено исключение: {ex.Message}");
                return new List<Player>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CATCH] Непредвиденная ошибка: {ex.Message}");
                return new List<Player>();
            }
            finally
            {
                Console.WriteLine("[FINALLY] Освобождение сетевых ресурсов и закрытие соединения.");
            }
        }

        public async Task<List<Player>> FetchPlayersAsync()
        {
            Console.WriteLine("[Task 1] Старт загрузки игроков (задержка 1500 мс)...");
            await Task.Delay(1500);
            Console.WriteLine("[Task 1] Игроки загружены.");
            return new List<Player>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Player(2, "ZywOo", "AWPer", 3150)
            };
        }

        public async Task<List<Team>> FetchTeamsAsync()
        {
            Console.WriteLine("[Task 2] Старт загрузки команд (задержка 1000 мс)...");
            await Task.Delay(1000);
            Console.WriteLine("[Task 2] Команды загружены.");
            return new List<Team>
            {
                new Team(10, "Natus Vincere", "NaVi"),
                new Team(11, "Team Vitality", "VIT")
            };
        }

        public async Task<List<string>> FetchTournamentsAsync()
        {
            Console.WriteLine("[Task 3] Старт загрузки турниров (задержка 800 мс)...");
            await Task.Delay(800);
            Console.WriteLine("[Task 3] Турниры загружены.");
            return new List<string> { "IEM Katowice 2026", "PGL Major 2026" };
        }

        public async Task FetchAllDataParallelAsync()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("[Task.WhenAll] Запуск 3 независимых асинхронных задач параллельно...\n");

            Task<List<Player>> task1 = FetchPlayersAsync();
            Task<List<Team>> task2 = FetchTeamsAsync();
            Task<List<string>> task3 = FetchTournamentsAsync();

            await Task.WhenAll(task1, task2, task3);

            stopwatch.Stop();

            List<Player> players = await task1;
            List<Team> teams = await task2;
            List<string> tournaments = await task3;

            Console.WriteLine($"\nВсе задачи выполнены за {stopwatch.ElapsedMilliseconds} мс (вместо ~3300 мс при последовательном вызове)!");
            Console.WriteLine($"• Игроков получено: {players.Count}");
            Console.WriteLine($"• Команд получено: {teams.Count}");
            Console.WriteLine($"• Турниров получено: {tournaments.Count}");
        }

        // --- БЛОК 3: Поддержка отмены операций через CancellationToken ---
        public async Task LongRunningOperationWithCancellationAsync(CancellationToken token)
        {
            Console.WriteLine("[Async Cancellation] Начало длительной пакетной обработки данных (5 шагов по 1000 мс)...");

            for (int step = 1; step <= 5; step++)
            {
                // Проверяем запрос отмены перед каждым шагом
                token.ThrowIfCancellationRequested();

                Console.WriteLine($"  -> [Шаг {step}/5] Обработка данных...");
                
                // Передаём токен отмены в задержку
                await Task.Delay(1000, token);
            }

            Console.WriteLine("[Async Cancellation] Длительная операция успешно завершена.");
        }
    }
}