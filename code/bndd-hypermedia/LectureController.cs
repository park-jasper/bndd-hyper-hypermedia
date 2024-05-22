using Microsoft.AspNetCore.Mvc;
using RESTyard.AspNetCore.WebApi.AttributedRoutes;
using bndd_hypermedia_server;

namespace bndd_hypermedia;

[Route("lectures")]
public class LectureController : ControllerBase
{
    [HttpGetHypermediaObject("{id}", typeof(LectureHto))]
    public IActionResult GetLectureHypermedia(string id)
    {
        return this.Ok(new LectureHto(
            id,
            "Defense against the dark Arts",
            2024,
            "Prof. Dumbledore",
            new LectureHto.CreateExamOp(() => true),
            false,
            null));
    }

    [HttpGet("rest/{id}")]
    public IActionResult GetLectureRest(string id)
    {
        return this.Ok(new {
            name = "Defense against the dark Arts",
            year = 2024,
            lecturer = "Prof. Dumbledore" 
        });
    }
}
