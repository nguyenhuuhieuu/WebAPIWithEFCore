using Microsoft.AspNetCore.Mvc;
using WebAPIwithEFCore.Models;

namespace WebAPIwithEFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        [HttpGet("getAll")]
        public async Task<List<StudentDTO>> GetAllStudent()
        {
            using (var context = new StudentContext())
            {
                /*List<StudentDTO> StudentDTOs = new List<StudentDTO>();
                foreach (var student in context.Students)
                {
                    var studentDTO = student.ToDTO();
                    StudentDTOs.Add(studentDTO);
                }*/
                var StudentDTOs = await (from s in context.Students
                                         select new StudentDTO { ClassId = s.ClassId, StudentId = s.StudentId, StudentName = s.StudentName, ClassName = s.Class.ClassName})
                                         .ToListAsync();
                return StudentDTOs;
            }
            
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudentWithId(int id)
        {
            using (var context = new StudentContext())
            {
                var result = (from s in context.Students
                              where s.StudentId == id
                              select s).FirstOrDefault();
                //var student = await result.FirstAsync();
                if(result == null)
                    return NotFound("This student id isnt exist.!");
                return Ok(result.ToDTO());
            }
            
        }

        [HttpGet("getStudentLazyLoading")] //lazy loading
        public async Task<ActionResult> GetStudent([FromQuery] int studentId)
        {
            using (var context = new StudentContext())
            {
                //eager loading
                var student = context.Students.Include(s => s.Class).FirstOrDefault(s => s.StudentId == studentId);

                //lazy loading
                //student.Friends = student.Class.Students.Count()-1;

                return Ok(student.StudentName + " has " + (student.Class.Students.Count()-1) + " friend(s).");
            }

        }

        [HttpPost("add")]
        public async Task<StudentDTO> AddStudent([FromQuery] string studentName, [FromQuery] int classId)
        {
            using(var context = new StudentContext())
            {
                var newStudent = new Student()
                {
                    StudentName = studentName,
                    ClassId = classId
                };
                context.Students.Add(newStudent);
                context.SaveChanges();
                return context.Students.OrderBy(s => s.StudentId).Last().ToDTO();
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<StudentDTO>> Update([FromQuery] int id, [FromQuery] string newName)
        {
            using (var context = new StudentContext())
            {
                var student = (from s in context.Students
                              where s.StudentId == id
                              select s).FirstOrDefault();

                if (student == null)
                    return NotFound("This student id isnt exist.!");

                student.StudentName = newName;
                context.SaveChanges();
                return student.ToDTO();
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<StudentDTO>> Delete([FromQuery] int id)
        {
            using (var context = new StudentContext())
            {
                var student = (from s in context.Students
                               where s.StudentId == id
                               select s).FirstOrDefault();

                if (student == null)
                    return NotFound("This student id isnt exist.!");

                context.Students.Remove(student);
                context.SaveChanges();

                return student.ToDTO();
            }
        }

        [HttpGet("getAllClass")]
        public async Task<List<ClassDTO>> GetAllClass()
        {
            using (var context = new StudentContext())
            {
                var ClassDTOs = await (from c in context.Classes
                                       select c.ToDTO()).ToListAsync();
                return ClassDTOs;
            }
        }

        [HttpGet("getClass")]
        public async Task<ActionResult<ClassDTO>> GetClass([FromQuery] int id)
        {
            using (var context = new StudentContext())
            {
                var Class = (from s in context.Classes
                             where s.ClassId == id
                             select s).FirstOrDefault();
                if (Class == null)
                    return NotFound("Not found.");
                return Class.ToDTO();
            }
        }

        [HttpGet("getStudentOfClass")]
        public async Task<ActionResult<List<StudentDTO>>> GetClassStudent([FromQuery] int id)
        {
            using (var context = new StudentContext())
            {
                var Class = (from s in context.Classes
                               where s.ClassId == id
                               select s).FirstOrDefault();
                if (Class == null)
                    return NotFound("Not found.");

                var studentDTOs = new List<StudentDTO>();
                foreach (var student in Class.Students)
                {
                    var studentDTO = student.ToDTO();
                    studentDTOs.Add(studentDTO);
                }
                return studentDTOs;
            }
        }

        [HttpPost("addClass")]
        public async Task<ActionResult<ClassDTO>> AddClass([FromQuery] string className)
        {
            using (var context = new StudentContext())
            {
                var newClass = new Class() { ClassName = className };
                context.Classes.Add(newClass);
                context.SaveChanges();
                return context.Classes.OrderBy(c => c.ClassId).Last().ToDTO();
            }
        }

        [HttpDelete("deleteClass")]
        public async Task<ActionResult<ClassDTO>> DeleteClass([FromQuery] int classId)
        {
            using (var context = new StudentContext())
            {
                var Class = (from s in context.Classes
                             where s.ClassId.Equals(classId)
                             select s).FirstOrDefault();
                if (Class == null) return NotFound("Not found.");
                context.Classes.Remove(Class);
                context.SaveChanges();
                return Class.ToDTO();
            }
        }
    }
}
