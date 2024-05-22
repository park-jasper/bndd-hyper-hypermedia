using Microsoft.AspNetCore.Mvc;
using bndd_hypermedia_server;

namespace bndd_hypermedia;

[Route("exams")]
public class ExamController : ControllerBase
{
    [HttpGet("{id}/{year:int}")]
    public IActionResult GetExam(string id, int year)
    {
        return this.NotFound();
    }
}
