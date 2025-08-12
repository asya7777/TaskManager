using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Entities;

namespace TaskManager.Controllers
{
    [ApiController]//tells ASP.NET Core that this class is an API controller(receives http requests and sends http responses)

    [Route("api/[controller]")]//controls the url path to access this controller


    public class TaskController: ControllerBase //inherits from ControllerBase provided by ASP.NET Core
    {
        private readonly AppDbContext _context;//to read from my database
        public TaskController(AppDbContext context)
        {
            _context = context;//receives my database connection and stores it in the private variable
        }


        [HttpGet("get_tasks/{userId}")]
        public async Task<IActionResult> GetTasks(int userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.usrId == userId)//filtering tasks by userId
                .ToListAsync();//runs the query and returns a list of tasks

            return Ok(tasks);
        }

        //now we define the actions that this controller will handle
        [HttpPost]//will handle POST requests
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskDTO dto)
        {
            var newTask = new Entities.Task
            {
                taskName = dto.taskName,
                taskDescription = dto.taskDescription,
                taskDeadline = dto.taskDeadline,
                usrId = dto.usrId
            };

            //now we assign tags
            if(dto.Tags.Count > 0)//checking if tags are requested
            {
                var normalized = dto.Tags
                    .Select(t => t.Trim())//removing spaces
                    .Where(t=> !string.IsNullOrWhiteSpace(t))//boşluk olanları tamamen siler
                    .Distinct(StringComparer.OrdinalIgnoreCase)//case insensitive bir şekilde duplicateları çıkarır
                    .ToList();//async değil çünkü databasele çalışmıyoruz
                //yollanan tag isimlerini temizledik

                var existingTags = await _context.Tags
                    .Where(t => normalized.Contains(t.tagName))//veritabanında var olan tagleri buluyoruz
                    .ToDictionaryAsync(t=>t.tagName, StringComparer.OrdinalIgnoreCase);

                foreach(var tagName in normalized)
                {
                    if(existingTags.TryGetValue(tagName, out var existingTag))
                    {
                        newTask.Tags.Add(existingTag); //tag zaten varsa, task'a ekliyoruz
                    }
                    else
                    {
                        newTask.Tags.Add(new Tag
                        {
                            tagName = tagName //yeni tag oluşturuyoruz
                        });
                    }
                }
            }

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync(); //ensure the changes are saved to the database before returning a response

            return Ok("Task created successfully!");

        }

        //[HttpPost("add_tags/{taskId}")]
        //public async Task<IActionResult> AddTags(int taskId, [FromBody] List<string> tags)
        //{
        //    var normalized = tags
        //            .Select(t => t.Trim())
        //            .Where(t => !string.IsNullOrWhiteSpace(t))
        //            .Distinct(StringComparer.OrdinalIgnoreCase)
        //            .ToList();

        //    var task = await _context.Tasks
        //        .Include(t => t.Tags)
        //        .FirstOrDefaultAsync(t => t.taskId == taskId);
        //    if (task == null)
        //    {
        //        return NotFound("Task not found");
        //    }

        //    var existingTags = await _context.Tags
        //        .ToDictionaryAsync(t => t.tagName, StringComparer.OrdinalIgnoreCase);

        //    foreach (var tagName in normalized)
        //    {
        //        if(!existingTags.TryGetValue(tagName, out var existingTag)){
                    
        //        }
        //    }


        //}
    }
}