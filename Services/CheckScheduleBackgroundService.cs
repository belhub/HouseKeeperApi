using HouseKeeperApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class CheckScheduleBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckScheduleBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HouseKeeperDbContext>();
                    var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

                    var schedule = await dbContext.Schedules
                        .Where(s => s.WeekEndDate < currentDate)
                        .ToListAsync() ?? throw new Exception("nie mozna pobrac listy grafikow"); ;

                    if (schedule != null)
                    {
                        var groupedSchedules = schedule.GroupBy(s => s.HouseId);

                        foreach (var group in groupedSchedules)
                        {
                            var tasks = group.ToList();

                            var rotatedTasks = tasks.Select((task, index) => new
                            {
                                Schedule = task,
                                NewTaskName = tasks[(index - 1 + tasks.Count) % tasks.Count].TaskName
                            })
                            .ToList();

                            // Zaktualizowanie TaskName w oryginalnych obiektach
                            foreach (var rotatedTask in rotatedTasks)
                            {
                                rotatedTask.Schedule.TaskName = rotatedTask.NewTaskName;
                            }
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }

                // Ustawienie czasu wykonywania na 3:00
                var now = DateTime.Now;
                var targetTime = now.Date.AddHours(3);
                if (now > targetTime)
                {
                    targetTime = targetTime.AddDays(1);
                }
                var delay = targetTime - now; // Obliczenie TimeSpan

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
