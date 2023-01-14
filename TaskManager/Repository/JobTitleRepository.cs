using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class JobTitleRepository
    {
        private ApplicationDbContext dbContext;

        public JobTitleRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public JobTitleRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<JobTitlesModel> GetAllJobTitles()
        {
            List<JobTitlesModel> jobTitlesModel = new List<JobTitlesModel>();

            foreach (var jobTitle in dbContext.JobTitles.OrderBy(x=>x.JobTitle1))
            {
                jobTitlesModel.Add(MapDbObjectToModel(jobTitle));
            }
            return jobTitlesModel;
        }

        public JobTitlesModel GetJobTitleById(Guid id)
        {
            JobTitlesModel jobTitleModel = new JobTitlesModel();

            jobTitleModel = MapDbObjectToModel(dbContext.JobTitles.FirstOrDefault(x => x.IdJobTitle == id));

            return jobTitleModel;
        }

        private JobTitlesModel MapDbObjectToModel(JobTitle jobTitle)
        {
            JobTitlesModel model = new JobTitlesModel();
            if (jobTitle != null)
            {
                model.IdJobTitle = jobTitle.IdJobTitle;
                model.JobTitle1 = jobTitle.JobTitle1;
            }

            return model;
        }

        public void InsertJobTitle(JobTitlesModel jobTitleModel)
        {
            if (jobTitleModel != null)
            {
                jobTitleModel.IdJobTitle = Guid.NewGuid();

                dbContext.JobTitles.Add(MapModelToDbObject(jobTitleModel));
                dbContext.SaveChanges();
            }
        }

        public void UpdateJobTitle(JobTitlesModel jobTitleModel)
        {
            JobTitle jobTitle = dbContext.JobTitles.FirstOrDefault(x => x.IdJobTitle == jobTitleModel.IdJobTitle);
            if (jobTitle != null)
            {
                jobTitle.JobTitle1 = jobTitleModel.JobTitle1;

                dbContext.JobTitles.Update(jobTitle);
                dbContext.SaveChanges();
            }
        }

        private JobTitle MapModelToDbObject(JobTitlesModel model)
        {
            JobTitle jobTitle = new JobTitle();

            jobTitle.IdJobTitle = model.IdJobTitle;
            jobTitle.JobTitle1 = model.JobTitle1;

            return jobTitle;
        }

        public void DeleteJobTitle(Guid id)
        {
            JobTitle jobTitle = dbContext.JobTitles.FirstOrDefault(x => x.IdJobTitle == id);

            if(jobTitle != null)
            {
                dbContext.JobTitles.Remove(jobTitle);
                dbContext.SaveChanges();
            }
        }

    }
}
