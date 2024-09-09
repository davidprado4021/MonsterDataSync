using EFPractica01.Models;
using MonsterDataSync.DTOs;

namespace EFPractica01.DTOs
{
    public class SkillsDTOs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RanksDTOs> Ranks { get; set; }
    }
}
