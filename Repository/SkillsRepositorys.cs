using AutoMapper;
using EFPractica01.DTOs;
using EFPractica01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonsterDataSync.DTOs;

namespace MonsterDataSync.Repository
{
    public class SkillsRepositorys : ICrudBase
    {
        private readonly SkillsContext _skillsContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SkillsRepositorys> _logger;

        public SkillsRepositorys(SkillsContext context, IMapper mapper, ILogger<SkillsRepositorys> logger)
        {
            _skillsContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        // Obtener toda la información de la BD
        public async Task<List<ViewSkillsDtos>> GetAll()
        {
            try
            {
                var skills = await _skillsContext.Skills
                                  .Include(s => s.Ranks)
                                  .ToListAsync();

                if (skills == null || !skills.Any())
                {
                    _logger.LogInformation("No se encontraron habilidades en la base de datos.");
                    return null;
                }

                _logger.LogInformation("Se obtuvieron {Count} habilidades de la base de datos.", skills.Count);
                return _mapper.Map<List<ViewSkillsDtos>>(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las habilidades.");
                throw;
            }
        }

        // Obtener Ranks por Id
        public async Task<RanksDTOs> GetRanksDTOs(int Id)
        {
            try
            {
                var ranks = await _skillsContext.Ranks.FirstOrDefaultAsync(x => x.Id == Id);

                if (ranks == null)
                {
                    _logger.LogInformation("No se encontró el rango con ID {Id}.", Id);
                    return null;
                }

                _logger.LogInformation("Se obtuvo el rango con ID {Id}.", Id);
                return _mapper.Map<RanksDTOs>(ranks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rango con ID {Id}.", Id);
                throw;
            }
        }

        // Obtener Skills por Id
        public async Task<ViewSkillsDtos> GetSkillDTOs(int Id)
        {
            try
            {
                var skills = await _skillsContext.Skills.FirstOrDefaultAsync(x => x.Id == Id);

                if (skills == null)
                {
                    _logger.LogInformation("No se encontró la habilidad con ID {Id}.", Id);
                    return null;
                }

                _logger.LogInformation("Se obtuvo la habilidad con ID {Id}.", Id);
                return _mapper.Map<ViewSkillsDtos>(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la habilidad con ID {Id}.", Id);
                throw;
            }
        }

        // Actualizar Ranks
        public async Task<bool> PutRanks(int Id, RanksUPDTOs ranksDTOs)
        {
            try
            {
                var rank = await _skillsContext.Ranks.FindAsync(Id);

                if (rank == null)
                {
                    _logger.LogInformation("No se encontró el rango con ID {Id} para actualizar.", Id);
                    return false;
                }

                _mapper.Map(ranksDTOs, rank);
                await _skillsContext.SaveChangesAsync();

                _logger.LogInformation("El rango con ID {Id} fue actualizado correctamente.", Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rango con ID {Id}.", Id);
                throw;
            }
        }

        // Actualizar Skills
        public async Task<bool> PutSkillDTOs(int Id, SkillsDTOs skillsdtos)
        {
            try
            {
                var skills = await _skillsContext.Skills
                                .Include(s => s.Ranks)
                                .FirstOrDefaultAsync(s => s.Id == Id);

                if (skills == null)
                {
                    _logger.LogInformation("No se encontró la habilidad con ID {Id} para actualizar.", Id);
                    return false;
                }

                _mapper.Map(skillsdtos, skills);

                foreach (var rankDto in skillsdtos.Ranks)
                {
                    var existingRank = skills.Ranks.FirstOrDefault(r => r.Id == rankDto.Id);
                    if (existingRank != null)
                    {
                        _mapper.Map(rankDto, existingRank);
                    }
                    else
                    {
                        var newRank = _mapper.Map<Ranks>(rankDto);
                        skills.Ranks.Add(newRank);
                    }
                }

                var ranksToRemove = skills.Ranks
                                    .Where(r => !skillsdtos.Ranks.Any(dto => dto.Id == r.Id))
                                    .ToList();

                foreach (var rankToRemove in ranksToRemove)
                {
                    skills.Ranks.Remove(rankToRemove);
                }

                await _skillsContext.SaveChangesAsync();

                _logger.LogInformation("La habilidad con ID {Id} fue actualizada correctamente.", Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la habilidad con ID {Id}.", Id);
                throw;
            }
        }

        // Eliminar Skills
        public async Task<bool> DeleteSkillDTOs(int Id)
        {
            try
            {
                var skill = await _skillsContext.Skills
                                   .Include(s => s.Ranks)
                                   .FirstOrDefaultAsync(s => s.Id == Id);

                if (skill == null)
                {
                    _logger.LogInformation("No se encontró la habilidad con ID {Id} para eliminar.", Id);
                    return false;
                }

                _skillsContext.Skills.Remove(skill);
                await _skillsContext.SaveChangesAsync();

                _logger.LogInformation("La habilidad con ID {Id} fue eliminada correctamente.", Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la habilidad con ID {Id}.", Id);
                throw;
            }
        }

        // Eliminar Ranks
        public async Task<bool> DeleteRanks(int Id)
        {
            try
            {
                var rank = await _skillsContext.Ranks.FindAsync(Id);

                if (rank == null)
                {
                    _logger.LogInformation("No se encontró el rango con ID {Id} para eliminar.", Id);
                    return false;
                }

                _skillsContext.Ranks.Remove(rank);
                await _skillsContext.SaveChangesAsync();

                _logger.LogInformation("El rango con ID {Id} fue eliminado correctamente.", Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rango con ID {Id}.", Id);
                throw;
            }
        }
    }
}
