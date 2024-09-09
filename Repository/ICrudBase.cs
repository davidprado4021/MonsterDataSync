using EFPractica01.DTOs;
using MonsterDataSync.DTOs;

namespace MonsterDataSync.Repository
{
    public interface ICrudBase
    {
        Task<List<ViewSkillsDtos>> GetAll();
        Task<ViewSkillsDtos> GetSkillDTOs(int Id);
        Task<RanksDTOs> GetRanksDTOs(int Id);

        Task<bool> PutSkillDTOs(int Id, SkillsDTOs skillsdtos);
        Task<bool> PutRanks(int Id, RanksUPDTOs ranksDTOs);

        Task<bool> DeleteSkillDTOs(int Id);
        Task<bool> DeleteRanks(int Id);
    }
}
