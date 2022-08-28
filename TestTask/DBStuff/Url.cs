namespace TestTask.DBStuff
{
    public class Url : BaseModel
    {
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public string CreationDate { get; set; }
        public int LinkCount { get; set; } = 0;
    }
}
