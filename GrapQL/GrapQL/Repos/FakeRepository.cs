using System.Collections.Generic;
using System.Linq;
using GraphQL.Models;

public interface ILessonRepository
{
    IEnumerable<Lesson> GetAll();

    Lesson GetById(int id);    
}

public class LessonRepository : ILessonRepository
{
    public IEnumerable<Lesson> GetAll()
    {
        return new List<Lesson> { 
            new Lesson { Name = "lesson", Id = 1, Teacher = new Teacher { FirstName = "On", SecondName = "Kub" } },
            new Lesson { Name = "lesson2", Id = 3 }
         };
    }

    public Lesson GetById(int id)
    {
        return GetAll().First(x => x.Id == id);
    }
}