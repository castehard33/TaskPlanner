using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TaskPlannerApp.Models
{
    // Klasa reprezentująca zadanie
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Status { get; set; }
    }

    // Klasa do zapisywania i wczytywania zadań w pliku JSON
    public class TaskService
    {
        private const string FileName = "tasks.json";

        // Zapisz listę zadań do pliku JSON
        public void SaveTasks(List<TaskItem> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }

        // Wczytaj listę zadań z pliku JSON
        public List<TaskItem> LoadTasks()
        {
            if (!File.Exists(FileName))
                return new List<TaskItem>();

            string json = File.ReadAllText(FileName);
            return JsonConvert.DeserializeObject<List<TaskItem>>(json);
        }
    }
}
