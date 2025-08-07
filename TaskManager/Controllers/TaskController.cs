using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Entities;

namespace TaskManager.Controller
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

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync(); //ensure the changes are saved to the database before returning a response

            return Ok("Task created successfully!");

        }


        [HttpPost("assign_tag")]
        public async Task<IActionResult> AssignTag([FromBody] AssignTagDTO dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Tags) 
                .FirstOrDefaultAsync(t => t.taskId == dto.taskId);

            if(task == null)
            {
                return NotFound("Task not found.");
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.tagName == dto.tagName);//looking through tags 

            if(tag == null)
            {
                //yeni tag oluşturabiliriz
                tag = new Tag
                {
                    tagName = dto.tagName
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            //tag zaten var mı?
            if(task.Tags.Any(t => t.tagId == tag.tagId))//task'in taglerini dolaşıp arıyor
            {
                return Ok("Tag already assigned to this task.");
            }

            task.Tags.Add(tag); //burada EF core tasktag tablosunu güncelliyor
            await _context.SaveChangesAsync();

            return Ok("Tag assigned to task successfully!");
        }
    }
}


//[HttpGet("db-check")]
//public async Task<IActionResult> CheckDatabase()
//{
//    try
//    {
//        // Try a simple query to see if the DB is connected
//        int userCount = await _context.Users.CountAsync();
//        return Ok($"✅ Connected to Oracle! User count: {userCount}");
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, $"❌ Failed to connect: {ex.Message}");
//    }
//}
