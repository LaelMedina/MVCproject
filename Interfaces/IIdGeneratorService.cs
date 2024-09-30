namespace MVCproyect.Interfaces
{
    public interface IIdGeneratorService
    {
        Task<int> GenerateNextIdAsync(string table);
    }
}
