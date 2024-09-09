using EFPractica01.DTOs;
using EFPractica01.Models;
using EFPractica01.Services;
using Microsoft.AspNetCore.Mvc;
using MonsterDataSync.DTOs;
using MonsterDataSync.Repository;

namespace EFPractica01.Controllers
{
    [ApiController]
    [Route("api/")]
    public class SkillsController : Controller
    {
        private readonly ICrudBase _skillsRepository;
        private readonly ILogger<SkillsController> _logger;

        public SkillsController(ICrudBase crudBase, ILogger<SkillsController> logger)
        {
            _skillsRepository = crudBase;
            _logger = logger;
        }

        // Obtener toda la tabla
        [HttpGet("All")]
        public async Task<ActionResult<List<ViewSkillsDtos>>> GetAll()
        {
            try
            {
                var SkillDTOs = await _skillsRepository.GetAll();
                if (SkillDTOs == null || !SkillDTOs.Any())
                {
                    _logger.LogInformation("No se encontraron habilidades.");
                    return NotFound("No se encontraron habilidades.");
                }
                _logger.LogInformation("Se obtuvieron {Count} habilidades.", SkillDTOs.Count);
                return Ok(SkillDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las habilidades.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Obtener en base al Id
        [HttpGet("skills/{Id}")]
        public async Task<ActionResult<ViewSkillsDtos>> GetSkillDTOs(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("ID de habilidad inválido: {Id}", Id);
                return BadRequest("ID inválido.");
            }

            try
            {
                var SkillDTOs = await _skillsRepository.GetSkillDTOs(Id);
                if (SkillDTOs == null)
                {
                    _logger.LogInformation("No se encontró la habilidad con ID: {Id}", Id);
                    return NotFound($"No se encontró la habilidad con ID {Id}.");
                }
                _logger.LogInformation("Se obtuvo la habilidad con ID: {Id}", Id);
                return Ok(SkillDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la habilidad con ID: {Id}", Id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("skills/ranks/{Id}")]
        public async Task<ActionResult<RanksDTOs>> GetRanksDTOs(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("ID de rango inválido: {Id}", Id);
                return BadRequest("ID inválido.");
            }

            try
            {
                var RanksDTOs = await _skillsRepository.GetRanksDTOs(Id);
                if (RanksDTOs == null)
                {
                    _logger.LogInformation("No se encontró el rango con ID: {Id}", Id);
                    return NotFound($"No se encontró el rango con ID {Id}.");
                }
                _logger.LogInformation("Se obtuvo el rango con ID: {Id}", Id);
                return Ok(RanksDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rango con ID: {Id}", Id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("skills/{Id}")]
        public async Task<IActionResult> PutSkillDTOs(int Id, [FromBody] SkillsDTOs skillsdtos)
        {
            if (Id <= 0 || skillsdtos == null)
            {
                _logger.LogWarning("Datos inválidos para actualización: ID {Id}, DTO {@SkillsDTOs}", Id, skillsdtos);
                return BadRequest("Datos inválidos o ID incorrecto.");
            }

            try
            {
                var updateResult = await _skillsRepository.PutSkillDTOs(Id, skillsdtos);
                if (!updateResult)
                {
                    _logger.LogInformation("No se encontró la entidad Skills con el ID: {Id}", Id);
                    return NotFound($"No se encontró la entidad Skills con el ID {Id}.");
                }
                _logger.LogInformation("La entidad Skills con ID {Id} ha sido actualizada exitosamente.", Id);
                return Ok("La entidad Skills ha sido actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la entidad Skills con ID: {Id}", Id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("skills/ranks/{Id}")]
        public async Task<IActionResult> PutRanks(int Id, [FromBody] RanksUPDTOs ranksDTOs)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("ID de rango inválido para actualización: {Id}", Id);
                return BadRequest("El ID debe ser mayor que 0.");
            }

            try
            {
                var VerfAction = await _skillsRepository.PutRanks(Id, ranksDTOs);
                if (!VerfAction)
                {
                    _logger.LogInformation("No se encontró el rango con ID: {Id}", Id);
                    return NotFound($"El rango con ID {Id} no fue encontrado.");
                }
                _logger.LogInformation("El rango con ID {Id} fue actualizado correctamente.", Id);
                return Ok("El rango fue actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rango con ID: {Id}", Id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Borrar en base al Id
        [HttpDelete("skills/{Id}")]
        public async Task<IActionResult> DeleteSKillDTOs(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("ID de habilidad inválido para eliminación: {Id}", Id);
                return BadRequest("ID inválido.");
            }

            try
            {
                var VerfAction = await _skillsRepository.DeleteSkillDTOs(Id);
                if (!VerfAction)
                {
                    _logger.LogInformation("No se encontró la habilidad con ID: {Id} para eliminar.", Id);
                    return NotFound($"No se encontró la habilidad con ID {Id} para eliminar.");
                }
                _logger.LogInformation("La habilidad con ID {Id} ha sido eliminada exitosamente.", Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la habilidad con ID: {Id}", Id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("skills/ranks/{Id}")]
        public async Task<IActionResult> DeleteRanks(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("ID de rango inválido para eliminación: {Id}", Id);
                return BadRequest("ID inválido.");
            }

            try
            {
                var VerfAction = await _skillsRepository.DeleteRanks(Id);
                if (!VerfAction)
                {
                    _logger.LogInformation("No se encontró el rango con ID: {Id} para eliminar.", Id);
                    return NotFound($"No se encontró el rango con ID {Id} para eliminar.");
                }
                _logger.LogInformation("El rango con ID {Id} ha sido eliminado exitosamente.", Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rango con ID: {Id}", Id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }

}