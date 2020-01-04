namespace GraphQL.Models
{
    public class Lesson
    {
        public int Id {get; set;}

        public string Name {get; set;}

        public Teacher Teacher {get; set;}
    }
}