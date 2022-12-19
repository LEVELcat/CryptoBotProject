namespace CryptoBotProject.Bot
{

    class SeanceKeeper
    {
        private static Dictionary<long, Seance> instances = new Dictionary<long, Seance>();

        public static Seance GetSeance(long id)
        {
            if (instances.ContainsKey(id) == false)
                instances.Add(id, new Seance());

            return instances[id];
        }

    }
}
