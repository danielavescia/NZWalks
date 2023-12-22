using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https://localhost:7297/api/students
    [Route( "api/[controller]" )]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: https://localhost:7297/api/students
        [HttpGet]
            public IActionResult GetAllStudents() {


                String [] students = new String [] { "John", "Maria", "Joana" };

                return Ok( students ); //RETURN RESPONSE 200 AND OBJECT
            }
    }
}
