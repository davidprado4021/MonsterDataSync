using EFPractica01.Models;

namespace MonsterDataSync.DTOs
{
    public class ViewSkillsDtos
    { 
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public ICollection<RanksDTOs> Ranks { get; set; }

    }
}
