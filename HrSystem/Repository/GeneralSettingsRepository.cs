using HrSystem.DTO;
using HrSystem.Models;

namespace HrSystem.Repository
{
    public class GeneralSettingsRepository:IGeneralSettingsRepository
    {
        ITIContext db;

        public GeneralSettingsRepository(ITIContext db)
        {
            this.db = db;
        }



        public List<General_Settings> GetAll()
        {
           return db.Performances.ToList();
        }
     

        public General_Settings GetbyId()
        {
            return db.Performances.FirstOrDefault();
        }

        public General_Settings Add(GeneralSettingsDTO settingsDTO)
        {
            General_Settings settings = new General_Settings()
            {
                Id = settingsDTO.Id,
                Discound = settingsDTO.Discound,
                OffDay1 = settingsDTO.OffDay1,
                OffDay2 = settingsDTO.OffDay2,
                Bonus = settingsDTO.Bonus,
                UnitUsed = settingsDTO.UnitUsed,
            };
            db.Performances.Add(settings);
            return settings;
         }

        public void Update(GeneralSettingsDTO settingsDTO, int id)
        {

            General_Settings settings = GetbyId();
           settings.Id = id;
            settings.Discound = settingsDTO.Discound;
            settings.OffDay1 = settingsDTO.OffDay1;
            settings.OffDay2 = settingsDTO.OffDay2;
            settings.Bonus = settingsDTO.Bonus;
            settings.UnitUsed = settingsDTO.UnitUsed;

        }
        public void Delete(int id)
        {
            General_Settings settings = GetbyId();    
            db.Performances.Remove(settings);

        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
