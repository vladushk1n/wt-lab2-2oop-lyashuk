using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wt_lab2_2oop.Interfaces;
using wt_lab2_2oop.Models;
using wt_lab2_2oop.Services;

namespace wt_lab2_2oop
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine("  Академия управления при Президенте РБ");
                Console.WriteLine("  Лабораторная работа №2: ООП, LINQ, async/await");
                Console.WriteLine("  Вариант 15: Игровой сервис / Турниры");
                Console.WriteLine("==================================================");
                Console.WriteLine("1.  [Блок 1.1] Базовый объект Player");
                Console.WriteLine("2.  [Блок 1.2] Инкапсуляция и Полиморфизм (List<BaseEntity>)");
                Console.WriteLine("3.  [Блок 1.3] Сервисный слой (IItemService и PlayerService)");
                Console.WriteLine("4.  [Блок 2.1] Перебор List<Player> через foreach");
                Console.WriteLine("5.  [Блок 2.1] Поиск в Dictionary через TryGetValue");
                Console.WriteLine("6.  [Блок 2.3] Простой LINQ (Where, OrderByDescending, Select)");
                Console.WriteLine("7.  [Блок 2.3] Группировка (GroupBy) и Агрегаты (Count, Sum, Avg, Min, Max)");
                Console.WriteLine("8.  [Блок 3.1] Объявление асинхронного метода (async Task)");
                Console.WriteLine("9.  [Блок 3.2] Вызов async-метода через await без блокировки потока");
                Console.WriteLine("10. [Блок 3.3] Получение коллекции из СУБД/API (async Task<List<T>>)");
                Console.WriteLine("11. [Блок 3.4] Обработка исключений в async-методе (try-catch-finally)");
                Console.WriteLine("12. [Блок 3.5] Параллельное выполнение задач через Task.WhenAll()");
                Console.WriteLine("13. [Блок 3.6] Отмена асинхронной операции (CancellationToken)");
                Console.WriteLine("0.  Выход");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DemoBlock1Level1();
                        break;
                    case "2":
                        DemoBlock1Level2();
                        break;
                    case "3":
                        DemoBlock1Level3();
                        break;
                    case "4":
                        DemoBlock2Level1List();
                        break;
                    case "5":
                        DemoBlock2Level1Dictionary();
                        break;
                    case "6":
                        DemoBlock2Level3LinqBasic();
                        break;
                    case "7":
                        DemoBlock2Level3LinqGroupAndAggregate();
                        break;
                    case "8":
                        await DemoBlock3Level1Async();
                        break;
                    case "9":
                        await DemoBlock3Level2AwaitNonBlocking();
                        break;
                    case "10":
                        await DemoBlock3Level3FetchCollectionAsync();
                        break;
                    case "11":
                        await DemoBlock3Level4ExceptionHandlingAsync();
                        break;
                    case "12":
                        await DemoBlock3Level5TaskWhenAllAsync();
                        break;
                    case "13":
                        await DemoBlock3Level6CancellationAsync();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }
            }
        }

        // --- БЛОК 1 ---
        static void DemoBlock1Level1()
        {
            Console.WriteLine("=== БЛОК 1. Уровень 1: Создание объекта ===");
            Player p = new Player(1, "s1mple", "AWPer", 3200);
            p.PrintInfo();
        }

        static void DemoBlock1Level2()
        {
            Console.WriteLine("=== БЛОК 1. Уровень 2: Инкапсуляция и Полиморфизм ===");
            List<BaseEntity> entities = new List<BaseEntity>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Team(10, "Natus Vincere", "NaVi")
            };

            foreach (var entity in entities)
            {
                entity.PrintInfo();
            }
        }

        static void DemoBlock1Level3()
        {
            Console.WriteLine("=== БЛОК 1. Уровень 3: Перегрузка, Protected и IItemService ===");
            IItemService<Player> service = new PlayerService();
            PlayerService concreteService = (PlayerService)service;

            service.Add(new Player(1, "s1mple", "AWPer", 3200));
            concreteService.Add("ZywOo", "Rifler", 3150);

            foreach (var player in service.GetAll())
            {
                player.PrintInfo();
            }
        }

        // --- БЛОК 2 ---
        static void DemoBlock2Level1List()
        {
            Console.WriteLine("=== БЛОК 2. Уровень 1: List<Player> и foreach ===");
            List<Player> playerList = new List<Player>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Player(2, "ZywOo", "AWPer", 3150),
                new Player(3, "b1t", "Rifler", 2900),
                new Player(4, "m0NESY", "AWPer", 3100),
                new Player(5, "electroNic", "Rifler", 2850),
                new Player(6, "Perfecto", "Support", 2700)
            };

            foreach (Player player in playerList)
            {
                player.PrintInfo();
            }
        }

        static void DemoBlock2Level1Dictionary()
        {
            Console.WriteLine("=== БЛОК 2. Уровень 1: Dictionary и TryGetValue ===");
            Dictionary<int, Player> playerDict = new Dictionary<int, Player>
            {
                { 1, new Player(1, "s1mple", "AWPer", 3200) },
                { 2, new Player(2, "ZywOo", "AWPer", 3150) },
                { 3, new Player(3, "b1t", "Rifler", 2900) }
            };

            int searchId = 2;
            if (playerDict.TryGetValue(searchId, out Player foundPlayer))
            {
                Console.Write($"[Найдено по ID={searchId}]: ");
                foundPlayer.PrintInfo();
            }

            int missingId = 99;
            if (!playerDict.TryGetValue(missingId, out _))
            {
                Console.WriteLine($"[Обработка ID={missingId}]: Элемент отсутствует (без ошибок).");
            }
        }

        static void DemoBlock2Level3LinqBasic()
        {
            Console.WriteLine("=== БЛОК 2. Уровень 3: LINQ (Where, OrderByDescending, Select) ===");

            List<Player> players = new List<Player>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Player(2, "ZywOo", "AWPer", 3150),
                new Player(3, "b1t", "Rifler", 2900),
                new Player(4, "m0NESY", "AWPer", 3100),
                new Player(5, "electroNic", "Rifler", 2850),
                new Player(6, "Perfecto", "Support", 2700)
            };

            var result = players
                .Where(p => p.Rating > 3000)
                .OrderByDescending(p => p.Rating)
                .Select(p => new { p.Nickname, p.Role, p.Rating });

            Console.WriteLine("Игроки с рейтингом > 3000 (отсортированы по убыванию):");
            foreach (var item in result)
            {
                Console.WriteLine($"Никнейм: {item.Nickname,-10} | Роль: {item.Role,-8} | MMR: {item.Rating}");
            }
        }

        static void DemoBlock2Level3LinqGroupAndAggregate()
        {
            Console.WriteLine("=== БЛОК 2. Уровень 3: Группировка (GroupBy) и Агрегаты ===");

            List<Player> players = new List<Player>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Player(2, "ZywOo", "AWPer", 3150),
                new Player(3, "b1t", "Rifler", 2900),
                new Player(4, "m0NESY", "AWPer", 3100),
                new Player(5, "electroNic", "Rifler", 2850),
                new Player(6, "Perfecto", "Support", 2700)
            };

            var statsByRole = players
                .GroupBy(p => p.Role)
                .Select(g => new
                {
                    Role = g.Key,
                    Count = g.Count(),
                    TotalRating = g.Sum(p => p.Rating),
                    AverageRating = g.Average(p => p.Rating),
                    MinRating = g.Min(p => p.Rating),
                    MaxRating = g.Max(p => p.Rating)
                });

            Console.WriteLine("Статистика по категориям (ролям игроков):\n");
            foreach (var stat in statsByRole)
            {
                Console.WriteLine($"Категория [Роль]: {stat.Role}");
                Console.WriteLine($"  • Количество (Count):  {stat.Count}");
                Console.WriteLine($"  • Сумма MMR (Sum):     {stat.TotalRating}");
                Console.WriteLine($"  • Средний MMR (Avg):   {stat.AverageRating:F1}");
                Console.WriteLine($"  • Мин. MMR (Min):      {stat.MinRating}");
                Console.WriteLine($"  • Макс. MMR (Max):     {stat.MaxRating}");
                Console.WriteLine(new string('-', 45));
            }
        }

        // --- БЛОК 3 ---
        static async Task DemoBlock3Level1Async()
        {
            Console.WriteLine("=== БЛОК 3. Уровень 1: Асинхронная операция (async Task) ===");

            PlayerService playerService = new PlayerService();
            List<Player> fetchedPlayers = await playerService.FetchDataAsync();

            Console.WriteLine("\nПолученные асинхронно объекты:");
            foreach (var player in fetchedPlayers)
            {
                player.PrintInfo();
            }
        }

        static async Task<int> CalculateTotalRatingAsync(List<Player> players)
        {
            Console.WriteLine("[Async Method] Начало неблокирующего расчета суммарного рейтинга...");
            await Task.Delay(1000);
            int total = players.Sum(p => p.Rating);
            Console.WriteLine("[Async Method] Расчет завершен.");
            return total;
        }

        static async Task DemoBlock3Level2AwaitNonBlocking()
        {
            Console.WriteLine("=== БЛОК 3. Уровень 2: Неблокирующий асинхронный вызов через await ===");

            List<Player> playerList = new List<Player>
            {
                new Player(1, "s1mple", "AWPer", 3200),
                new Player(2, "ZywOo", "AWPer", 3150),
                new Player(3, "m0NESY", "AWPer", 3100)
            };

            int totalRating = await CalculateTotalRatingAsync(playerList);

            Console.WriteLine($"\n[Результат]: Суммарный рейтинг игроков = {totalRating} MMR");
            Console.WriteLine("Вызов выполнен асинхронно через 'await', избегая применения .Result / .Wait().");
        }

        static async Task DemoBlock3Level3FetchCollectionAsync()
        {
            Console.WriteLine("=== БЛОК 3. Уровень 3: Асинхронное получение коллекции Task<List<T>> ===");

            IItemService<Player> service = new PlayerService();
            List<Player> dbPlayers = await service.GetCollectionFromDbAsync();

            Console.WriteLine($"\nУспешно загружено из СУБД/API {dbPlayers.Count} элементов:");
            foreach (var player in dbPlayers)
            {
                player.PrintInfo();
            }
        }

        static async Task DemoBlock3Level4ExceptionHandlingAsync()
        {
            Console.WriteLine("=== БЛОК 3: Обработка исключений (try-catch-finally) в async-методе ===");

            PlayerService playerService = new PlayerService();

            Console.WriteLine("\n--- Тест 1: Успешный вызов ---");
            List<Player> successList = await playerService.FetchDataWithExceptionHandlingAsync(shouldFail: false);
            Console.WriteLine($"Получено элементов: {successList.Count}");

            Console.WriteLine("\n--- Тест 2: Имитация исключения (shouldFail = true) ---");
            List<Player> failedList = await playerService.FetchDataWithExceptionHandlingAsync(shouldFail: true);
            Console.WriteLine($"Получено элементов (после обработки ошибки): {failedList.Count}");
        }

        static async Task DemoBlock3Level5TaskWhenAllAsync()
        {
            Console.WriteLine("=== БЛОК 3: Параллельное выполнение независимых задач (Task.WhenAll) ===");

            PlayerService playerService = new PlayerService();
            await playerService.FetchAllDataParallelAsync();
        }

        // Демонстрация отмены асинхронной операции через CancellationTokenSource
        static async Task DemoBlock3Level6CancellationAsync()
        {
            Console.WriteLine("=== БЛОК 3: Отмена асинхронной операции (CancellationToken) ===");

            PlayerService playerService = new PlayerService();
            using CancellationTokenSource cts = new CancellationTokenSource();

            // Задаём таймаут отмены через 2.5 секунды (операция должна прерваться на 3-м шаге)
            cts.CancelAfter(2500);

            try
            {
                await playerService.LongRunningOperationWithCancellationAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n[Уведомление]: Асинхронная операция была успешно отменена пользователем / по таймауту!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ошибка]: {ex.Message}");
            }
        }
    }
}