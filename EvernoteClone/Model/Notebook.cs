
namespace EvernoteClone.Model
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public interface HasId
    {
        public string Id { get; set; }
    }

    public class Notebook : HasId
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}
