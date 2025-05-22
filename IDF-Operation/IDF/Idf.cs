
namespace IDF_Operation.IDF
{
    internal class Idf
    {
        private DateTime _date = new DateTime(1948, 5, 26);
        private string? _commanderName;
        private List<StrikeOptions.StrikeOptions> _strikeOptions;
        public Idf(string? commanderName, List<StrikeOptions.StrikeOptions> strikeOptions)
        {
            this._commanderName = commanderName;
            this._strikeOptions = strikeOptions;
        }

        public List<StrikeOptions.StrikeOptions> GetStrikeOptions()
        {
            return this._strikeOptions;
        }

        public void addStrikeOptions(StrikeOptions.StrikeOptions option)
        {
            this._strikeOptions.Add(option);
        }

        public string printAvalibleOptions()
        {
            foreach (var option in this._strikeOptions)
            {
                if (option.isAvailable())
                {
                    return $"Attack unit name: {option.getName()}, " +
                        $"remaining capacity: {option.getCapacity()}";
                }
            }
            return "No available options";
        }
    }
}
