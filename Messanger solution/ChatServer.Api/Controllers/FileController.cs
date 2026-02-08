using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("upload")]
public class FileController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл пуст");

        var uploads = Path.Combine("wwwroot", "uploads");
        Directory.CreateDirectory(uploads);

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string path = Path.Combine(uploads, fileName);

        using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);

        string url = "/uploads/" + fileName;
        return Ok(url);
    }
}
