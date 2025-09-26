// WebApi/Controllers/TransactionImportController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolRowingApp.Application.Services;
using SchoolRowingApp.Domain.Banking;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.Examples;

namespace SchoolRowingApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionImportController : ControllerBase
{
    private readonly ITransactionImportService _importService;
    private readonly ILogger<TransactionImportController> _logger;

    public TransactionImportController(
        ITransactionImportService importService,
        ILogger<TransactionImportController> logger)
    {
        _importService = importService;
        _logger = logger;
    }
    
    /// <summary>
    /// Импортирует операции из CSV-файла
    /// </summary>
    /// <param name="file">CSV-файл с операциями</param>
    /// <returns>Результат импорта</returns>
    [HttpPost("import")]
    //[ProducesResponseType(typeof(ImportResultDto), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Файл не предоставлен");
        }

        if (!file.ContentType.Contains("csv") && !file.FileName.EndsWith(".csv"))
        {
            return BadRequest("Поддерживаются только CSV-файлы");
        }

        var tempPath = Path.GetTempFileName();

        try
        {
            // Сохраняем временный файл
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Выполняем импорт
            var result = await _importService.ImportFromCsvAsync(
                tempPath,
                file.FileName,
                HttpContext.RequestAborted);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при импорте файла {FileName}", file.FileName);
            return StatusCode(500, new ImportResultDto
            {
                Success = false,
                Message = $"Внутренняя ошибка сервера: {ex.Message}"
            });
        }
        finally
        {
            // Удаляем временный файл
            if (System.IO.File.Exists(tempPath))
            {
                try
                {
                    System.IO.File.Delete(tempPath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Не удалось удалить временный файл {Path}", tempPath);
                }
            }
        }
    }
    /// <summary>
    /// Получает историю импортов
    /// </summary>
    /// <returns>Список импортов</returns>
    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<TransactionImport>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetImportHistory()
    {
        // Здесь нужно добавить реализацию получения истории импортов
        // Для примера возвращаем пустой список
        return Ok(new List<object>());
    }
}