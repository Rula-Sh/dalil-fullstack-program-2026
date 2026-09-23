namespace ThemeParkManagementSystem
{
    public class Visitor
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int NumberOfAccompanyingAdults { get; set; }
        public Tier Tier { get; set; }

        internal static int Count { get; set; }

        public Visitor(int id, int age, int height, int numberOfAccompanyingAdults, Tier tier)
        {
            Id = id;
            Age = age;
            Height = height;
            NumberOfAccompanyingAdults = numberOfAccompanyingAdults;
            Tier = tier;
            ParkSystem.Add(this);
            Count++;
        }
    }
}
